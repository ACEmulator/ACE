using System;
using System.IO;
using System.Numerics;

using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class ChessPieceCoord : IEquatable<ChessPieceCoord>
    {
        public int X;
        public int Y;

        public int Offset => X + Y * Chess.BoardSize;

        public int Rank => Y + 1;

        public ChessPieceCoord()
        {
            X = -1;
            Y = -1;
        }

        public ChessPieceCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public ChessPieceCoord(int offset)
        {
            X = offset % Chess.BoardSize;
            Y = offset / Chess.BoardSize;
        }

        public ChessPieceCoord(BinaryReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public ChessPieceCoord(ChessPieceCoord coord)
        {
            X = coord.X;
            Y = coord.Y;
        }

        public bool Equals(ChessPieceCoord coord)
        {
            return coord != null && X == coord.X && Y == coord.Y;
        }

        public override string ToString()
        {
            //return $"({X},{Y})";
            return $"{(char)('A' + X)}{Y + 1}";
        }

        public bool IsValid()
        {
            if (X < 0 || X >= Chess.BoardSize)
                return false;
            if (Y < 0 || Y >= Chess.BoardSize)
                return false;
            return true;
        }

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveOffset(int x, int y)
        {
            X += x;
            Y += y;
        }

        public void MoveOffset(Vector2 offset)
        {
            MoveOffset((int)offset.X, (int)offset.Y);
        }
    }

    public static class ChessPieceCoordExtensions
    {
        public static ChessPieceCoord ReadChessPieceCoord(this BinaryReader reader)
        {
            return new ChessPieceCoord(reader);
        }

        public static void Write(this BinaryWriter writer, ChessPieceCoord coord)
        {
            writer.Write(coord.X);
            writer.Write(coord.Y);
        }
    }
}
