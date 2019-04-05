using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    public class ChessAiMoveResult
    {
        public ChessMoveResult Result;
        public ChessPieceCoord From;
        public ChessPieceCoord To;

        public uint ProfilingTime;      // time taken in milliseconds to calculate ai move
        public uint ProfilingCounter;   // minimax recursion count

        public ChessAiMoveResult()
        {

        }

        public void SetResult(ChessMoveResult result, ChessPieceCoord from, ChessPieceCoord to)
        {
            Result = result;
            From = from;
            To = to;
        }
    }
}
