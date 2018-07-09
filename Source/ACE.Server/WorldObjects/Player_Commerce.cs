using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
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
            wochk = null;

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
                AddNewWorldObjectToInventory(wo);
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
                        payment = payment + wo.StackSize.Value;
                        cost.Add(wo);
                    }
                    else if (payment + wo.StackSize.Value > amount)
                    {
                        // add payment
                        payment = payment + wo.StackSize.Value;
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
                {
                    TryRemoveFromInventoryWithNetworking(wo);

                    /*TryRemoveFromInventory(wo.Guid);
                    ObjectGuid clearContainer = new ObjectGuid(0);
                    Session.Network.EnqueueSend(new GameMessagePublicUpdateInstanceID(wo, PropertyInstanceId.Container, clearContainer));

                    // clean up the shard database.
                    throw new NotImplementedException();
                    // todo fix for EF
                    //DatabaseManager.Shard.DeleteObject(wo.SnapShotOfAceObject(), null);*/
                    Session.Network.EnqueueSend(new GameMessageDeleteObject(wo));
                }

                // if there is change - readd - do this at the end to try to prevent exploiting
                if (change > 0)
                {
                    AddNewWorldObjectToInventory(changeobj);
                }

                UpdateCurrencyClientCalculations(WeenieType.Coin);
                return cost;
            }
            return null;
        }


        /// <summary>
        /// Sends updated network packets to client / vendor item list.
        /// </summary>
        public void ApproachVendor(Vendor vendor, List<WorldObject> itemsForSale)
        {
            Session.Network.EnqueueSend(new GameEventApproachVendor(Session, vendor, itemsForSale));

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
                    {
                        wo.ContainerId = Guid.Full;
                        wo.PlacementPosition = 0;
                        AddToInventory(wo);
                        Session.Network.EnqueueSend(new GameMessageCreateObject(wo));
                        Session.Network.EnqueueSend(new GameEventItemServerSaysContainId(Session, wo, this));
                        Session.Network.EnqueueSend(new GameMessagePublicUpdateInstanceID(wo, PropertyInstanceId.Container, Guid));
                    }

                    foreach (var gen in genlist)
                        AddNewWorldObjectToInventory(gen);
                }
                else // not enough cash.
                {
                    valid = false;
                }
            }

            vendor.BuyItemsFinalTransaction(this, uqlist, valid);
        }

        public void FinalizeSellTransaction(WorldObject vendor, bool valid, List<WorldObject> purchaselist, uint payout)
        {
            // pay player in voinds
            if (valid)
            {
                CreateCurrency(WeenieType.Coin, payout);
            }
        }


        // =========================================
        // Game Action Handlers
        // =========================================

        /// <summary>
        /// Fired from the client / client is sending us a Buy transaction to vendor
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="items"></param>
        public void HandleActionBuyItem(ObjectGuid vendorId, List<ItemProfile> items)
        {
            var vendor = (CurrentLandblock?.GetObject(vendorId) as Vendor);

            if (vendor != null)
                vendor.BuyValidateTransaction(vendorId, items, this);
        }

        /// <summary>
        /// Client Calls this when Sell is clicked.
        /// </summary>
        public void HandleActionSellItem(List<ItemProfile> itemprofiles, ObjectGuid vendorId)
        {
            var purchaselist = new List<WorldObject>();

            foreach (ItemProfile profile in itemprofiles)
            {
                // check packs of item.
                WorldObject item = GetInventoryItem(profile.Guid);

                if (item == null)
                {
                    // check to see if this item is wielded
                    item = GetWieldedItem(profile.Guid);

                    if (item != null)
                    {
                        TryDequipObject(item.Guid);

                        Session.Network.EnqueueSend(
                           new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                           new GameMessageObjDescEvent(this),
                           new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, new ObjectGuid(0)),
                           new GameMessagePublicUpdatePropertyInt(item, PropertyInt.CurrentWieldedLocation, 0));
                    }
                }
                else
                {
                    // remove item from inventory.
                    TryRemoveFromInventory(profile.Guid);
                }

                //Session.Network.EnqueueSend(new GameMessagePrivateUpdateInstanceId(profile, PropertyInstanceId.Container, new ObjectGuid(0).Full));

                item.SetPropertiesForVendor();

                // clean up the shard database.
                throw new NotImplementedException();
                // todo fix for EF
                //DatabaseManager.Shard.DeleteObject(item.SnapShotOfAceObject(), null);

                Session.Network.EnqueueSend(new GameMessageDeleteObject(item));
                purchaselist.Add(item);
            }

            var vendor = CurrentLandblock?.GetObject(vendorId) as Vendor;

            if (vendor != null)
                vendor.SellItemsValidateTransaction(this, purchaselist);
        }
    }
}
