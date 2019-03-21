using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class ChessDelayedAction
    {
        public ChessDelayedActionType Action;
        public ChessColor Color;
        public ChessPieceCoord From;
        public ChessPieceCoord To;
        public bool Stalemate;

        public ChessDelayedAction(ChessDelayedActionType action)
        {
            Action = action;
        }

        public ChessDelayedAction(ChessDelayedActionType action, ChessColor color, bool stalemate = false)
        {
            Action = action;
            Color = color;
            Stalemate = stalemate;
        }

        public ChessDelayedAction(ChessDelayedActionType action, ChessColor color, ChessPieceCoord from, ChessPieceCoord to, bool stalemate = false)
        {
            Action = action;
            Color = color;
            From = from;
            To = to;
            Stalemate = stalemate;
        }
    }
}
