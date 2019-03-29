namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Quits a chess game
    /// </summary>
    public static class GameActionChessQuit
    {
        [GameAction(GameActionType.ChessQuit)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.HandleActionChessQuit();
        }
    }
}
