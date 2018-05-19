using System.Collections.Generic;

using ACE.Entity;
using ACE.Server.Entity;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionCreateTinkeringTool
    {
        [GameAction(GameActionType.CreateTinkeringTool)]
        public static void Handle(ClientMessage message, Session session)
        {
            ObjectGuid vendorId = new ObjectGuid(message.Payload.ReadUInt32());
            uint itemcount = message.Payload.ReadUInt32();

            List<ObjectGuid> items = new List<ObjectGuid>();

            while (itemcount > 0)
            {
                itemcount--;
                ObjectGuid item = new ObjectGuid();
                item = new ObjectGuid(message.Payload.ReadUInt32());
                items.Add(item);
            }

            session.Player.HandleTinkeringTool(items);
        }
    }
}
