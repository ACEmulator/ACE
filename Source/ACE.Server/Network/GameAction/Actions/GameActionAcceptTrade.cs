using ACE.Entity;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionAcceptTrade
    {
        [GameAction(GameActionType.AcceptTrade)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionAcceptTrade(session, session.Player.Guid);
        }
    }
}
