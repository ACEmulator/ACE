using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        // player buying items from vendor

        /// <summary>
        /// Called when player clicks 'Buy Items'
        /// </summary>
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

            if (vendor == null)
            {
                SendUseDoneEvent(WeenieError.NoObject);
                return;
            }

            // if this succeeds, it automatically calls player.FinalizeBuyTransaction()
            vendor.BuyItems_ValidateTransaction(items, this);

            SendUseDoneEvent();
        }

        private static readonly uint coinStackWcid = (uint)ACE.Entity.Enum.WeenieClassName.W_COINSTACK_CLASS;

        /// <summary>
        /// Vendor has validated the transactions and sent a list of items for processing.
        /// </summary>
        public void FinalizeBuyTransaction(Vendor vendor, List<WorldObject> genericItems, List<WorldObject> uniqueItems, uint cost)
        {
            // transaction has been validated by this point

            var currencyWcid = vendor.AlternateCurrency ?? coinStackWcid;

            SpendCurrency(currencyWcid, cost, true);

            vendor.MoneyIncome += (int)cost;

            foreach (var item in genericItems)
            {
                var service = item.GetProperty(PropertyBool.VendorService) ?? false;

                if (!service)
                {
                    // errors shouldn't be possible here, since the items were pre-validated, but just in case...
                    if (!TryCreateInInventoryWithNetworking(item))
                    {
                        log.Error($"[VENDOR] {Name}.FinalizeBuyTransaction({vendor.Name}) - couldn't add {item.Name} ({item.Guid}) to player inventory after validation, this shouldn't happen!");

                        item.Destroy();  // cleanup for guid manager
                    }

                    vendor.NumItemsSold++;
                }
                else
                    vendor.ApplyService(item, this);
            }

            foreach (var item in uniqueItems)
            {
                if (TryCreateInInventoryWithNetworking(item))
                {
                    vendor.UniqueItemsForSale.Remove(item.Guid);

                    // this was only for when the unique item was sold to the vendor,
                    // to determine when the item should rot on the vendor. it gets removed now
                    item.SoldTimestamp = null;

                    vendor.NumItemsSold++;
                }
                else
                    log.Error($"[VENDOR] {Name}.FinalizeBuyTransaction({vendor.Name}) - couldn't add {item.Name} ({item.Guid}) to player inventory after validation, this shouldn't happen!");
            }

            Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.PickUpItem));

            if (PropertyManager.GetBool("player_receive_immediate_save").Item)
                RushNextPlayerSave(5);

            var altCurrencySpent = vendor.AlternateCurrency != null ? cost : 0;

            vendor.ApproachVendor(this, VendorType.Buy, altCurrencySpent);
        }

        // player selling items to vendor

        // whereas most of the logic for buying items is in vendor,
        // most of the logic for selling items is located in player_commerce
        // the functions have similar structure, just in different places
        // there's really no point in there being differences in location,
        // and it might be better to move them all to vendor for consistency.

        /// <summary>
        /// Called when player clicks 'Sell Items'
        /// </summary>
        public void HandleActionSellItem(uint vendorGuid, List<ItemProfile> itemProfiles)
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

            // perform validations on requested sell items,
            // and filter to list of validated items

            // one difference between sell and buy is here.
            // when an itemProfile is invalid in buy, the entire transaction is failed immediately.
            // when an itemProfile is invalid in sell, we just remove the invalid itemProfiles, and continue onwards
            // this might not be the best for safety, and it's a tradeoff between safety and player convenience
            // should we fail the entire transaction (similar to buy), if there are any invalids in the transaction request?

            var sellList = VerifySellItems(itemProfiles, vendor);

            if (sellList.Count == 0)
            {
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, Guid.Full));
                SendUseDoneEvent();
                return;
            }

            // calculate pyreals to receive
            var payoutCoinAmount = vendor.CalculatePayoutCoinAmount(sellList);

            if (payoutCoinAmount < 0)
            {
                log.Warn($"[VENDOR] {Name} (0x({Guid}) tried to sell something to {vendor.Name} (0x{vendor.Guid}) resulting in a payout of {payoutCoinAmount} pyreals.");

                SendTransientError("Transaction failed.");
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, Guid.Full));

                SendUseDoneEvent();

                return;
            }

            // verify player has enough pack slots / burden to receive these pyreals
            var itemsToReceive = new ItemsToReceive(this);

            itemsToReceive.Add((uint)ACE.Entity.Enum.WeenieClassName.W_COINSTACK_CLASS, payoutCoinAmount);

            if (itemsToReceive.PlayerExceedsLimits)
            {
                if (itemsToReceive.PlayerExceedsAvailableBurden)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You are too encumbered to sell that!"));
                else if (itemsToReceive.PlayerOutOfInventorySlots)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You do not have enough free pack space to sell that!"));

                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, Guid.Full));
                SendUseDoneEvent();     // WeenieError.FullInventoryLocation?
                return;
            }

            var payoutCoinStacks = CreatePayoutCoinStacks(payoutCoinAmount);

            vendor.MoneyOutflow += payoutCoinAmount;

            // remove sell items from player inventory
            foreach (var item in sellList.Values)
            {
                if (TryRemoveFromInventoryWithNetworking(item.Guid, out _, RemoveFromInventoryAction.SellItem) || TryDequipObjectWithNetworking(item.Guid, out _, DequipObjectAction.SellItem))
                    Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, item, vendor));
                else
                    log.WarnFormat("[VENDOR] Item 0x{0:X8}:{1} for player {2} not found in HandleActionSellItem.", item.Guid.Full, item.Name, Name); // This shouldn't happen
            }

            // send the list of items to the vendor
            // for the vendor to determine what to do with each item (resell, destroy)
            vendor.ProcessItemsForPurchase(this, sellList);

            // add coins to player inventory
            foreach (var item in payoutCoinStacks)
            {
                if (!TryCreateInInventoryWithNetworking(item))  // this shouldn't happen because of pre-validations in itemsToReceive
                {
                    log.WarnFormat("[VENDOR] Payout 0x{0:X8}:{1} for player {2} failed to add to inventory HandleActionSellItem.", item.Guid.Full, item.Name, Name);
                    item.Destroy();
                }
            }

            // UpdateCoinValue removed -- already handled in TryCreateInInventoryWithNetworking

            Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.PickUpItem));

            SendUseDoneEvent();
        }

        /// <summary>
        /// Filters the list of ItemProfiles the player is attempting to sell to the vendor
        /// to the list of verified WorldObjects in the player's inventory w/ validations
        /// </summary>
        private Dictionary<uint, WorldObject> VerifySellItems(List<ItemProfile> sellItems, Vendor vendor)
        {
            var allPossessions = GetAllPossessions().ToDictionary(i => i.Guid.Full, i => i);

            var acceptedItemTypes = (ItemType)(vendor.MerchandiseItemTypes ?? 0);

            var verified = new Dictionary<uint, WorldObject>();

            foreach (var sellItem in sellItems)
            {
                if (!allPossessions.TryGetValue(sellItem.ObjectGuid, out var wo))
                {
                    log.Warn($"[VENDOR] {Name} tried to sell item {sellItem.ObjectGuid:X8} not in their inventory to {vendor.Name}");
                    continue;
                }

                // verify item profile (unique guids, amount)
                if (verified.ContainsKey(wo.Guid.Full))
                {
                    log.Warn($"[VENDOR] {Name} tried to sell duplicate item {wo.Name} ({wo.Guid}) to {vendor.Name}");
                    continue;
                }

                if (!sellItem.IsValidAmount)
                {
                    log.Warn($"[VENDOR] {Name} tried to sell {sellItem.Amount}x {wo.Name} ({wo.Guid}) to {vendor.Name}");
                    continue;
                }

                if (sellItem.Amount > (wo.StackSize ?? 1))
                {
                    log.Warn($"[VENDOR] {Name} tried to sell {sellItem.Amount}x {wo.Name} ({wo.Guid}) to {vendor.Name}, but they only have {wo.StackSize ?? 1}x");
                    continue;
                }

                // verify wo / vendor / player properties
                if ((acceptedItemTypes & wo.ItemType) == 0 || !wo.IsSellable || wo.Retained)
                {
                    var itemName = (wo.StackSize ?? 1) > 1 ? wo.GetPluralName() : wo.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"The {itemName} is unsellable.")); // retail message did not include item name, leaving in that for now.
                    continue;
                }

                if (wo.Value < 1)
                {
                    var itemName = (wo.StackSize ?? 1) > 1 ? wo.GetPluralName() : wo.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"The {itemName} has no value and cannot be sold.")); // retail message did not include item name, leaving in that for now.
                    continue;
                }

                if (IsTrading && wo.IsBeingTradedOrContainsItemBeingTraded(ItemsInTradeWindow))
                {
                    var itemName = (wo.StackSize ?? 1) > 1 ? wo.GetPluralName() : wo.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"You cannot sell that! The {itemName} is currently being traded.")); // custom message?
                    continue;
                }

                if (wo is Container container && container.Inventory.Count > 0)
                {
                    var itemName = (wo.StackSize ?? 1) > 1 ? wo.GetPluralName() : wo.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"You cannot sell that! The {itemName} must be empty.")); // custom message?
                    continue;
                }

                verified.Add(wo.Guid.Full, wo);
            }

            return verified;
        }

        private List<WorldObject> CreatePayoutCoinStacks(int amount)
        {
            var coinStacks = new List<WorldObject>();

            while (amount > 0)
            {
                var currencyStack = WorldObjectFactory.CreateNewWorldObject("coinstack");

                var currentStackAmount = Math.Min(amount, currencyStack.MaxStackSize.Value);

                currencyStack.SetStackSize(currentStackAmount);
                coinStacks.Add(currencyStack);
                amount -= currentStackAmount;
            }
            return coinStacks;
        }

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

        private List<WorldObject> SpendCurrency(uint currentWcid, uint amount, bool destroy = false)
        {
            if (currentWcid == 0 || amount == 0)
                return null;

            var cost = new List<WorldObject>();

            if (currentWcid == coinStackWcid)
            {
                if (amount > CoinValue)
                    return null;
            }
            if (destroy)
            {
                TryConsumeFromInventoryWithNetworking(currentWcid, (int)amount);
            }
            else
            {
                cost = CollectCurrencyStacks(currentWcid, amount);

                foreach (var stack in cost)
                {
                    if (!TryRemoveFromInventoryWithNetworking(stack.Guid, out _, RemoveFromInventoryAction.SpendItem))
                        UpdateCoinValue(); // this coinstack was created by spliting up an existing one, and not actually added to the players inventory. The existing stack was already adjusted down but we need to update the player's CoinValue, so we do that now.
                }
            }
            return cost;
        }

        private List<WorldObject> CollectCurrencyStacks(uint currencyWcid, uint amount)
        {
            var currencyStacksCollected = new List<WorldObject>();

            var currencyStacksInInventory = GetInventoryItemsOfWCID(currencyWcid);
            //currencyStacksInInventory = currencyStacksInInventory.OrderBy(o => o.Value).ToList();

            var remaining = (int)amount;

            foreach (var stack in currencyStacksInInventory)
            {
                var amountToRemove = Math.Min(remaining, stack.StackSize ?? 1);

                if (stack.StackSize == amountToRemove)
                {
                    currencyStacksCollected.Add(stack);
                }
                else
                {
                    // create new stack
                    var newStack = WorldObjectFactory.CreateNewWorldObject(currencyWcid);
                    newStack.SetStackSize(amountToRemove);
                    currencyStacksCollected.Add(newStack);

                    var stackToAdjust = FindObject(stack.Guid, SearchLocations.MyInventory, out var foundInContainer, out var rootContainer, out _);

                    // adjust existing stack
                    if (stackToAdjust != null)
                    {
                        AdjustStack(stackToAdjust, -amountToRemove, foundInContainer, rootContainer);
                        Session.Network.EnqueueSend(new GameMessageSetStackSize(stackToAdjust));
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.EncumbranceVal, EncumbranceVal ?? 0));
                    }
                    // UpdateCoinValue removed -- already called upstream
                }

                remaining -= amountToRemove;

                if (remaining <= 0)
                    break;
            }
            return currencyStacksCollected;
        }
    }
}
