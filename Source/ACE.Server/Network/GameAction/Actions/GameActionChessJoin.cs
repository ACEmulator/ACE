namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Joins a chess game
    /// </summary>
    public static class GameActionChessJoin
    {
        [GameAction(GameActionType.ChessJoin)]
        public static void Handle(ClientMessage message, Session session)
        {
            var boardGuid = message.Payload.ReadUInt32();   // chessboard guid
            var color = message.Payload.ReadInt32();        // expecting -1 here, unused?

            session.Player.HandleActionChessJoin(boardGuid);
        }
    }
}
