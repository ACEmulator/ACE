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
        /// <summary>
        /// Sends updated network packets to client / vendor item list.
        /// </summary>
        public void ApproachVendor(Vendor vendor, List<WorldObject> itemsForSale)
        {
            Session.Network.EnqueueSend(new GameEventApproachVendor(Session, vendor, itemsForSale));
            SendUseDoneEvent();
        }

        /// <summary>
        /// Fired from the client / client is sending us a Buy transaction to vendor
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="items"></param>
        public void BuyFromVendor(ObjectGuid vendorId, List<ItemProfile> items)
        {
            Vendor vendor = (CurrentLandblock.GetObject(vendorId) as Vendor);
            vendor.BuyValidateTransaction(vendorId, items, this);
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
                if (SpendCurrency(goldcost, WeenieType.Coin))
                {
                    foreach (WorldObject wo in uqlist)
                    {
                        wo.ContainerId = (int)Guid.Full;
                        wo.PlacementPosition = 0;
                        AddToInventory(wo);
                        Session.Network.EnqueueSend(new GameMessageCreateObject(wo));
                        Session.Network.EnqueueSend(new GameMessagePutObjectInContainer(Session, Guid, wo, 0));
                        Session.Network.EnqueueSend(new GameMessageUpdateInstanceId(Guid, wo.Guid, PropertyInstanceId.Container));
                    }
                    HandleAddNewWorldObjectsToInventory(genlist);
                }
                else // not enough cash.
                {
                    valid = false;
                }
            }

            vendor.BuyItemsFinalTransaction(this, uqlist, valid);
        }

        /// <summary>
        /// Client Calls this when Sell is clicked.
        /// </summary>
        public void SellToVendor(List<ItemProfile> itemprofiles, ObjectGuid vendorId)
        {
            List<WorldObject> purchaselist = new List<WorldObject>();
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
                        TryDequipObject(item.Guid, out WorldObject _);
                        Session.Network.EnqueueSend(
                           new GameMessageSound(Guid, Sound.WieldObject, (float)1.0),
                           new GameMessageObjDescEvent(this),
                           new GameMessageUpdateInstanceId(Guid, new ObjectGuid(0), PropertyInstanceId.Wielder),
                           new GameMessagePublicUpdatePropertyInt(Sequences, item.Guid, PropertyInt.CurrentWieldedLocation, 0));
                    }
                }
                else
                {
                    // remove item from inventory.
                    TryRemoveFromInventory(profile.Guid, out WorldObject _);
                }

                Session.Network.EnqueueSend(new GameMessageUpdateInstanceId(profile.Guid, new ObjectGuid(0), PropertyInstanceId.Container));

                item.SetPropertiesForVendor();

                // clean up the shard database.
                throw new NotImplementedException();
                // todo fix for EF
                //DatabaseManager.Shard.DeleteObject(item.SnapShotOfAceObject(), null);

                Session.Network.EnqueueSend(new GameMessageRemoveObject(item));
                purchaselist.Add(item);
            }

            Vendor vendor = CurrentLandblock.GetObject(vendorId) as Vendor;
            vendor.SellItemsValidateTransaction(this, purchaselist);
        }

        public void FinalizeSellTransaction(WorldObject vendor, bool valid, List<WorldObject> purchaselist, uint payout)
        {
            // pay player in voinds
            if (valid)
            {
                CreateCurrency(WeenieType.Coin, payout);
            }
        }


        public bool CreateCurrency(WeenieType type, uint amount)
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
                HandleAddNewWorldObjectToInventory(wo);
            }
            UpdateCurrencyClientCalculations(WeenieType.Coin);
            return true;
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

        private bool SpendCurrency(uint amount, WeenieType type)
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
                        }
                        break;
                    }
                    else if (payment == amount)
                        break;
                }

                // destroy all stacks of currency required / sale
                foreach (WorldObject wo in cost)
                {
                    TryRemoveFromInventory(wo.Guid, out WorldObject _);
                    ObjectGuid clearContainer = new ObjectGuid(0);
                    Session.Network.EnqueueSend(new GameMessageUpdateInstanceId(wo.Guid, clearContainer, PropertyInstanceId.Container));

                    // clean up the shard database.
                    throw new NotImplementedException();
                    // todo fix for EF
                    //DatabaseManager.Shard.DeleteObject(wo.SnapShotOfAceObject(), null);
                    Session.Network.EnqueueSend(new GameMessageRemoveObject(wo));
                }

                // if there is change - readd - do this at the end to try to prevent exploiting
                if (change > 0)
                {
                    HandleAddNewWorldObjectToInventory(changeobj);
                }

                UpdateCurrencyClientCalculations(WeenieType.Coin);
                return true;
            }

            return false;
        }
    }
}
