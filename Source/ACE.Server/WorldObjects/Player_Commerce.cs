using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private void UpdateCoinValue(bool sendUpdateMessageIfChanged = true)
        {
            int coins = 0;

            foreach (var coinStack in GetInventoryItemsOfTypeWeenieType(WeenieType.Coin))
                coins += coinStack.Value ?? 0;

            if (sendUpdateMessageIfChanged && CoinValue == coins)
                sendUpdateMessageIfChanged = false;

            CoinValue = coins;

            if (sendUpdateMessageIfChanged)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CoinValue, CoinValue ?? 0));
        }

        private List<WorldObject> CreatePayoutCoinStacks(int amount)
        {
            var coinStacks = new List<WorldObject>();

            while (amount > 0)
            {
                var currencyStack = WorldObjectFactory.CreateNewWorldObject("coinstack");

                // payment contains a max stack
                if (currencyStack.MaxStackSize <= amount)
                {
                    currencyStack.SetStackSize(currencyStack.MaxStackSize);
                    coinStacks.Add(currencyStack);
                    amount -= currencyStack.MaxStackSize.Value;
                }
                else // not a full stack
                {
                    currencyStack.SetStackSize(amount);
                    coinStacks.Add(currencyStack);
                    amount -= amount;
                }
            }

            return coinStacks;
        }

        public int PreCheckItem(uint weenieClassId, int amount, int playerFreeContainerSlots, int playerFreeInventorySlots, int playerFreeAvailableBurden, out int requiredEncumbrance, out bool requiresBackpackSlot)
        {
            var itemStacks = 0;
            requiredEncumbrance = 0;
            requiresBackpackSlot = false;

            var item = DatabaseManager.World.GetCachedWeenie(weenieClassId);

            if (item != null)
            {
                var isVendorService = item.IsVendorService();
                if (isVendorService)
                    return 0;

                requiresBackpackSlot = item.RequiresBackpackSlotOrIsContainer();

                var isStackable = item.IsStackable();

                var itemStackUnitEncumbrance = isStackable ? (item.GetProperty(PropertyInt.StackUnitEncumbrance).HasValue ? item.GetProperty(PropertyInt.StackUnitEncumbrance) ?? 0 : item.GetProperty(PropertyInt.EncumbranceVal) ?? 0) : item.GetProperty(PropertyInt.EncumbranceVal) ?? 0;
                var itemStackMaxStackSize = isStackable ? item.GetProperty(PropertyInt.MaxStackSize) ?? 1 : 1;

                if (!isStackable)
                {
                    requiredEncumbrance = itemStackUnitEncumbrance;
                    return amount;
                }

                while (amount > 0)
                {
                    // amount contains a max stack
                    if (itemStackMaxStackSize <= amount)
                    {
                        itemStacks++;
                        requiredEncumbrance += itemStackUnitEncumbrance * itemStackMaxStackSize;
                        amount -= itemStackMaxStackSize;
                    }
                    else // not a full stack
                    {
                        itemStacks++;
                        requiredEncumbrance += itemStackUnitEncumbrance * amount;
                        amount -= amount;
                    }

                    if (itemStacks > playerFreeInventorySlots || requiredEncumbrance > playerFreeAvailableBurden)
                        break;
                }
            }

            return itemStacks;
        }

        private List<WorldObject> SpendCurrency(uint currencyWeenieClassId, uint amountToSpend, bool destroy = false)
        {
            if (currencyWeenieClassId == 0 || amountToSpend == 0)
                return null;

            var cost = new List<WorldObject>();

            if (currencyWeenieClassId == Vendor.CoinStackWCID)
            {
                if (amountToSpend > CoinValue)
                    return null;
            }

            if (destroy)
            {
                TryConsumeFromInventoryWithNetworking(currencyWeenieClassId, (int)amountToSpend);
            }
            else
            {
                cost = CollectCurrencyStacks(currencyWeenieClassId, amountToSpend);

                foreach (var stack in cost)
                    TryRemoveFromInventoryWithNetworking(stack.Guid, out _, RemoveFromInventoryAction.SpendItem);
            }

            return cost;
        }

        private List<WorldObject> CollectCurrencyStacks(uint currencyWeenieClassId, uint amountToSpend)
        {
            var currencyStacksCollected = new List<WorldObject>();

            var currencyStacksInInventory = GetInventoryItemsOfWCID(currencyWeenieClassId);
            //currencyStacksInInventory = currencyStacksInInventory.OrderBy(o => o.Value).ToList();

            var leftToCollect = (int)amountToSpend;
            foreach (var stack in currencyStacksInInventory)
            {
                var amountToRemove = Math.Min(leftToCollect, stack.StackSize ?? 1);
                if (stack.StackSize == amountToRemove)
                    currencyStacksCollected.Add(stack);
                else
                {
                    var newStack = WorldObjectFactory.CreateNewWorldObject(currencyWeenieClassId);
                    newStack.SetStackSize(amountToRemove);
                    currencyStacksCollected.Add(newStack);
                    var stackToAdjust = FindObject(stack.Guid, SearchLocations.MyInventory, out var foundInContainer, out var rootContainer, out _);
                    if (stackToAdjust != null)
                    {
                        AdjustStack(stackToAdjust, -amountToRemove, foundInContainer, rootContainer);
                        Session.Network.EnqueueSend(new GameMessageSetStackSize(stackToAdjust));
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));

                        if (stackToAdjust.WeenieType == WeenieType.Coin)
                            UpdateCoinValue();
                    }
                }

                leftToCollect -= amountToRemove;
                if (leftToCollect <= 0)
                    break;
            }

            return currencyStacksCollected;
        }


        // ===============================
        // Game Action Handlers - Buy Item
        // ===============================

        /// <summary>
        /// Fired from the client / client is sending us a Buy transaction to vendor
        /// </summary>
        /// <param name="vendorGuid"></param>
        /// <param name="items"></param>
        public void HandleActionBuyItem(uint vendorGuid, List<ItemProfile> items)
        {
            if (IsBusy)
            {
                SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            if (IsTrading)
            {
                SendUseDoneEvent(WeenieError.CantDoThatTradeInProgress);
                return;
            }

            var vendor = CurrentLandblock?.GetObject(vendorGuid) as Vendor;

            if (vendor != null)
                vendor.BuyItems_ValidateTransaction(items, this);

            SendUseDoneEvent();
        }

        /// <summary>
        /// Returns TRUE if player meets the gold / alternate currency costs for purchase
        /// </summary>
        public bool ValidateBuyTransaction(Vendor vendor, uint goldcost, uint altcost)
        {
            // validation
            var valid = true;

            if (goldcost > CoinValue)
                valid = false;

            if (altcost > 0)
            {
                var altCurrency = vendor.AlternateCurrency ?? 0;

                var numItems = GetNumInventoryItemsOfWCID(altCurrency);

                if (numItems < altcost)
                    valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Vendor has validated the transactions and sent a list of items for processing.
        /// </summary>
        public void FinalizeBuyTransaction(Vendor vendor, List<WorldObject> uqlist, List<WorldObject> genlist, uint goldcost, uint altcost)
        {
            // vendor accepted the transaction

            var valid = ValidateBuyTransaction(vendor, goldcost, altcost);

            if (valid)
            {
                if (altcost > 0)
                {
                    var altCurrencyWCID = vendor.AlternateCurrency ?? 0;

                    SpendCurrency(altCurrencyWCID, altcost, true);
                }
                else
                    SpendCurrency(Vendor.CoinStackWCID, goldcost, true);

                foreach (WorldObject wo in uqlist)
                {
                    wo.RemoveProperty(PropertyFloat.SoldTimestamp);
                    TryCreateInInventoryWithNetworking(wo);
                }

                foreach (var gen in genlist)
                {
                    var service = gen.GetProperty(PropertyBool.VendorService) ?? false;

                    if (!service)
                        TryCreateInInventoryWithNetworking(gen);
                    else
                    {
                        var spell = new Spell(gen.SpellDID ?? 0);
                        if (!spell.NotFound)
                        {
                            var preCastTime = vendor.PreCastMotion(this);
                            vendor.IsBusy = true;

                            var castChain = new ActionChain();
                            castChain.AddDelaySeconds(preCastTime);
                            castChain.AddAction(vendor, () =>
                            {
                                vendor.TryCastSpell(spell, this, vendor);
                                vendor.PostCastMotion();
                            });

                            var postCastTime = vendor.GetPostCastTime(spell);

                            castChain.AddDelaySeconds(postCastTime);
                            castChain.AddAction(vendor, () => vendor.IsBusy = false);

                            castChain.EnqueueChain();
                        }
                    }
                }

                Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.PickUpItem));

                if (PropertyManager.GetBool("player_receive_immediate_save").Item)
                    RushNextPlayerSave(5);
            }

            vendor.BuyItems_FinalTransaction(this, uqlist, valid, altcost);
        }


        public List<ItemProfile> VerifySellItems(List<ItemProfile> sellItems, Vendor vendor)
        {
            var uniques = new HashSet<uint>();

            var verified = new List<ItemProfile>();

            foreach (var sellItem in sellItems)
            {
                var wo = FindObject(sellItem.ObjectGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems);

                if (wo == null)
                {
                    log.Warn($"{Name} tried to sell item {sellItem.ObjectGuid:X8} not in their inventory to {vendor.Name}");
                    continue;
                }

                if (uniques.Contains(sellItem.ObjectGuid))
                {
                    log.Warn($"{Name} tried to sell duplicate item {wo.Name} ({wo.Guid}) to {vendor.Name}");
                    continue;
                }

                if (sellItem.Amount > (wo.StackSize ?? 1))
                {
                    log.Warn($"{Name} tried to sell {sellItem.Amount}x {wo.Name} ({wo.Guid}) to {vendor.Name}, but they only have {wo.StackSize ?? 1}x");
                    continue;
                }

                uniques.Add(sellItem.ObjectGuid);

                verified.Add(sellItem);
            }

            return verified;
        }

        // ================================
        // Game Action Handlers - Sell Item
        // ================================

        private const uint coinStackWeenieClassId = 273;

        /// <summary>
        /// Client Calls this when Sell is clicked.
        /// </summary>
        public void HandleActionSellItem(List<ItemProfile> itemprofiles, uint vendorGuid)
        {
            if (IsBusy)
            {
                SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            var vendor = CurrentLandblock?.GetObject(vendorGuid) as Vendor;

            if (vendor == null)
            {
                SendUseDoneEvent(WeenieError.NoObject);
                return;
            }

            itemprofiles = VerifySellItems(itemprofiles, vendor);

            var allPossessions = GetAllPossessions();

            var sellList = new List<WorldObject>();

            var acceptedItemTypes = (ItemType)(vendor.MerchandiseItemTypes ?? 0);

            foreach (ItemProfile profile in itemprofiles)
            {
                var item = allPossessions.FirstOrDefault(i => i.Guid.Full == profile.ObjectGuid);

                if (item == null)
                    continue;

                if ((acceptedItemTypes & item.ItemType) == 0 || !item.IsSellable || item.Retained)
                {
                    var itemName = (item.StackSize ?? 1) > 1 ? item.GetPluralName() : item.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"The {itemName} is unsellable.")); // retail message did not include item name, leaving in that for now.

                    continue;
                }

                if (item.Value < 1)
                {
                    var itemName = (item.StackSize ?? 1) > 1 ? item.GetPluralName() : item.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"The {itemName} has no value and cannot be sold.")); // retail message did not include item name, leaving in that for now.

                    continue;
                }

                if (IsTrading && ItemsInTradeWindow.Contains(item.Guid))
                {
                    var itemName = (item.StackSize ?? 1) > 1 ? item.GetPluralName() : item.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"You cannot sell that! The {itemName} is currently being traded.")); // custom message?

                    continue;
                }

                sellList.Add(item);
            }

            if (sellList.Count == 0)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, Guid.Full));
                SendUseDoneEvent(WeenieError.None);
                return;
            }

            var payoutCoinAmount = vendor.CalculatePayoutCoinAmount(sellList);

            if (payoutCoinAmount < 0)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Transaction failed."));
                log.Warn($"{Name} (0x({Guid}) tried to sell something to {vendor.Name} (0x{vendor.Guid}) resulting in a payout of {payoutCoinAmount} pyreals.");
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, Guid.Full));
                SendUseDoneEvent();
                return;
            }

            var playerFreeInventorySlots = GetFreeInventorySlots();
            var playerAvailableBurden = GetAvailableBurden();

            var numberOfCoinStacksToCreate = PreCheckItem(coinStackWeenieClassId, payoutCoinAmount, 0, GetFreeInventorySlots(), GetAvailableBurden(), out var totalEncumburanceOfCoinStacks, out _);

            var playerDoesNotHaveEnoughPackSpace = playerFreeInventorySlots < numberOfCoinStacksToCreate;
            var playerDoesNotHaveEnoughBurdenCapacity = playerAvailableBurden < totalEncumburanceOfCoinStacks;

            if (playerDoesNotHaveEnoughPackSpace || playerDoesNotHaveEnoughBurdenCapacity)
            {
                if (playerDoesNotHaveEnoughBurdenCapacity)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You are too encumbered to sell that!"));
                else // if (playerDoesNotHaveEnoughPackSpace)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You do not have enough free pack space to sell that!"));
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, Guid.Full));
                SendUseDoneEvent();
                return;
            }

            var payoutCoinStacks = CreatePayoutCoinStacks(payoutCoinAmount);

            // Make sure we have enough pack space for the payout
            if (GetFreeInventorySlots() + sellList.Count - payoutCoinStacks.Count < 0)
            {
                Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "Not enough inventory space!")); // TODO: find retail messages
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, Guid.Full));

                foreach (var item in payoutCoinStacks)
                    item.Destroy();

                SendUseDoneEvent(WeenieError.FullInventoryLocation);
                return;
            }

            // Remove the items we're selling from our inventory
            foreach (var item in sellList)
            {
                if (TryRemoveFromInventoryWithNetworking(item.Guid, out _, RemoveFromInventoryAction.SellItem) || TryDequipObjectWithNetworking(item.Guid, out _, DequipObjectAction.SellItem))
                    Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, vendor));
                else
                    log.WarnFormat("Item 0x{0:X8}:{1} for player {2} not found in HandleActionSellItem.", item.Guid.Full, item.Name, Name); // This shouldn't happen
            }

            // Send the list of items to the vendor to complete the transaction
            vendor.ProcessItemsForPurchase(this, sellList);

            // Add the payout to inventory
            foreach (var item in payoutCoinStacks)
            {
                if (!TryCreateInInventoryWithNetworking(item)) // This shouldn't happen
                {
                    log.WarnFormat("Payout 0x{0:X8}:{1} for player {2} failed to add to inventory HandleActionSellItem.", item.Guid.Full, item.Name, Name);
                    item.Destroy();
                }
            }

            UpdateCoinValue(false);

            Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.PickUpItem));

            SendUseDoneEvent();
        }
    }
}
