
using System;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAcceptTrade
    {
        [GameAction(GameActionType.AcceptTrade)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint partnerGuid = message.Payload.ReadUInt32();
            double tradeStamp = message.Payload.ReadDouble();
            uint tradeStatus = message.Payload.ReadUInt32();
            uint initiatorGuid = message.Payload.ReadUInt32();
            bool initatorAccepts = Convert.ToBoolean(message.Payload.ReadUInt32());
            bool partnerAccepts = Convert.ToBoolean(message.Payload.ReadUInt32());

            session.Player.HandleActionAcceptTrade();
        }
    }
}
