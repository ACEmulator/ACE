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
            List<WorldObject> vendorlist = new List<WorldObject>();
            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
            {
                vendorlist.Add(wo.Value);
            }

            // todo: send more then default items.
            player.TrackInteractiveObjects(vendorlist);
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
                // check default items for id
                if (defaultItemsForSale.ContainsKey(item.Guid))
                {
                    // todo: payments ?
                    while (item.Amount > 0)
                    {
                        WorldObject wo = WorldObjectFactory.CreateNewWorldObject(defaultItemsForSale[item.Guid].WeenieClassId);
                        // can we stack this ?
                        if (wo.MaxStackSize.HasValue)
                        {
                            if ((wo.MaxStackSize.Value != 0) & (wo.MaxStackSize.Value <= item.Amount))
                            {
                                item.Amount -= wo.MaxStackSize.Value;
                                goldcost += (int)defaultItemsForSale[item.Guid].Value.Value * wo.MaxStackSize.Value;
                                wo.StackSize = wo.MaxStackSize.Value;
                                purchaselist.Add(wo);
                            }
                            // else we cant stack it or its less then max stack size.
                            else
                            {
                                if (item.Amount > 0)
                                    goldcost += (int)defaultItemsForSale[item.Guid].Value.Value * item.Amount;
                                else
                                {
                                    item.Amount = 0;
                                    goldcost += (int)defaultItemsForSale[item.Guid].Value.Value;
                                }
                                wo.StackSize = (ushort)item.Amount;
                                purchaselist.Add(wo);
                            }
                        }
                        else
                        {
                            // single item with no stack options.
                            item.Amount = 0;
                            wo.StackSize = 0;
                            goldcost += (int)defaultItemsForSale[item.Guid].Value.Value;
                            purchaselist.Add(wo);
                        }
                    }
                }
            }

            // send transaction to player for granting.
            player.HandleActionBuyTransaction(purchaselist, goldcost);
        }

        public void StartSellItems(List<WorldObject> items, Player player)
        {
            // todo: add unique objects to local vendor list..
            // todo: calculate payment based on vendor multipliers
            uint coin = 0;
            foreach (WorldObject item in items)
            {
                coin += (uint)item.Value;
            }

            // Items are sold, pay player / delete items in the inventory.
            player.HandleActionFinishSellTransaction(items, Guid, coin);
        }
    }
}