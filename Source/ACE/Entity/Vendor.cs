using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using ACE.Entity.Enum.Properties;
using System;
using ACE.DatLoader.FileTypes;
using ACE.Network.GameMessages.Messages;
using ACE.Common;
using System.Collections.Generic;
using ACE.Database;
using ACE.Factories;

namespace ACE.Entity
{
    public class Vendor : WorldObject
    {
        private Dictionary<ObjectGuid, WorldObject> defaultItemsForSale = new Dictionary<ObjectGuid, WorldObject>();
        private bool inventoryloaded = false;

        // todo : SO : Turning to player movement states  - looks at @og
        public Vendor(AceObject aceO)
            : base(aceO)
        {
        }

        public override void HandleActionOnUse(ObjectGuid playerId)
        {
            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
            {
                Player player = wo as Player;
                if (player == null)
                {
                    return;
                }

                if (!player.IsWithinUseRadiusOf(this))
                    player.DoMoveTo(this);
                else
                {
                    chain.AddAction(this, () =>
                    {
                        LoadInventory();
                        UseVendor(player);
                    });          
                }
            });

            chain.EnqueueChain();
        }

        /// <summary>
        /// Fired when user approaches vendor, sends all items vendor has for sale to players
        /// local tracked item list and opens vendor inventory screen. 
        /// </summary>
        /// <param name="player"></param>
        private void UseVendor(Player player)
        {
            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
            {
                player.TrackInteractiveObject(wo.Value);
            }

            // todo: send more then default items.
            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, this, defaultItemsForSale));
            player.SendUseDoneEvent();
        }

        /// <summary>
        /// Load Inventory for default items from database table / assignes default objects.
        /// </summary>
        private void LoadInventory()
        {
            // Load Vendor Inventory from database.
            if (!inventoryloaded)
            {
                List<VendorItems> items = new List<VendorItems>();
                items = DatabaseManager.World.GetVendorWeenieInventoryById(AceObject.WeenieClassId);
                foreach (VendorItems item in items)
                {
                    WorldObject wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);
                    if (wo != null)
                    {
                        wo.ContainerId = Guid.Full;
                        defaultItemsForSale.Add(wo.Guid, wo);
                    }
                }
                inventoryloaded = true;
            }
        }

        /// <summary>
        /// Buys Items from player
        /// </summary>
        /// <param name="vendorid">GUID of Vendor</param>
        /// <param name="items">Item Profile, Ammount and ID</param>
        /// <param name="player"></param>
        public void BuyItems(ObjectGuid vendorid, List<ItemProfile> items, Player player)
        {
            // do you have enough cash / iventory space for all this shit.
            int goldcost = 0;
            List<WorldObject> purchaselist = new List<WorldObject>();

            // que transactions.
            foreach (ItemProfile item in items)
            {
                ObjectGuid objid = new ObjectGuid(item.Iid);

                // check default items for id
                if (defaultItemsForSale.ContainsKey(objid))
                {
                    // todo: more stack logic ?
                    while (item.Amount > 0)
                    {
                        WorldObject wo = WorldObjectFactory.CreateNewWorldObject(defaultItemsForSale[objid].WeenieClassId);
                        // can we stack this ?
                        if (item.Amount <= wo.MaxStackSize)
                        {
                            item.Amount -= wo.MaxStackSize.Value;
                            goldcost += (int)defaultItemsForSale[objid].Value.Value * wo.MaxStackSize.Value;
                            wo.StackSize = wo.MaxStackSize.Value;
                            purchaselist.Add(wo);
                        }
                        // else we cant stack it or its less then max stack size.
                        else
                        {
                            item.Amount -= item.Amount;
                            goldcost += (int)defaultItemsForSale[objid].Value.Value * (int)item.Amount;
                            wo.StackSize = (ushort)item.Amount;
                            purchaselist.Add(wo);
                        }
                    }
                }

                // todo: vendor items sold by player
                // todo: now check to make sure you can aford this shit and you have pack space.
            }

            // send transaction to player for granting.
            player.HandleActionBuyTransaction(purchaselist, goldcost);
        
            // send updated vendor inventory
            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, this, defaultItemsForSale));
            player.SendUseDoneEvent();
        }

        public void SellItems(ObjectGuid vendorid, List<ItemProfile> items, Player player)
        {
            // todo: do have iventory space for all money from this shit ?
            int goldcost = 0;
            List<WorldObject> purchaselist = new List<WorldObject>();

            // que transactions.
            foreach (ItemProfile item in items)
            {
                ObjectGuid objid = new ObjectGuid(item.Iid);

                // check default items for id, is this unique ?
                if (defaultItemsForSale.ContainsKey(objid))
                {
                    // todo: more stack logic ?
                    while (item.Amount > 0)
                    {
                    }
                }
                // todo: vendor items sold by player
                // todo: now check to make sure you can aford this shit and you have pack space.
            }

            // send transaction to player for granting.
            player.HandleActionSellTransaction(purchaselist, goldcost);

            // send updated vendor inventory
            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, this, defaultItemsForSale));
            player.SendUseDoneEvent();
        }
    }
}