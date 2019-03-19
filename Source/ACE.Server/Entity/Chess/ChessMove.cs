using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    /// <summary>
    /// Holds all of the information that can change during a half turn
    /// </summary>
    public class ChessMove
    {
        public ChessMoveFlag Flags;
        public ChessColor Color;
        public ChessPieceType Type;
        public ChessPieceCoord From;
        public ChessPieceCoord To;
        public ChessPieceType Promotion;
        public ChessPieceType Captured;
        public uint Move;
        public uint HalfMove;
        public List<ChessMoveFlag> Castling;
        public ChessPieceCoord EnPassantCoord;
        public ObjectGuid Guid;
        public ObjectGuid CapturedGuid;

        public ChessMove(ChessMoveFlag flags, ChessColor color, ChessPieceType type, ChessPieceCoord from, ChessPieceCoord to, ChessPieceType promotion,
            ChessPieceType captured, uint move, uint halfMove, List<ChessMoveFlag> castling, ChessPieceCoord enPassantCoord, ObjectGuid guid, ObjectGuid capturedGuid)
        {
            Flags = flags;
            Color = color;
            Type = type;
            From = new ChessPieceCoord(from);
            To = new ChessPieceCoord(to);
            Promotion = promotion;
            Captured = captured;
            Move = move;
            HalfMove = halfMove;
            Castling = castling;
            EnPassantCoord = enPassantCoord;
            Guid = guid;
            CapturedGuid = capturedGuid;
        }
    }
}
