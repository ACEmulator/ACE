using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// Identifies the chess move attempt result.
    /// Negative/0 values are failures.
    /// </summary>
    //[Flags]
    public enum ChessMoveResult: int
    {
        NoMoveResult                    = 0x0000,
        OKMoveToEmptySquare             = 0x0001,
        OKMoveToOccupiedSquare          = 0x0002,
        //OKMoveEnPassant                 = OKMoveToEmptySquare | OKMoveToOccupiedSquare,
        //OKMoveMask                      = 0x03FF,
        OKMoveCheck                     = 0x0400,
        OKMoveCheckmate                 = 0x0800,
        OKMovePromotion                 = 0x1000,
        //OKMoveToEmptySquareCheck        = OKMoveCheck | OKMoveToEmptySquare,
        //OKMoveToOccupiedSquareCheck     = OKMoveCheck | OKMoveToOccupiedSquare,
        //OKMoveEnPassantCheck            = OKMoveCheck | OKMoveEnPassant,
        //OKMovePromotionCheck            = OKMovePromotion | OKMoveCheck,
        //OKMoveToEmptySquareCheckmate    = OKMoveCheckmate | OKMoveToEmptySquare,
        //OKMoveToOccupiedSquareCheckmate = OKMoveCheckmate | OKMoveToOccupiedSquare,
        //OKMoveEnPassantCheckmate        = OKMoveCheckmate | OKMoveEnPassant,
        //OKMovePromotionCheckmate        = OKMovePromotion | OKMoveCheckmate,

        BadMoveInvalidCommand           = -1,       // That move is invalid
        BadMoveNotPlaying               = -2,
        BadMoveNotYourTurn              = -3,       // Its not your turn, please wait for your opponents move.
        BadMoveDirection                = -100,     // The selected piece cannot move that direction
        BadMoveDistance                 = -101,     // The selected piece cannot move that far
        BadMoveNoPiece                  = -102,     // You tried to move an empty square
        BadMoveNotYours                 = -103,     // The selected piece is not yours
        BadMoveDestination              = -104,     // You cannot move off the board
        BadMoveWouldClobber             = -105,     // You cannot attack your own pieces
        BadMoveSelfCheck                = -106,     // That move would put you in check
        BadMoveWouldCollide             = -107,     // You can only move through empty squares
        BadMoveCantCastleOutOfCheck     = -108,     // You cannot castle out of check
        BadMoveCantCastleThroughCheck   = -109,     // You cannot castle through check
        BadMoveCantCastleAfterMoving    = -110,     // You cannot castle after moving the King or Rook
        BadMoveInvalidBoardState        = -111
    }
}
