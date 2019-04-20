using System.Collections.Generic;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionCreateTinkeringTool
    {
        [GameAction(GameActionType.CreateTinkeringTool)]
        public static void Handle(ClientMessage message, Session session)
        {
            var vendorGuid = message.Payload.ReadUInt32();
            uint itemcount = message.Payload.ReadUInt32();

            var items = new List<uint>();

            while (itemcount > 0)
            {
                itemcount--;
                items.Add(message.Payload.ReadUInt32());
            }

            session.Player.HandleSalvaging(items);
        }
    }
}
