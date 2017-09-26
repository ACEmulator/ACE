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

namespace ACE.Entity
{
    public class Vendor : WorldObject
    {
        private List<AceObject> defaultItemsForSale = new List<AceObject>();
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
            // give player starting money
            player.GiveCoin(5000);

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
                    AceObject obj = new AceObject();
                    obj = DatabaseManager.World.GetAceObjectByWeenie(item.WeenieClassId);
                    if (obj != null)
                    {
                        defaultItemsForSale.Add(obj);           
                    }
                }
                inventoryloaded = true;
            }
        }

        private void Reset()
        {
        }
    }
}