using System;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class BishopPiece : BasePiece
    {
        public BishopPiece(ChessColor color, ChessPieceCoord to)
            : base(ChessPieceType.Bishop, color, to)
        {
        }

        public override bool CanMove(int dx, int dy)
        {
            return Math.Abs(dx) == Math.Abs(dy);
        }
    }
}
