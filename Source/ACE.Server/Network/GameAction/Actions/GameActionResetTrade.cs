using ACE.Server.Managers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionResetTrade
    {
        [GameAction(GameActionType.ResetTrade)]
        public static void Handle(ClientMessage message, Session session)
        {
            var whoReset = session.Player.Guid;

            var target = PlayerManager.GetOnlinePlayer(session.Player.TradePartner);

            if (target != null)
            {
                session.Player.HandleActionResetTrade(whoReset);

                //Send GameEvent to reset partner's trade window
                target.HandleActionResetTrade(whoReset);
            }
        }
    }
}
