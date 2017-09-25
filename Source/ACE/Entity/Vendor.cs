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
        // todo : so : Turning to player movement states  - @og
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
                    // add to action chain  ?
                    UseVendor(player);

                    ActionChain vendorChain = new ActionChain();

                    vendorChain.AddAction(this, () =>
                    {
                        ActionChain openVendor = new ActionChain();
                        openVendor.AddAction(this, () => Reset());
                        openVendor.EnqueueChain();

                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        player.Session.Network.EnqueueSend(sendUseDoneEvent);
                    });
                }
            });
            chain.EnqueueChain();
        }

        private void UseVendor(Player player)
        {
            // load inventory for vendor from db..
            // List<AceObject> saveditems =  ;

            // give player starting money
            player.GiveCoin(5000);
            player.SendUseDoneEvent();

            List<VendorItems> items = new List<VendorItems>();
            items = DatabaseManager.World.GetVendorWeenieInventoryById(WeenieClassId);

            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, Guid, items));         
        }

        private void Reset()
        {
        }
    }
}