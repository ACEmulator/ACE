using ACE.Common.Extensions;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionEnterPkLite
    {
        [GameAction(GameActionType.EnterPkLite)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionEnterPkLite();
        }
    }
}
