using System.Collections.Generic;

using ACE.Server.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionBuyItems
    {
        [GameAction(GameActionType.Buy)]
        public static void Handle(ClientMessage message, Session session)
        {
            var vendorGuid = message.Payload.ReadUInt32();

            uint itemcount = message.Payload.ReadUInt32();

            List<ItemProfile> items = new List<ItemProfile>();

            while (itemcount > 0)
            {
                itemcount--;
                ItemProfile item = new ItemProfile();
                item.Amount = message.Payload.ReadUInt32();
                // item.Amount = item.Amount & 0xFFFFFF;

                item.ObjectGuid = message.Payload.ReadUInt32();
                items.Add(item);
            }
            
            // curancy 0 default, if else then currancy is set other then money
            uint i_alternateCurrencyID = message.Payload.ReadUInt32();

            // todo: take into account other currencyIds other then assuming default
            session.Player.HandleActionBuyItem(vendorGuid, items);
        }
    }
}
