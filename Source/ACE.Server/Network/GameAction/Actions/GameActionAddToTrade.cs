using ACE.Entity;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAddToTrade
    {
        [GameAction(GameActionType.AddToTrade)]
        public static void Handle(ClientMessage message, Session session)
        {
            ObjectGuid item = new ObjectGuid(message.Payload.ReadUInt32());
            uint tradeSlot = message.Payload.ReadUInt32();

            session.Player.HandleActionAddToTrade(session, item, tradeSlot);
        }
    }
}
