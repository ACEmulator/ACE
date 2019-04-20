using System;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class QueenPiece : BasePiece
    {
        public QueenPiece(ChessColor color, ChessPieceCoord to)
            : base(ChessPieceType.Queen, color, to)
        {
        }

        public override bool CanMove(int dx, int dy)
        {
            return dx == 0 || dy == 0 || Math.Abs(dx) == Math.Abs(dy);
        }
    }
}
