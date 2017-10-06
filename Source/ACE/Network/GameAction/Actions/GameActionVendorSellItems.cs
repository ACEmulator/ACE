using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionVendorSellItems
    {
        [GameAction(GameActionType.Sell)]
        public static void Handle(ClientMessage message, Session session)
        {
            ObjectGuid vendorId = new ObjectGuid(message.Payload.ReadUInt32());
            uint itemcount = message.Payload.ReadUInt32();

            List<ItemProfile> items = new List<ItemProfile>();

            while (itemcount > 0)
            {
                itemcount--;
                ItemProfile item = new ItemProfile();
                item.Amount = message.Payload.ReadUInt32();
                // item.Amount = item.Amount & 0xFFFFFF;

                item.Guid = new ObjectGuid(message.Payload.ReadUInt32());
                items.Add(item);
            }

            // curancy 0 default, if else then currancy is set other then money
            // uint i_alternateCurrencyID = message.Payload.ReadUInt32();

            // todo: take into account other currencyIds other then assuming default
            session.Player.HandleActionSell(items, vendorId);
        }
    }
}
