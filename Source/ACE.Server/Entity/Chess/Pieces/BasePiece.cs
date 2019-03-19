using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class BasePiece
    {
        public ObjectGuid Guid;

        public ChessPieceType Type;
        public ChessColor Color;
        public ChessPieceCoord Coord;

        public BasePiece(ChessPieceType type, ChessColor color, ChessPieceCoord to)
        {
            Type = type;
            Color = color;
            Coord = to;
        }

        public bool CanAttack(ChessPieceCoord target)
        {
            var dx = Coord.X - target.X;
            var dy = Coord.Y - target.Y;

            return CanAttack(dx, dy);
        }

        public virtual bool CanAttack(int dx, int dy)
        {
            return false;
        }

        public virtual bool CanMove(int dx, int dy)
        {
            return false;
        }
    }
}
