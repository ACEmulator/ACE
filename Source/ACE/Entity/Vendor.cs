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

        private void UseVendor(Player player)
        {
            foreach (KeyValuePair<ObjectGuid, WorldObject> wo in defaultItemsForSale)
            {
                player.TrackInteractiveObject(wo.Value);
            }

            player.AddCoin(50000);

            // todo: send more then default items.
            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, Guid, defaultItemsForSale));
            player.SendUseDoneEvent();
        }

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

        public void BuyItem(ObjectGuid vendorid, List<ItemProfile> items, Player player)
        {
            // do you have enough cash / iventory space for all this shit.
            uint goldcost = 0;
            List<WorldObject> purchaselist = new List<WorldObject>();

            // que transactions.
            foreach (ItemProfile item in items)
            {
                ObjectGuid objid = new ObjectGuid(item.Iid);

                // check default items for id
                if (defaultItemsForSale.ContainsKey(objid))
                {
                    // todo: stack logic ?
                    while (item.Amount > 0)
                    {
                        item.Amount--;
                        goldcost += defaultItemsForSale[objid].Value.Value;
                        WorldObject wo = WorldObjectFactory.CreateNewWorldObject(defaultItemsForSale[objid].WeenieClassId);
                        purchaselist.Add(wo);
                    }
                }

                // todo: vendor items sold by player
                // todo: now check to make sure you can aford this.       
            }

            // send transaction to player for granting.
            player.HandleActionBuyTransaction(purchaselist, goldcost);
        
            // send updated vendor inventory
            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, Guid, defaultItemsForSale));
            player.SendUseDoneEvent();
        }

        private void Reset()
        {
        }
    }
}