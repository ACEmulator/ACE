using ACE.Server.Entity.Chess;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Makes a chess move
    /// </summary>
    public static class GameActionChessMove
    {
        [GameAction(GameActionType.ChessMove)]
        public static void Handle(ClientMessage message, Session session)
        {
            var from = message.Payload.ReadChessPieceCoord();
            var to = message.Payload.ReadChessPieceCoord();

            session.Player.HandleActionChessMove(from, to);
        }
    }
}
