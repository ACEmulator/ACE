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

            foreach (var possession in GetAllPossessions())
            {
                if (possession.WeenieType == WeenieType.Coin)
                    coins += possession.Value ?? 0;
            }

            if (sendUpdateMessageIfChanged && CoinValue == coins)
                sendUpdateMessageIfChanged = false;

            CoinValue = coins;

            if (sendUpdateMessageIfChanged)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CoinValue, CoinValue ?? 0));
        }

        // todo re-think how this works..
        private void UpdateCurrencyClientCalculations(WeenieType type)
        {
            int coins = 0;
            List<WorldObject> currency = new List<WorldObject>();
            currency.AddRange(GetInventoryItemsOfTypeWeenieType(type));
            foreach (WorldObject wo in currency)
            {
                if (wo.WeenieType == WeenieType.Coin)
                    coins += wo.StackSize.Value;
            }
            // send packet to client letthing them know
            CoinValue = coins;
        }


        private bool CreateCurrency(WeenieType type, uint amount)
        {
            // todo: we need to look up this object to understand it by its weenie id.
            // todo: support more then hard coded coin.
            const uint coinWeenieId = 273;
            WorldObject wochk = WorldObjectFactory.CreateNewWorldObject(coinWeenieId);
            ushort maxstacksize = wochk.MaxStackSize.Value;

            List<WorldObject> payout = new List<WorldObject>();

            while (amount > 0)
            {
                WorldObject currancystack = WorldObjectFactory.CreateNewWorldObject(coinWeenieId);
                // payment contains a max stack
                if (maxstacksize <= amount)
                {
                    currancystack.StackSize = maxstacksize;
                    payout.Add(currancystack);
                    amount = amount - maxstacksize;
                }
                else // not a full stack
                {
                    currancystack.StackSize = (ushort)amount;
                    payout.Add(currancystack);
                    amount = amount - amount;
                }
            }

            // add money to player inventory.
            foreach (WorldObject wo in payout)
            {
                TryCreateInInventoryWithNetworking(wo);
            }
            UpdateCurrencyClientCalculations(WeenieType.Coin);
            return true;
        }

        private List<WorldObject> SpendCurrency(uint amount, WeenieType type)
        {
            if (CoinValue - amount >= 0)
            {
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
                            changeobj.StackSize = (ushort)change;
                            wo.StackSize -= (ushort)change;
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
                {
                    TryCreateInInventoryWithNetworking(changeobj);
                }

                UpdateCurrencyClientCalculations(WeenieType.Coin);
                return cost;
            }
            return null;
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
                        {
                            TryCreateInInventoryWithNetworking(gen);
                        }
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

        public void FinalizeSellTransaction(WorldObject vendor, bool valid, List<WorldObject> purchaselist, uint payout)
        {
            // pay player in voinds
            if (valid)
            {
                CreateCurrency(WeenieType.Coin, payout);

                Session.Network.EnqueueSend(new GameMessageSound(Guid, Sound.PickUpItem));
            }
        }


        // =========================================
        // Game Action Handlers
        // =========================================

        /// <summary>
        /// Fired from the client / client is sending us a Buy transaction to vendor
        /// </summary>
        /// <param name="vendorGuid"></param>
        /// <param name="items"></param>
        public void HandleActionBuyItem(uint vendorGuid, List<ItemProfile> items)
        {
            var vendor = (CurrentLandblock?.GetObject(vendorGuid) as Vendor);

            if (vendor != null)
                vendor.BuyItems_ValidateTransaction(vendorGuid, items, this);

            SendUseDoneEvent();
        }

        /// <summary>
        /// Client Calls this when Sell is clicked.
        /// </summary>
        public void HandleActionSellItem(List<ItemProfile> itemprofiles, uint vendorGuid)
        {
            var sellList = new List<WorldObject>();

            var allPossessions = GetAllPossessions();
            var rejected = new List<WorldObject>();

            foreach (ItemProfile profile in itemprofiles)
            {
                var item = allPossessions.FirstOrDefault(i => i.Guid.Full == profile.ObjectGuid);

                if (item == null) continue;

                if (!(item.GetProperty(PropertyBool.IsSellable) ?? true) || (item.GetProperty(PropertyBool.Retained) ?? false))
                {
                    rejected.Add(item);
                    continue;
                }

                if (TryRemoveFromInventoryWithNetworking(profile.ObjectGuid, out item, RemoveFromInventoryAction.SellItem) || TryDequipObjectWithNetworking(profile.ObjectGuid, out item, DequipObjectAction.SellItem))
                {
                    Session.Network.EnqueueSend(new GameMessageDeleteObject(item));

                    sellList.Add(item);
                }
                else
                {
                    // todo give the client an error message
                }
            }

            if (rejected.Count > 0)
            {
                foreach (var item in rejected)
                {
                    var itemName = (item.StackSize ?? 1) > 1 ? item.GetPluralName() : item.Name;
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, $"The {itemName} cannot be sold"));     // TODO: find retail messages
                }
                Session.Network.EnqueueSend(new GameEventInventoryServerSaveFailed(Session, Guid.Full));
            }

            if (sellList.Count > 0)
            {
                var vendor = CurrentLandblock?.GetObject(vendorGuid) as Vendor;

                if (vendor != null)
                    vendor.SellItems_ValidateTransaction(this, sellList);
            }

            SendUseDoneEvent();
        }
    }
}
