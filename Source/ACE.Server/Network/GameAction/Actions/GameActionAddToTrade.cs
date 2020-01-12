
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAddToTrade
    {
        [GameAction(GameActionType.AddToTrade)]
        public static void Handle(ClientMessage message, Session session)
        {
            var itemGuid = message.Payload.ReadUInt32();
            var tradeSlot = message.Payload.ReadUInt32();

            session.Player.HandleActionAddToTrade(itemGuid, tradeSlot);
        }
    }
}
