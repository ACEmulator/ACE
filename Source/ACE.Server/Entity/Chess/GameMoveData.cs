using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class GameMoveData
    {
        public ChessMoveType MoveType;
        public ChessColor Color;
        public ChessPieceCoord From;
        public ChessPieceCoord To;

        public GameMoveData(ChessMoveType moveType, ChessColor color, ChessPieceCoord from, ChessPieceCoord to)
        {
            MoveType = moveType;
            Color = color;
            From = from;
            To = to;
        }
    }
}
