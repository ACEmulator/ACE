using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Structure;
using ACE.Server.Physics.Animation;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Chess
{
    // ported from Anahera's code
    public class ChessMatch
    {
        public Game ChessBoard;
        public ChessState State;
        public ChessSide[] Sides;
        public ChessLogic Logic;
        public Queue<ChessDelayedAction> Actions;    // delayed action queue

        public ChessAiState AiState;
        public ChessAiMoveResult AiFuture;

        public ChessMoveResult MoveResult;
        public bool WaitingForMotion;
        public List<ObjectGuid> Motions;

        public DateTime? NextRangeCheck;
        public DateTime? StartAiTime;

        public List<ChessMove> Log;

        public ChessMatch(Game chessBoard)
        {
            ChessBoard = chessBoard;
            Sides = new ChessSide[Chess.NumColors];

            Logic = new ChessLogic();

            Actions = new Queue<ChessDelayedAction>();
            Motions = new List<ObjectGuid>();

            Log = new List<ChessMove>();
        }

        public ChessColor GetColor(ObjectGuid playerGuid)
        {
            foreach (var side in Sides)
            {
                if (side == null)
                    continue;

                if (side.PlayerGuid == playerGuid)
                    return side.Color;
            }
            return ChessColor.None;
        }

        public bool InMatch(ObjectGuid playerGuid)
        {
            return GetColor(playerGuid) != ChessColor.None;
        }

        public void Update()
        {
            switch (State)
            {
                case ChessState.WaitingForPlayers:
                    {
                        if (StartAiTime != null && DateTime.UtcNow >= StartAiTime)
                        {
                            AddAi();
                        }
                        break;
                    }

                case ChessState.InProgress:
                    {
                        switch (AiState)
                        {
                            case ChessAiState.WaitingToStart:
                                StartAiMove();
                                break;
                            case ChessAiState.WaitingForFinish:
                                FinishAiMove();
                                break;
                        }
                        break;
                    }
            }

            // don't handle any delayed actions while ai is working to prevent races
            if (AiState != ChessAiState.None)
                return;

            // don't handle any delayed actions while weenie pieces are moving or attacking
            if (WaitingForMotion)
                return;

            while (Actions.Count > 0)
            {
                var action = Actions.Dequeue();

                switch (action.Action)
                {
                    case ChessDelayedActionType.Start:
                        Start();
                        break;
                    case ChessDelayedActionType.Move:
                        MoveDelayed(action);
                        break;
                    case ChessDelayedActionType.MovePass:
                        MovePassDelayed(action);
                        break;
                    case ChessDelayedActionType.Stalemate:
                        StalemateDelayed(action);
                        break;
                    case ChessDelayedActionType.Quit:
                        QuitDelayed(action.Color);
                        break;
                }
            }

            if (NextRangeCheck != null)
            {
                if (NextRangeCheck.Value <= DateTime.UtcNow)
                {
                    foreach (var side in Sides)
                    {
                        if (side == null)
                            continue;

                        if (side.IsAi())
                            continue;

                        var player = side.GetPlayer();
                        if (player == null)
                        {
                            QuitDelayed(side.Color);
                            return;
                        }

                        // arbitrary distance, should there be some warning before reaching leash range?
                        var distanceToGame = player.Location.DistanceTo(ChessBoard.Location);
                        if (distanceToGame > 40.0f)
                        {
                            QuitDelayed(side.Color);
                            return;
                        }
                    }
                    NextRangeCheck = DateTime.UtcNow.AddSeconds(5);
                }
            }
        }

        public void AddSide(Player player, ChessColor color)
        {
            // ai is represented with object guid 0
            var playerGuid = player != null ? player.Guid : new ObjectGuid(0);

            Sides[(int)color] = new ChessSide(playerGuid, color);

            if (player != null)
                player.ChessMatch = this;

            // spawn weenie pieces in the world for side
            Logic.WalkPieces((piece) =>
            {
                if (piece.Color == color)
                    AddWeeniePiece(piece);
            });

            if (Sides[(int)Chess.InverseColor(color)] == null)
            {
                var ai_enabled = PropertyManager.GetDouble("chess_ai_start_time").Item;
                if (ai_enabled > 0)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"If another player doesn't join within {ai_enabled} seconds, the game will automatically start with AI", ChatMessageType.Broadcast));
                    StartAiTime = DateTime.UtcNow.AddSeconds(ai_enabled);
                }
            }
            else
                Actions.Enqueue(new ChessDelayedAction(ChessDelayedActionType.Start));
        }

        public void AddAi()
        {
            if (State != ChessState.WaitingForPlayers)
                return;

            var color = GetFreeColor();
            if (color == ChessColor.None)
                return;

            AddSide(null, color);
        }

        public void Join(Player player)
        {
            var color = GetFreeColor();
            if (color != ChessColor.None)
            {
                if (NextRangeCheck == null)
                    NextRangeCheck = DateTime.UtcNow.AddSeconds(5);

                AddSide(player, color);
            }

            player.Session.Network.EnqueueSend(new GameEventJoinGameResponse(player.Session, ChessBoard.Guid, color));
        }

        public void MoveEnqueue(Player player, ChessPieceCoord from, ChessPieceCoord to)
        {
            if (State != ChessState.InProgress)
                return;

            var color = GetColor(player.Guid);

            Actions.Enqueue(new ChessDelayedAction(ChessDelayedActionType.Move, color, from, to));
        }

        public void MovePassEnqueue(Player player)
        {
            if (State != ChessState.InProgress)
                return;

            var color = GetColor(player.Guid);

            Actions.Enqueue(new ChessDelayedAction(ChessDelayedActionType.MovePass, color));
        }

        public void QuitEnqueue(Player player)
        {
            if (State != ChessState.WaitingForPlayers && State != ChessState.InProgress)
                return;

            var color = GetColor(player.Guid);

            Actions.Enqueue(new ChessDelayedAction(ChessDelayedActionType.Quit, color));
        }

        public void StalemateEnqueue(Player player, bool stalemate)
        {
            if (State != ChessState.InProgress)
                return;

            var color = GetColor(player.Guid);

            Actions.Enqueue(new ChessDelayedAction(ChessDelayedActionType.Stalemate, color, stalemate));
        }

        public void AsyncMoveAiSimple(ChessAiAsyncTurnKey key, ChessAiMoveResult result)
        {
            Debug.Assert(AiState == ChessAiState.WaitingForWorker);
            AiState = ChessAiState.InProgress;

            ChessPieceCoord from = null, to = null;
            var moveResult = Logic.AsyncCalculateAiSimpleMove(key, ref from, ref to);
            result.SetResult(moveResult, from, to);

            AiState = ChessAiState.WaitingForFinish;
        }

        public void AsyncMoveAiComplex(ChessAiAsyncTurnKey key, ChessAiMoveResult result)
        {
            Debug.Assert(AiState == ChessAiState.WaitingForWorker);
            AiState = ChessAiState.InProgress;

            ChessPieceCoord from = null, to = null;
            uint counter = 0;
            var moveResult = Logic.AsyncCalculateAiComplexMove(key, ref from, ref to, ref counter);
            result.SetResult(moveResult, from, to);
            result.ProfilingCounter = counter;

            AiState = ChessAiState.WaitingForFinish;
        }

        public void PieceReady(ObjectGuid pieceGuid)
        {
            if (MoveResult.HasFlag(ChessMoveResult.OKMovePromotion))
            {
                var piece = Logic.GetPiece(pieceGuid);
                Debug.Assert(piece != null);
                UpgradeWeeniePiece(piece);
            }

            Motions.Remove(pieceGuid);
            if (Motions.Count == 0)
            {
                WaitingForMotion = false;
                FinishTurn();
            }
        }

        public ChessColor GetFreeColor()
        {
            for (var i = 0; i < Chess.NumColors; i++)
                if (Sides[i] == null)
                    return (ChessColor)i;

            return ChessColor.None;
        }

        public void Start()
        {
            Debug.Assert(State == ChessState.WaitingForPlayers);
            State = ChessState.InProgress;

            foreach (var side in Sides)
            {
                if (side.IsAi())
                    continue;

                var player = side.GetPlayer();
                Debug.Assert(player != null);
                SendStartGame(player, Logic.Turn);
            }
        }

        public void Finish(int winner)
        {
            if (State != ChessState.WaitingForPlayers && State != ChessState.InProgress)
                return;

            foreach (var side in Sides)
            {
                if (side == null)
                    continue;

                if (side.IsAi())
                    continue;

                var player = PlayerManager.FindByGuid(side.PlayerGuid, out bool isOnline);
                Player onlinePlayer = null;
                if (isOnline)
                {
                    onlinePlayer = PlayerManager.GetOnlinePlayer(side.PlayerGuid);
                    SendGameOver(onlinePlayer, winner);
                }

                if (winner != Chess.ChessWinnerEndGame)
                {
                    var totalGames = (player.GetProperty(PropertyInt.ChessTotalGames) ?? 0) + 1;
                    player.SetProperty(PropertyInt.ChessTotalGames, totalGames);

                    if (isOnline)
                        onlinePlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(onlinePlayer, PropertyInt.ChessTotalGames, totalGames));
                }

                if (winner >= 0)
                {
                    var winnerColor = (ChessColor)winner;
                    if (winnerColor == side.Color)
                    {
                        var won = (player.GetProperty(PropertyInt.ChessGamesWon) ?? 0) + 1;
                        player.SetProperty(PropertyInt.ChessGamesWon, won);

                        if (isOnline)
                            onlinePlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(onlinePlayer, PropertyInt.ChessGamesWon, won));
                    }
                    else
                    {
                        var lost = (player.GetProperty(PropertyInt.ChessGamesLost) ?? 0) + 1;
                        player.SetProperty(PropertyInt.ChessGamesLost, lost);

                        if (isOnline)
                            onlinePlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(onlinePlayer, PropertyInt.ChessGamesLost, lost));
                    }
                }

                if (isOnline)
                    onlinePlayer.ChessMatch = null;
            }

            if (winner >= 0)
            {
                // adjust player ranks
                var playerGuid = Sides[0].PlayerGuid;
                var opponentGuid = Sides[1].PlayerGuid;
                var winnerGuid = winner == 0 ? playerGuid : opponentGuid;

                AdjustPlayerRanks(playerGuid, opponentGuid, winnerGuid);
            }

            Actions.Clear();

            Logic.WalkPieces((piece) =>
            {
                RemoveWeeniePiece(piece);
            });

            State = ChessState.Finished;
            NextRangeCheck = null;

            ChessBoard.ChessMatch = null;
        }

        public void FinishTurn()
        {
            var side = Sides[(int)Chess.InverseColor(Logic.Turn)];
            var opponent = side.GetPlayer();
            var opponentGuid = side.PlayerGuid;
            if (side != null && !side.IsAi())
                SendMoveResponse(opponent, MoveResult);

            side = Sides[(int)Logic.Turn];
            if (side != null)
            {
                if (side.IsAi())
                    AiState = ChessAiState.WaitingToStart;
                else
                {
                    var move = Logic.GetLastMove();
                    var piece = Logic.GetPiece(move.To);
                    var data = new GameMoveData(ChessMoveType.FromTo, move.Color, move.From, move.To);
                    SendOpponentTurn(side.GetPlayer(), opponentGuid, piece, data);
                }
            }
        }

        public void StartAiMove()
        {
            Debug.Assert(AiState == ChessAiState.WaitingToStart);
            AiState = ChessAiState.WaitingForWorker;

            // todo: execute ai work on a separate thread
            var key = new ChessAiAsyncTurnKey();
            AiFuture = new ChessAiMoveResult();
            AsyncMoveAiSimple(key, AiFuture);
        }

        public void FinishAiMove()
        {
            Debug.Assert(AiState == ChessAiState.WaitingForFinish);
            AiState = ChessAiState.None;

            var result = AiFuture;
            MoveResult = result.Result;

            if (MoveResult == ChessMoveResult.NoMoveResult)
            {
                var color = Chess.InverseColor(Logic.Turn);
                var side = Sides[(int)color];
                var opSide = Sides[(int)Logic.Turn];

                // checkmate
                if (Logic.InCheckmate(color, true))
                {
                    Finish((int)Logic.Turn);
                }
                // stalemate
                else
                {
                    side.Stalemate = true;

                    if (opSide.Stalemate)
                        Finish(Chess.ChessWinnerStalemate);
                    else
                        SendOpponentStalemate(opSide.GetPlayer(), side.Color, true);
                }
            }
            else
                FinalizeWeenieMove(result.Result);

            //Console.WriteLine($"Calculated Chess AI move in {result.ProfilingTime} ms with {result.ProfilingCounter} minimax calculations.");
        }

        public void FinalizeWeenieMove(ChessMoveResult result)
        {
            var move = Logic.GetLastMove();
            Log.Add(move);

            // need to use destination coordinate as Logic has already moved the piece
            var piece = Logic.GetPiece(move.To);

            if (result.HasFlag(ChessMoveResult.OKMoveToEmptySquare))
            {
                MoveWeeniePiece(piece);

                var flags = move.Flags;
                if ((flags & (ChessMoveFlag.KingSideCastle | ChessMoveFlag.QueenSideCastle)) != 0)
                {
                    var castlingTo = new ChessPieceCoord(move.To);
                    if (flags.HasFlag(ChessMoveFlag.KingSideCastle))
                        castlingTo.MoveOffset(-1, 0);
                    if (flags.HasFlag(ChessMoveFlag.QueenSideCastle))
                        castlingTo.MoveOffset(1, 0);

                    var rookPiece = Logic.GetPiece(castlingTo);
                    Debug.Assert(rookPiece != null);

                    MoveWeeniePiece(rookPiece);
                }
            }
            else if (result.HasFlag(ChessMoveResult.OKMoveToOccupiedSquare))
                AttackWeeniePiece(piece, move.CapturedGuid);
        }

        public void MoveDelayed(ChessDelayedAction action)
        {
            if (Logic.Turn != action.Color)
                return;

            var player = Sides[(int)action.Color].GetPlayer();
            if (player == null)
            {
                QuitDelayed(action.Color);
                return;
            }

            var result = Logic.DoMove(action.Color, action.From, action.To);
            if (result < ChessMoveResult.NoMoveResult)
            {
                SendMoveResponse(player, result);
                return;
            }

            MoveResult = result;
            FinalizeWeenieMove(result);
        }

        public void MovePassDelayed(ChessDelayedAction action)
        {

        }

        public void QuitDelayed(ChessColor color)
        {
            switch (State)
            {
                case ChessState.WaitingForPlayers:
                    Finish(Chess.ChessWinnerEndGame);
                    break;
                case ChessState.InProgress:
                    Finish((int)Chess.InverseColor(color));
                    break;
            }
        }

        public void StalemateDelayed(ChessDelayedAction action)
        {
            var side = Sides[(int)action.Color];
            var opSide = Sides[(int)Chess.InverseColor(action.Color)];

            side.Stalemate = action.Stalemate;

            if (action.Stalemate && opSide.Stalemate)
                Finish(Chess.ChessWinnerStalemate);
            else if (!opSide.IsAi())
                SendOpponentStalemate(opSide.GetPlayer(), side.Color, action.Stalemate);
        }

        public void CalculateWeeniePosition(ChessPieceCoord coord, ChessColor color, AFrame frame)
        {
            var heading = (uint)ChessBoard.PhysicsObj.Position.Frame.get_heading();
            heading += color == ChessColor.Black ? 180u : 0u;
            heading %= 360;

            frame.Origin += new Vector3(coord.X - 3.5f, coord.Y - 3.5f, 0.0f);
            frame.set_heading(heading);
        }

        public void AddWeeniePiece(BasePiece piece)
        {
            var frame = new AFrame(ChessBoard.PhysicsObj.Position.Frame);
            CalculateWeeniePosition(piece.Coord, piece.Color, frame);

            var monster = piece.Color == ChessColor.White ? "drudge" : "mosswart";
            var weeniename = "";
            switch (piece.Type)
            {
                case ChessPieceType.Pawn:
                    weeniename = $"{monster}pawn";
                    break;
                case ChessPieceType.Rook:
                    weeniename = $"{monster}rook";
                    break;
                case ChessPieceType.Knight:
                    weeniename = $"{monster}knight";
                    break;
                case ChessPieceType.Bishop:
                    weeniename = $"{monster}bishop";
                    break;
                case ChessPieceType.Queen:
                    weeniename = $"{monster}queen";
                    break;
                case ChessPieceType.King:
                    weeniename = $"{monster}king";
                    break;
            }

            var wo = WorldObjectFactory.CreateNewWorldObject(weeniename);
            //Console.WriteLine($"AddWeeniePiece: {weeniename}, {piece.Coord}, {frame.Origin}");

            // add to position, spawn
            wo.Location = new Position(ChessBoard.Location);
            wo.Location.Pos = frame.Origin;
            wo.Location.Rotation = frame.Orientation;

            wo.EnterWorld();

            piece.Guid = wo.Guid;
            (wo as GamePiece).ChessMatch = this;
        }

        public void MoveWeeniePiece(BasePiece piece)
        {
            var gamePiece = ChessBoard.CurrentLandblock.GetObject(piece.Guid) as GamePiece;
            if (gamePiece == null)
            {
                Console.WriteLine($"ChessMatch.MoveWeeniePiece({piece.Guid}): couldn't find game piece");
                DebugMove();
                Logic.DebugBoard();
                return;
            }

            var frame = new AFrame(ChessBoard.PhysicsObj.Position.Frame);
            CalculateWeeniePosition(piece.Coord, piece.Color, frame);

            var position = new Position(ChessBoard.Location);
            position.Pos = frame.Origin;
            position.Rotation = frame.Orientation;

            gamePiece.MoveEnqueue(position);

            AddPendingWeenieMotion(piece);
        }

        public void AttackWeeniePiece(BasePiece piece, ObjectGuid victim)
        {
            var gamePiece = ChessBoard.CurrentLandblock.GetObject(piece.Guid) as GamePiece;
            if (gamePiece == null)
            {
                DebugMove();
                Logic.DebugBoard();
            }
            Debug.Assert(gamePiece != null);

            var frame = new AFrame(ChessBoard.PhysicsObj.Position.Frame);
            CalculateWeeniePosition(piece.Coord, piece.Color, frame);

            var position = new Position(ChessBoard.Location);
            position.Pos = frame.Origin;
            position.Rotation = frame.Orientation;

            gamePiece.AttackEnqueue(position, victim);

            AddPendingWeenieMotion(piece);
        }

        public void RemoveWeeniePiece(BasePiece piece)
        {
            var gamePiece = ChessBoard.CurrentLandblock.GetObject(piece.Guid) as GamePiece;
            if (gamePiece == null)
            {
                Console.WriteLine($"RemoveWeeniePiece - couldn't find {piece.Guid} @ {piece.Coord}");
                return;
            }
            piece.Guid = new ObjectGuid(0);
            gamePiece.Destroy();
        }

        public void UpgradeWeeniePiece(BasePiece piece)
        {
            RemoveWeeniePiece(piece);

            // AC's Chess implementation doesn't support underpromotion
            piece.Type = ChessPieceType.Queen;
            AddWeeniePiece(piece);
        }

        public void AddPendingWeenieMotion(BasePiece piece)
        {
            Motions.Add(piece.Guid);
            WaitingForMotion = true;
        }

        public void SendStartGame(Player player, ChessColor color)
        {
            player.Session.Network.EnqueueSend(new GameEventStartGame(player.Session, ChessBoard.Guid, color));
        }

        public void SendMoveResponse(Player player, ChessMoveResult result)
        {
            player.Session.Network.EnqueueSend(new GameEventMoveResponse(player.Session, ChessBoard.Guid, result));
        }

        public void SendOpponentTurn(Player player, ObjectGuid opponentGuid, BasePiece piece, GameMoveData data)
        {
            player.Session.Network.EnqueueSend(new GameEventOpponentTurn(player.Session, ChessBoard.Guid, new ChessMoveData(opponentGuid, piece.Guid, data)));
        }

        public void SendOpponentStalemate(Player player, ChessColor color, bool stalemate)
        {
            player.Session.Network.EnqueueSend(new GameEventOpponentStalemate(player.Session, ChessBoard.Guid, color, stalemate));
        }

        public void SendGameOver(Player player, int winner)
        {
            //Console.WriteLine($"Sending game over({winner}) to {player.Name}");

            player.Session.Network.EnqueueSend(new GameEventGameOver(player.Session, ChessBoard.Guid, winner));
        }

        /// <summary>
        /// How quickly the rankings will raise / lower
        /// </summary>
        public static readonly int RankFactor = 50;

        /// <summary>
        /// Adjusts the chess ranks for 2 players after a match
        /// </summary>
        public static void AdjustPlayerRanks(ObjectGuid playerGuid, ObjectGuid opponentGuid, ObjectGuid winnerGuid)
        {
            var player = PlayerManager.FindByGuid(playerGuid, out bool playerIsOnline);
            var opponent = PlayerManager.FindByGuid(opponentGuid, out bool opponentIsOnline);

            var rank = player.GetProperty(PropertyInt.ChessRank) ?? 1400;
            var opRank = 1400;
            if (opponent != null)   // chess ai
                opRank = opponent.GetProperty(PropertyInt.ChessRank) ?? 1400;

            var chance = ExpectationToWin(rank, opRank);

            var win = playerGuid == winnerGuid ? 1.0f : 0.0f;
            var delta = (int)Math.Round(RankFactor * (win - chance));

            player.SetProperty(PropertyInt.ChessRank, rank + delta);

            if (opponent != null)
                opponent.SetProperty(PropertyInt.ChessRank, opRank - delta);

            if (playerIsOnline)
            {
                var onlinePlayer = PlayerManager.GetOnlinePlayer(playerGuid);
                onlinePlayer.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(onlinePlayer, PropertyInt.ChessRank, rank + delta));
            }

            if (opponent != null && opponentIsOnline)
            {
                var onlineOp = PlayerManager.GetOnlinePlayer(opponentGuid);
                onlineOp.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(onlineOp, PropertyInt.ChessRank, opRank - delta));
            }
        }

        /// <summary>
        /// Returns the expected chance of rank beating opRank
        /// </summary>
        public static double ExpectationToWin(int rank, int opRank)
        {
            var rankDiff = opRank - rank;

            return 1.0f / (1.0f + Math.Pow(10, rankDiff / 400.0f));
        }

        public void DebugMove()
        {
            Console.WriteLine("  | White | Black");
            Console.WriteLine("-----------------");
            var num = 1;

            for (var i = 0; i < Log.Count; i++)
            {
                if (i % 2 == 0)
                {
                    var numStr = num.ToString().PadRight(2, ' ');
                    Console.Write($"{numStr}| ");
                    num++;
                }

                var move = Log[i];
                Console.Write($"{move.From}-{move.To}");

                if (i % 2 == 0)
                    Console.Write(" | ");
                else
                    Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
