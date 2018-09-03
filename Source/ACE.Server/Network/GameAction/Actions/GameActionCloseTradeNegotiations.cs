using ACE.Server.Managers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionCloseTradeNegotiations
    {
        [GameAction(GameActionType.CloseTradeNegotiations)]
        public static void Handle(ClientMessage message, Session session)
        {
            var targetsession = WorldManager.Find(session.Player.TradePartner);

            if (targetsession != null)
            {
                session.Player.HandleActionCloseTradeNegotiations(session);

                //Close the trade window for the trade partner
                targetsession.Player.HandleActionCloseTradeNegotiations(targetsession);
            }
            
        }
    }
}
