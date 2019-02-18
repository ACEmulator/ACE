namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionClearMotd
    {
        [GameAction(GameActionType.ClearMotd)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionClearMotd();
        }
    }
}
