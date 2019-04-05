using System;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class KnightPiece : BasePiece
    {
        public KnightPiece(ChessColor color, ChessPieceCoord to)
            : base(ChessPieceType.Knight, color, to)
        {
        }

        public override bool CanMove(int dx, int dy)
        {
            var adx = Math.Abs(dx);
            var ady = Math.Abs(dy);

            return adx == 1 && ady == 2 || adx == 2 && ady == 1;
        }
    }
}
