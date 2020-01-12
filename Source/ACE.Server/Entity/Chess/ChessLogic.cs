using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Entity.Chess
{
    // ported from Anahera's code
    public class ChessLogic
    {
        public ChessColor Turn;

        public uint Move;
        public uint HalfMove;

        public List<ChessMoveFlag> Castling = new List<ChessMoveFlag>()
        {
            { ChessMoveFlag.KingSideCastle | ChessMoveFlag.QueenSideCastle },
            { ChessMoveFlag.KingSideCastle | ChessMoveFlag.QueenSideCastle },
        };

        public ChessPieceCoord EnPassantCoord;

        public BasePiece[] Board = new BasePiece[Chess.BoardSize * Chess.BoardSize];

        public Stack<ChessMove> History { get; set; }

        public ChessLogic()
        {
            Turn = ChessColor.White;

            // setup white
            AddPiece(ChessColor.White, ChessPieceType.Rook, 0, 0);
            AddPiece(ChessColor.White, ChessPieceType.Knight, 1, 0);
            AddPiece(ChessColor.White, ChessPieceType.Bishop, 2, 0);
            AddPiece(ChessColor.White, ChessPieceType.Queen, 3, 0);
            AddPiece(ChessColor.White, ChessPieceType.King, 4, 0);
            AddPiece(ChessColor.White, ChessPieceType.Bishop, 5, 0);
            AddPiece(ChessColor.White, ChessPieceType.Knight, 6, 0);
            AddPiece(ChessColor.White, ChessPieceType.Rook, 7, 0);

            for (uint i = 0; i < Chess.BoardSize; i++)
                AddPiece(ChessColor.White, ChessPieceType.Pawn, i, 1);

            // setup black
            AddPiece(ChessColor.Black, ChessPieceType.Rook, 0, 7);
            AddPiece(ChessColor.Black, ChessPieceType.Knight, 1, 7);
            AddPiece(ChessColor.Black, ChessPieceType.Bishop, 2, 7);
            AddPiece(ChessColor.Black, ChessPieceType.Queen, 3, 7);
            AddPiece(ChessColor.Black, ChessPieceType.King, 4, 7);
            AddPiece(ChessColor.Black, ChessPieceType.Bishop, 5, 7);
            AddPiece(ChessColor.Black, ChessPieceType.Knight, 6, 7);
            AddPiece(ChessColor.Black, ChessPieceType.Rook, 7, 7);

            for (uint i = 0; i < Chess.BoardSize; i++)
                AddPiece(ChessColor.Black, ChessPieceType.Pawn, i, 6);

            History = new Stack<ChessMove>();
        }

        public BasePiece AddPiece(ChessColor color, ChessPieceType type, uint x, uint y)
        {
            var to = new ChessPieceCoord((int)x, (int)y);
            Debug.Assert(to.IsValid());

            BasePiece piece = null;
            switch (type)
            {
                case ChessPieceType.Pawn:
                    piece = new PawnPiece(color, to);
                    break;

                case ChessPieceType.Rook:
                    piece = new RookPiece(color, to);
                    break;

                case ChessPieceType.Knight:
                    piece = new KnightPiece(color, to);
                    break;

                case ChessPieceType.Bishop:
                    piece = new BishopPiece(color, to);
                    break;

                case ChessPieceType.Queen:
                    piece = new QueenPiece(color, to);
                    break;

                case ChessPieceType.King:
                    piece = new KingPiece(color, to);
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            return Board[x + y * Chess.BoardSize] = piece;
        }

        public BasePiece AddPiece(ChessColor color, ChessPieceType type, ChessPieceCoord to)
        {
            return AddPiece(color, type, (uint)to.X, (uint)to.Y);
        }

        public BasePiece GetPiece(ChessPieceCoord from)
        {
            if (!from.IsValid())
                return null;

            return Board[from.X + from.Y * Chess.BoardSize];
        }

        public BasePiece GetPiece(ChessColor color, ChessPieceType type)
        {
            return Board.FirstOrDefault(i => i != null && i.Color == color && i.Type == type);
        }

        public BasePiece GetPiece(ObjectGuid guid)
        {
            return Board.FirstOrDefault(i => i != null && i.Guid == guid);
        }

        public void RemovePiece(ChessPieceCoord target)
        {
            var piece = GetPiece(target);
            if (piece != null)
                RemovePiece(piece);
        }

        public void RemovePiece(BasePiece piece)
        {
            Debug.Assert(piece != null);

            var coord = piece.Coord;
            Board[coord.Offset] = null;
        }

        public void MovePiece(ChessPieceCoord from, ChessPieceCoord to)
        {
            var fromPiece = GetPiece(from);
            fromPiece.Coord = to;

            RemovePiece(to);

            Board[to.Offset] = Board[from.Offset];
            Board[from.Offset] = null;
        }

        public void WalkPieces(Action<BasePiece> callback)
        {
            for (uint y = 0; y < Chess.BoardSize; y++)
            {
                for (uint x = 0; x < Chess.BoardSize; x++)
                {
                    var piece = Board[x + y * Chess.BoardSize];
                    if (piece != null)
                        callback(piece);
                }
            }
        }

        public ChessMoveResult DoMove(ChessColor color, ChessPieceCoord from, ChessPieceCoord to)
        {
            if (!from.IsValid())
                return ChessMoveResult.BadMoveDestination;
            if (!to.IsValid())
                return ChessMoveResult.BadMoveDestination;

            if (Turn != color)
                return ChessMoveResult.BadMoveNotYourTurn;

            var fromPiece = GetPiece(from);
            if (fromPiece == null)
                return ChessMoveResult.BadMoveNoPiece;

            if (fromPiece.Color != color)
                return ChessMoveResult.BadMoveNotYours;

            var storage = new List<ChessMove>();
            GenerateMoves(fromPiece, true, storage);

            ChessMove foundMove = null;
            foreach (var move in storage)
            {
                if (move.From.Equals(from) && move.To.Equals(to))
                {
                    foundMove = move;
                    break;
                }
            }

            // if this fails the client and server failed to find a common valid move
            if (foundMove == null)
                //return ChessMoveResult.BadMoveDestination;
                return ChessMoveResult.BadMoveInvalidCommand;

            return FinalizeMove(foundMove);
        }

        public ChessMoveResult AsyncCalculateAiSimpleMove(ChessAiAsyncTurnKey key, ref ChessPieceCoord from, ref ChessPieceCoord to)
        {
            var color = Turn;

            var bestBoardScore = 0.0f;
            ChessMove bestMove = null;

            var storage = new List<ChessMove>();
            GenerateMoves(color, storage);

            var nonCheckMoves = new List<ChessMove>();
            foreach (var generatedMove in storage)
            {
                // no need to evaluate the board if the ai has checkmated the other player
                var result = FinalizeMove(generatedMove);
                if (result.HasFlag(ChessMoveResult.OKMoveCheckmate))
                {
                    from = generatedMove.From;
                    to = generatedMove.To;
                    return result;
                }

                if (!InCheck(color))
                {
                    var boardScore = EvaluateBoard();
                    if (boardScore > bestBoardScore)
                    {
                        bestMove = generatedMove;
                        bestBoardScore = boardScore;
                    }
                    nonCheckMoves.Add(generatedMove);
                }

                UndoMove(1);
            }

            // every generated move had the same board score, pick one at random
            // this shouldn't happen, just here to prevent crash
            if (bestMove == null && nonCheckMoves.Count > 0)
            {
                var rng = ThreadSafeRandom.Next(0, nonCheckMoves.Count - 1);
                rng = 0;    // easier debugging
                bestMove = nonCheckMoves[rng];
            }

            // checkmate / stalemate
            if (bestMove == null)
                return ChessMoveResult.NoMoveResult;

            from = bestMove.From;
            to = bestMove.To;

            return FinalizeMove(bestMove);
        }

        public ChessMoveResult AsyncCalculateAiComplexMove(ChessAiAsyncTurnKey key, ref ChessPieceCoord from, ref ChessPieceCoord to, ref uint counter)
        {
            uint depth = 3;
            var isMaximizingPower = true;

            var storage = new List<ChessMove>();
            GenerateMoves(Turn, storage);

            var bestBoardScore = -9999.0f;
            ChessMove bestMove = null;

            foreach (var generatedMove in storage)
            {
                // no need to evaluate the board if the ai has checkmated the other player
                var result = FinalizeMove(generatedMove);
                if (result.HasFlag(ChessMoveResult.OKMoveCheckmate))
                {
                    from = generatedMove.From;
                    to = generatedMove.To;
                    return result;
                }

                var boardScore = MinimaxAlphaBeta(depth - 1, -10000, 10000, !isMaximizingPower, ref counter);
                UndoMove(1);

                if (boardScore >= bestBoardScore)
                {
                    bestMove = generatedMove;
                    bestBoardScore = boardScore;
                }
            }

            // ever generated move had the same board score, pick one at random
            // this shouldn't happen, just here to prevent crash
            if (bestMove == null && storage.Count > 0)
            {
                var rng = ThreadSafeRandom.Next(0, storage.Count - 1);
                bestMove = storage[rng];
            }

            Debug.Assert(bestMove != null);
            from = bestMove.From;
            to = bestMove.To;

            return FinalizeMove(bestMove);
        }

        public float MinimaxAlphaBeta(uint depth, float alpha, float beta, bool isMaximizingPower, ref uint counter)
        {
            counter++;
            if (depth <= 0)
                return -EvaluateBoard();

            var storage = new List<ChessMove>();
            GenerateMoves(Turn, storage);

            if (isMaximizingPower)
            {
                var bestBoardScore = -9999.0f;
                foreach (var move in storage)
                {
                    FinalizeMove(move);
                    bestBoardScore = Math.Max(bestBoardScore, MinimaxAlphaBeta(depth - 1, alpha, beta, false, ref counter));
                    UndoMove(1);

                    alpha = Math.Max(alpha, bestBoardScore);
                    if (beta <= alpha)
                        return bestBoardScore;
                }
                return bestBoardScore;
            }
            else
            {
                var bestBoardScore = 9999.0f;
                foreach (var move in storage)
                {
                    FinalizeMove(move);
                    bestBoardScore = Math.Max(bestBoardScore, MinimaxAlphaBeta(depth - 1, alpha, beta, true, ref counter));
                    UndoMove(1);

                    beta = Math.Max(beta, bestBoardScore);
                    if (beta <= alpha)
                        return bestBoardScore;
                }
                return bestBoardScore;
            }
        }

        public float EvaluateBoard()
        {
            var boardScore = 0.0f;
            WalkPieces((piece) =>
            {
                // the knight and queen only have a single shared table
                var tableColor = piece.Color;
                if (piece.Type == ChessPieceType.Knight || piece.Type == ChessPieceType.Queen)
                    tableColor = ChessColor.White;

                var value = 0.0f;
                value += Chess.PieceSquareTable[(int)piece.Type, (int)tableColor, piece.Coord.Offset];
                value += Chess.PieceWorth[(int)piece.Type];

                boardScore += piece.Color == ChessColor.Black ? value : -value;
            });

            return boardScore;
        }

        public void GenerateMoves(BasePiece piece, bool single, List<ChessMove> storage)
        {
            var color = piece.Color;
            if (piece.Type == ChessPieceType.Pawn)
            {
                // single
                var from = new ChessPieceCoord(piece.Coord);
                var to = new ChessPieceCoord(from);
                to.MoveOffset(Chess.PawnOffsets[(int)color, 0]);

                if (GetPiece(to) == null)
                {
                    BuildMove(storage, ChessMoveFlag.Normal, color, piece.Type, from, to);

                    // second
                    to = new ChessPieceCoord(from);
                    to.MoveOffset(Chess.PawnOffsets[(int)color, 1]);

                    if (GetPiece(to) == null && from.Rank == (color == ChessColor.White ? 2 : 7))
                        BuildMove(storage, ChessMoveFlag.BigPawn, color, piece.Type, from, to);
                }

                // capture
                for (uint i = 2; i < 4; i++)
                {
                    to = new ChessPieceCoord(from);
                    to.MoveOffset(Chess.PawnOffsets[(int)color, i]);
                    if (!to.IsValid())
                        continue;

                    var toPiece = GetPiece(to);
                    if (toPiece != null && toPiece.Color != color)
                        BuildMove(storage, ChessMoveFlag.Capture, color, piece.Type, from, to);
                    else if (to.Equals(EnPassantCoord))
                        BuildMove(storage, ChessMoveFlag.EnPassantCapture, color, piece.Type, from, EnPassantCoord);
                }
            }
            else
            {
                var range = Chess.PieceOffsets[piece.Type];
                for (var i = 0; i < range.Count; i++)
                {
                    var from = piece.Coord;
                    var to = new ChessPieceCoord(from);

                    while (true)
                    {
                        to.MoveOffset(range[i]);
                        if (!to.IsValid())
                            break;

                        var toPiece = GetPiece(to);
                        if (toPiece != null)
                        {
                            if (toPiece.Color != color)
                                BuildMove(storage, ChessMoveFlag.Capture, color, piece.Type, from, to);
                            break;
                        }

                        BuildMove(storage, ChessMoveFlag.Normal, color, piece.Type, from, to);

                        // Knights and Kings can't move more than once
                        if (piece.Type == ChessPieceType.Knight || piece.Type == ChessPieceType.King)
                            break;
                    }
                }
            }

            // only check for castling during full board generation or for a single king
            if (!single || piece.Type == ChessPieceType.King)
            {
                if ((Castling[(int)color] & ChessMoveFlag.KingSideCastle | ChessMoveFlag.QueenSideCastle) != 0)
                {
                    var king = GetPiece(color, ChessPieceType.King);
                    var kingCoord = king.Coord;

                    var opColor = Chess.InverseColor(color);

                    if ((Castling[(int)color] & ChessMoveFlag.KingSideCastle) != 0)
                    {
                        var castlingToK = new ChessPieceCoord(kingCoord);    // destination king
                        castlingToK.MoveOffset(2, 0);
                        var castlingToR = new ChessPieceCoord(kingCoord);    // destination rook
                        castlingToR.MoveOffset(1, 0);

                        if (GetPiece(castlingToR) == null
                            && GetPiece(castlingToK) == null
                            && !CanAttack(opColor, kingCoord)
                            && !CanAttack(opColor, castlingToR)
                            && !CanAttack(opColor, castlingToK))
                        {
                            BuildMove(storage, ChessMoveFlag.KingSideCastle, color, ChessPieceType.King, kingCoord, castlingToK);
                        }
                    }

                    if ((Castling[(int)color] & ChessMoveFlag.QueenSideCastle) != 0)
                    {
                        var castlingToK = new ChessPieceCoord(kingCoord);    // destination king
                        castlingToK.MoveOffset(-2, 0);
                        var castlingToR = new ChessPieceCoord(kingCoord);    // destination rook
                        castlingToR.MoveOffset(-1, 0);
                        var castlingToI = new ChessPieceCoord(kingCoord);    // intermediate
                        castlingToI.MoveOffset(-3, 0);

                        if (GetPiece(castlingToR) == null
                            && GetPiece(castlingToK) == null
                            && GetPiece(castlingToI) == null
                            && !CanAttack(opColor, kingCoord)
                            && !CanAttack(opColor, castlingToR)
                            && !CanAttack(opColor, castlingToK))
                        {
                            BuildMove(storage, ChessMoveFlag.QueenSideCastle, color, ChessPieceType.King, kingCoord, castlingToK);
                        }
                    }
                }
            }
        }

        public void GenerateMoves(ChessColor color, List<ChessMove> storage)
        {
            WalkPieces((piece) =>
            {
                if (piece.Color != color)
                    return;

                GenerateMoves(piece, false, storage);
            });
        }

        public bool CanAttack(ChessColor attacker, ChessPieceCoord victim)
        {
            for (uint x = 0; x < Chess.BoardSize; x++)
            {
                for (uint y = 0; y < Chess.BoardSize; y++)
                {
                    var piece = Board[x + y * Chess.BoardSize];
                    if (piece == null)
                        continue;

                    if (piece.Color != attacker)
                        continue;

                    if (piece.CanAttack(victim))
                    {
                        // the knight can jump over other pieces and the pawn can only attack a single space
                        if (piece.Type == ChessPieceType.Knight || piece.Type == ChessPieceType.Pawn)
                            return true;

                        var range = Chess.PieceOffsets[piece.Type];
                        for (var i = 0; i < range.Count; i++)
                        {
                            var from = piece.Coord;
                            var to = new ChessPieceCoord(from);

                            while (true)
                            {
                                to.MoveOffset(range[i]);
                                if (!to.IsValid())
                                    break;

                                var toPiece = GetPiece(to);
                                if (toPiece != null)
                                {
                                    if (to.Equals(victim))
                                        return true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool InCheck(ChessColor color)
        {
            var king = GetPiece(color, ChessPieceType.King);
            Debug.Assert(king != null);
            return CanAttack(Chess.InverseColor(color), king.Coord);
        }

        public bool InCheckmate(ChessColor color, bool fullCheck = false)
        {
            var storage = new List<ChessMove>();
            GenerateMoves(color, storage);

            var hasMove = false;
            if (fullCheck)
            {
                foreach (var generatedMove in storage)
                {
                    var result = FinalizeMove(generatedMove);

                    if (!InCheck(color))
                        hasMove = true;

                    UndoMove(1);

                    if (hasMove)
                        break;
                }
            }
            else
                hasMove = storage.Count > 0;

            return InCheck(color) && !hasMove;
        }

        public void BuildMove(List<ChessMove> storage, ChessMoveFlag result, ChessColor color, ChessPieceType type, ChessPieceCoord from, ChessPieceCoord to)
        {
            var fromPiece = GetPiece(from);
            var toPiece = GetPiece(to);

            // AC's Chess implementation doesn't support underpromotion
            var promotion = ChessPieceType.Empty;
            if (fromPiece.Type == ChessPieceType.Pawn && (to.Rank == 1 || to.Rank == 8))
            {
                promotion = ChessPieceType.Queen;
                result |= ChessMoveFlag.Promotion;
            }

            var captured = ChessPieceType.Empty;
            if (toPiece != null)
                captured = toPiece.Type;
            else if (result.HasFlag(ChessMoveFlag.EnPassantCapture))
            {
                captured = ChessPieceType.Pawn;

                var enPassantCoord = new ChessPieceCoord(to);
                enPassantCoord.MoveOffset(0, color == ChessColor.Black ? 1 : -1);

                toPiece = GetPiece(enPassantCoord);
            }

            storage.Add(new ChessMove(result, color, type, from, to, promotion, captured, Move, HalfMove, Castling, EnPassantCoord, fromPiece.Guid, captured != ChessPieceType.Empty ? toPiece.Guid : new ObjectGuid(0)));
        }

        public ChessMoveResult FinalizeMove(ChessMove move)
        {
            InternalMove(move);

            var result = (move.Flags & (ChessMoveFlag.Capture | ChessMoveFlag.EnPassantCapture)) != 0 ? ChessMoveResult.OKMoveToOccupiedSquare : ChessMoveResult.OKMoveToEmptySquare;
            if (move.Flags.HasFlag(ChessMoveFlag.Promotion))
                result |= ChessMoveResult.OKMovePromotion;

            // win conditions
            if (InCheck(Turn))
                result |= ChessMoveResult.OKMoveCheck;
            if (InCheckmate(Turn, false))
                result |= ChessMoveResult.OKMoveCheckmate;

            return result;
        }

        public void InternalMove(ChessMove move)
        {
            var flags = move.Flags;
            var to = move.To;
            var from = move.From;
            var color = move.Color;
            var opColor = Chess.InverseColor(color);

            MovePiece(from, to);

            if (flags.HasFlag(ChessMoveFlag.EnPassantCapture))
            {
                var enPassantCoord = new ChessPieceCoord(to);
                enPassantCoord.MoveOffset(0, color == ChessColor.Black ? 1 : -1);
                RemovePiece(enPassantCoord);
            }

            if (flags.HasFlag(ChessMoveFlag.Promotion))
            {
                var pawnPiece = GetPiece(to);
                var guid = pawnPiece.Guid;

                RemovePiece(pawnPiece);

                var queenPiece = AddPiece(color, ChessPieceType.Queen, (uint)to.X, (uint)to.Y);
                queenPiece.Guid = guid;
            }

            if (move.Type == ChessPieceType.King)
            {
                // if we castled, move the rook next to our king
                if ((flags & (ChessMoveFlag.KingSideCastle | ChessMoveFlag.QueenSideCastle)) != 0)
                {
                    var castlingTo = new ChessPieceCoord(move.To);
                    var castlingFrom = new ChessPieceCoord(castlingTo);

                    if (flags.HasFlag(ChessMoveFlag.KingSideCastle))
                    {
                        castlingTo.MoveOffset(-1, 0);
                        castlingFrom.MoveOffset(1, 0);
                    }
                    if (flags.HasFlag(ChessMoveFlag.QueenSideCastle))
                    {
                        castlingTo.MoveOffset(1, 0);
                        castlingFrom.MoveOffset(-2, 0);
                    }

                    MovePiece(castlingFrom, castlingTo);
                }

                // turn off castling, our king has moved
                Castling[(int)color] = ChessMoveFlag.None;
            }

            // turn off castling if we have moved one of our rooks
            if (Castling[(int)color] != ChessMoveFlag.None)
                DoCastleCheck(color, from);

            // turn off castling if we capture one of the opponent's rooks
            if (Castling[(int)opColor] != ChessMoveFlag.None)
                DoCastleCheck(opColor, from);

            if (flags.HasFlag(ChessMoveFlag.BigPawn))
            {
                var enPassantCoord = new ChessPieceCoord(to);
                //enPassantCoord.MoveOffset(0, color == ChessColor.Black ? 2 : -2);
                enPassantCoord.MoveOffset(0, color == ChessColor.Black ? 1 : -1);
                EnPassantCoord = enPassantCoord;
            }
            else
                EnPassantCoord = null;

            History.Push(move);

            if (color == ChessColor.Black)
                Move++;

            // reset 50 move rule counter if a pawn is moved or a piece is captured
            if (move.Type == ChessPieceType.Pawn
                || (flags & (ChessMoveFlag.Capture | ChessMoveFlag.EnPassantCapture)) != 0)
                HalfMove = 0;
            else
                HalfMove++;

            Turn = opColor;
        }

        public void UndoMove(uint count)
        {
            while (History.Count > 0 && count > 0)
            {
                var move = History.Peek();

                // undo 'global' information
                Turn = Chess.InverseColor(move.Color);
                Castling = move.Castling;
                EnPassantCoord = move.EnPassantCoord;
                HalfMove = move.HalfMove;
                Move = move.Move;

                MovePiece(move.To, move.From);

                var flags = move.Flags;
                if (flags.HasFlag(ChessMoveFlag.Promotion))
                {
                    var piece = AddPiece(Turn, ChessPieceType.Pawn, move.To);
                    piece.Guid = move.Guid;
                }

                if (flags.HasFlag(ChessMoveFlag.Capture))
                {
                    var piece = AddPiece(Turn, move.Captured, move.To);
                    piece.Guid = move.CapturedGuid;
                }

                if (flags.HasFlag(ChessMoveFlag.EnPassantCapture))
                {
                    var enPassantFrom = new ChessPieceCoord(move.To);
                    enPassantFrom.MoveOffset(0, move.Color == ChessColor.Black ? 1 : -1);

                    var piece = AddPiece(Turn, ChessPieceType.Pawn, enPassantFrom);
                    piece.Guid = move.CapturedGuid;
                }

                if ((flags & (ChessMoveFlag.KingSideCastle | ChessMoveFlag.QueenSideCastle)) != 0)
                {
                    var castlingTo = new ChessPieceCoord(move.To);
                    var castlingFrom = new ChessPieceCoord(castlingTo);

                    if (flags.HasFlag(ChessMoveFlag.KingSideCastle))
                    {
                        castlingTo.MoveOffset(1, 0);
                        castlingFrom.MoveOffset(-1, 0);
                    }
                    if (flags.HasFlag(ChessMoveFlag.QueenSideCastle))
                    {
                        castlingTo.MoveOffset(-2, 0);
                        castlingFrom.MoveOffset(1, 0);
                    }

                    MovePiece(castlingTo, castlingFrom);
                }

                History.Pop();
                count--;
            }
        }

        public void DoCastleCheck(ChessColor color, ChessPieceCoord from)
        {
            var rookFlags = Chess.RookFlags[color];
            foreach (var rookFlag in rookFlags)
            {
                if (from.X == rookFlag.Vector.X
                    && from.Y == rookFlag.Vector.Y
                    && (Castling[(int)color] & rookFlag.Flag) != 0)
                {
                    Castling[(int)color] &= ~rookFlag.Flag;
                    break;
                }
            }
        }

        public ChessMove GetLastMove()
        {
            return History.Peek();
        }

        public void DebugBoard()
        {
            for (var y = 7; y >= 0; y--)
            {
                for (var x = 0; x <= 7; x++)
                {
                    var piece = GetPiece(new ChessPieceCoord(x, y));
                    if (piece == null)
                        Console.Write(" ");
                    else if (piece is PawnPiece)
                        Console.Write("P");
                    else if (piece is KingPiece)
                        Console.Write("K");
                    else if (piece is QueenPiece)
                        Console.Write("Q");
                    else if (piece is BishopPiece)
                        Console.Write("B");
                    else if (piece is KnightPiece)
                        Console.Write("N");
                    else if (piece is RookPiece)
                        Console.Write("R");
                }
                Console.WriteLine();
            }
        }
    }
}
