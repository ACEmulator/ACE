using System;
using ACE.Server.Entity.Chess;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public ChessMatch ChessMatch;

        /// <summary>
        /// Joins a chess game
        /// </summary>
        /// <param name="boardGuid">The guid of the chess board</param>
        public void HandleActionChessJoin(uint boardGuid)
        {
            var chessboard = CurrentLandblock.GetObject(boardGuid) as Game;
            if (chessboard == null)
                return;

            chessboard.ActOnUse(this);
        }

        /// <summary>
        /// Performs a move in a chess game
        /// </summary>
        /// <param name="from">The chessboard x/y coordinate moving piece from</param>
        /// <param name="to">The chessboard x/y coordinate moving piece to</param>
        public void HandleActionChessMove(ChessPieceCoord from, ChessPieceCoord to)
        {
            //Console.WriteLine($"{Name}.HandleActionChessMove({from}, {to})");

            if (ChessMatch != null)
                ChessMatch.MoveEnqueue(this, from, to);
        }

        public void HandleActionChessMovePass()
        {
            //Console.WriteLine($"{Name}.HandleActionChessMovePass()");

            if (ChessMatch != null)
                ChessMatch.MovePassEnqueue(this);
        }

        /// <summary>
        /// Called when the 'Resign' button is clicked
        /// </summary>
        public void HandleActionChessQuit()
        {
            //Console.WriteLine($"{Name}.HandleActionChessQuit()");

            if (ChessMatch != null)
                ChessMatch.QuitEnqueue(this);
        }

        /// <summary>
        /// Offer or confirm a stalemate
        /// </summary>
        /// <param name="stalemate">if FALSE, retract the stalemate offer</param>
        public void HandleActionChessStalemate(bool stalemate)
        {
            //Console.WriteLine($"{Name}.HandleActionChessStalemate({stalemate})");

            if (ChessMatch != null)
                ChessMatch.StalemateEnqueue(this, stalemate);
        }
    }
}
