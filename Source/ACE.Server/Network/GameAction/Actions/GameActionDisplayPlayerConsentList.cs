
namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionDisplayPlayerConsentList
    {
        [GameAction(GameActionType.DisplayPlayerConsentList)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionDisplayPlayerConsentList();
        }
    }
}
