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
            var numItems = message.Payload.ReadUInt32();

            var items = new List<ItemProfile>();

            for (var i = 0; i < numItems; i++)
            {
                // verify amount > 0 here?

                var amount = message.Payload.ReadInt32();
                //amount &= 0xFFFFFF;

                var objectID = message.Payload.ReadUInt32();

                items.Add(new ItemProfile(amount, objectID));
            }

            // currency id is set to 0 by default
            // if non-zero, use as alternate currency wcid

            //var altCurrencyWcid = message.Payload.ReadUInt32();

            session.Player.HandleActionBuyItem(vendorGuid, items);
        }
    }
}
