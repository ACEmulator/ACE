using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Factories;
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
            const uint coinWeenieId = 273;

            var coinStacks = new List<WorldObject>();

            while (amount > 0)
            {
                var currencyStack = WorldObjectFactory.CreateNewWorldObject(coinWeenieId);

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

        private List<WorldObject> SpendCurrency(uint amount, WeenieType type)
        {
            if (type == WeenieType.Coin && amount > CoinValue)
                return null;

            List<WorldObject> currency = new List<WorldObject>();
            currency.AddRange(GetInventoryItemsOfTypeWeenieType(type));
            currency = currency.OrderBy(o => o.Value).ToList();

            List<WorldObject> cost = new List<WorldObject>();
            uint payment = 0;

            WorldObject changeobj = WorldObjectFactory.CreateNewWorldObject(273);
            uint change = 0;

            foreach (WorldObject wo in currency)
            {
                if (payment + wo.StackSize.Value <= amount)
                {
                    // add to payment
                    payment = payment + (uint)wo.StackSize.Value;
                    cost.Add(wo);
                }
                else if (payment + wo.StackSize.Value > amount)
                {
                    // add payment
                    payment = payment + (uint)wo.StackSize.Value;
                    cost.Add(wo);
                    // calculate change
                    if (payment > amount)
                    {
                        change = payment - amount;
                        // add new change object.
                        changeobj.SetStackSize((int)change);
                        wo.SetStackSize(wo.StackSize - (int)change);
                    }
                    break;
                }
                else if (payment == amount)
                    break;
            }

            // destroy all stacks of currency required / sale
            foreach (WorldObject wo in cost)
                TryConsumeFromInventoryWithNetworking(wo);

            // if there is change - readd - do this at the end to try to prevent exploiting
            if (change > 0)
                TryCreateInInventoryWithNetworking(changeobj);

            UpdateCoinValue(false);

            return cost;
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
            var vendor = CurrentLandblock?.GetObject(vendorGuid) as Vendor;

            if (vendor == null)
            {
                SendUseDoneEvent();
                return;
            }

            vendor.BuyItems_ValidateTransaction(items, this);

            SendUseDoneEvent();
        }

        /// <summary>
        /// Vendor has validated the transactions and sent a list of items for processing.
        /// </summary>
        public void FinalizeBuyTransaction(Vendor vendor, List<WorldObject> uqlist, List<WorldObject> genlist, bool valid, uint goldcost)
        {
            // todo research packets more for both buy and sell. ripley thinks buy is update..
            // vendor accepted the transaction
            if (valid)
            {
                if (SpendCurrency(goldcost, WeenieType.Coin) != null)
                {
                    foreach (WorldObject wo in uqlist)
                        TryCreateInInventoryWithNetworking(wo);

                    foreach (var gen in genlist)
                    {
                        var service = gen.GetProperty(PropertyBool.VendorService) ?? false;

                        if (!service)
                            TryCreateInInventoryWithNetworking(gen);
                        else
                        {
                            var spell = new Spell(gen.SpellDID ?? 0);
                            TryCastSpell(spell, this, null, false, false);
                        }
                    }

                    Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.PickUpItem));
                }
                else // not enough cash.
                {
                    valid = false;
                }
            }

            vendor.BuyItems_FinalTransaction(this, uqlist, valid);
        }


        // ================================
        // Game Action Handlers - Sell Item
        // ================================

        /// <summary>
        /// Client Calls this when Sell is clicked.
        /// </summary>
        public void HandleActionSellItem(List<ItemProfile> itemprofiles, uint vendorGuid)
        {
            var vendor = CurrentLandblock?.GetObject(vendorGuid) as Vendor;

            if (vendor == null)
            {
                SendUseDoneEvent(WeenieError.NoObject);
                return;
            }

            var allPossessions = GetAllPossessions();

            var sellList = new List<WorldObject>();

            foreach (ItemProfile profile in itemprofiles)
            {
                var item = allPossessions.FirstOrDefault(i => i.Guid.Full == profile.ObjectGuid);

                if (item == null)
                    continue;

                if (!(item.GetProperty(PropertyBool.IsSellable) ?? true) || (item.GetProperty(PropertyBool.Retained) ?? false))
                {
                    var itemName = (item.StackSize ?? 1) > 1 ? item.GetPluralName() : item.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"The {itemName} cannot be sold")); // TODO: find retail messages
                    Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, item.Guid.Full));

                    continue;
                }

                sellList.Add(item);
            }

            if (sellList.Count == 0)
            {
                SendUseDoneEvent(WeenieError.NoObject);
                return;
            }

            var payoutCoinAmount = vendor.CalculatePayoutCoinAmount(sellList);

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
                    Session.Network.EnqueueSend(new GameMessageDeleteObject(item));
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
