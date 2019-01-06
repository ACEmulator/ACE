using ACE.Server.Managers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionOpenTradeNegotiations
    {
        [GameAction(GameActionType.OpenTradeNegotiations)]
        public static void Handle(ClientMessage message, Session session)
        {
            var tradePartnerGuid = message.Payload.ReadUInt32();

            var tradePartner = PlayerManager.GetOnlinePlayer(tradePartnerGuid);

            if (tradePartner != null)
            {
                //Open the trade window for the trade partner
                if (session.Player.HandleActionOpenTradeNegotiations(session, tradePartner, true))
                    //Trade partner met all criteria to initiate trade, open their window
                    tradePartner.HandleActionOpenTradeNegotiations(tradePartner.Session, session.Player);
            }
        }
    }
}
