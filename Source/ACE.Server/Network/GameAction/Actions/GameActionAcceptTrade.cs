
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAcceptTrade
    {
        [GameAction(GameActionType.AcceptTrade)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint partnerGuid = message.Payload.ReadUInt32();
            ulong tradeStamp = message.Payload.ReadUInt64();
            uint tradeStatus = message.Payload.ReadUInt32();
            uint initiatorGuid = message.Payload.ReadUInt32();
            bool initatorAccepts = message.Payload.ReadBoolean();
            bool partnerAccepts = message.Payload.ReadBoolean();

            session.Player.HandleActionAcceptTrade(session, session.Player.Guid);
        }
    }
}
