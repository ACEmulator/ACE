using System;

namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// Offer or confirm stalemate
    /// </summary>
    public static class GameActionChessStalemate
    {
        [GameAction(GameActionType.ChessStalemate)]
        public static void Handle(ClientMessage message, Session session)
        {
            var stalemate = Convert.ToBoolean(message.Payload.ReadInt32());
            session.Player.HandleActionChessStalemate(stalemate);
        }
    }
}
