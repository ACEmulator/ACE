namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionClearPlayerConsentList
    {
        [GameAction(GameActionType.ClearPlayerConsentList)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionClearPlayerConsentList();
        }
    }
}
