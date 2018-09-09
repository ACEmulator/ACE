using ACE.Entity;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionResetTrade
    {
        [GameAction(GameActionType.ResetTrade)]
        public static void Handle(ClientMessage message, Session session)
        {
            ObjectGuid whoReset = session.Player.Guid;

            var targetsession = WorldManager.Find(session.Player.TradePartner);

            if (targetsession != null)
            {
                session.Player.HandleActionResetTrade(session, whoReset);

                //Send GameEvent to reset partner's trade window
                targetsession.Player.HandleActionResetTrade(targetsession, whoReset);
            }
        }
    }
}
