namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionFinishBarber
    {
        [GameAction(GameActionType.FinishBarber)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionFinishBarber(message);
        }
    }
}
