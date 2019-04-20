using System;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class RookPiece: BasePiece
    {
        public RookPiece(ChessColor color, ChessPieceCoord to)
            : base(ChessPieceType.Rook, color, to)
        {
        }

        public override bool CanMove(int dx, int dy)
        {
            return (dx != 0) ^ (dy != 0);
        }
    }
}
