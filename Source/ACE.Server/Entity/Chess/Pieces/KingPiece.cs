using System;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class KingPiece : BasePiece
    {
        public KingPiece(ChessColor color, ChessPieceCoord to)
            : base(ChessPieceType.King, color, to)
        {
        }

        public override bool CanMove(int dx, int dy)
        {
            return Math.Abs(dx) < 2 && Math.Abs(dy) < 2;
        }
    }
}
