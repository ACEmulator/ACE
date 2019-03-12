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
            var rank = Color == ChessColor.Black ? ChessPieceRank.Rank2 : ChessPieceRank.Rank7;
            var hasMoved = (int)rank != dy;
            var ady = Math.Abs(dy);
            return dx == 0 && (ady == 1 || ady == 2 && !hasMoved);
        }

        public override bool CanAttack(int dx, int dy)
        {
            return Math.Abs(dx) == 1 && dy == 1;
        }
    }
}
