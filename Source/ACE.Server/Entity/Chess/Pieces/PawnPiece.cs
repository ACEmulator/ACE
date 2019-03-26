using System;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class PawnPiece : BasePiece
    {
        public PawnPiece(ChessColor color, ChessPieceCoord to)
            : base(ChessPieceType.Pawn, color, to)
        {
        }

        public override bool CanMove(int dx, int dy)
        {
            var startRank = Color == ChessColor.White ? 2 : 7;
            var hasMoved = startRank != Coord.Y;
            var ady = Math.Abs(dy);
            return dx == 0 && (ady == 1 || ady == 2 && !hasMoved);
        }

        public override bool CanAttack(int dx, int dy)
        {
            var y = Color == ChessColor.White ? 1 : -1;
            return Math.Abs(dx) == 1 && dy == y;
        }
    }
}
