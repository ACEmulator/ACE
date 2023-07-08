using ACE.Database;
using ACE.Database.Models.Log;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace ACE.Server.Entity
{
    public class ArenaLocation
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public uint LandblockId { get; set; }

        public List<string> SupportedEventTypes { get; set; }

        public ArenaEvent ActiveEvent { get; set; }

        public bool HasActiveEvent { get { return ActiveEvent != null; } }

        private DateTime lastTickDateTime = DateTime.MinValue;
        private DateTime lastEventTimerMessage = DateTime.MinValue;

        public string ArenaName { get; set; }

        public ArenaLocation()
        {
            lastTickDateTime = DateTime.MinValue;
        }

        /// <summary>
        /// An arena location drives the lifecycle of its active arena event
        /// If there's no active arena event, calls ArenaManager to match make an event
        /// </summary>
        public void Tick()
        {
            //log.Info($"ArenaLocation.Tick() called for {this.ArenaName}");
            bool isArenasDisabled = PropertyManager.GetBool("disable_arenas").Item;
            if (isArenasDisabled)
            {
                if(this.HasActiveEvent)
                {
                    EndEventTimelimitExceeded();
                    ClearPlayersFromArena();
                    this.ActiveEvent = null;
                }

                return;
            }

            //If there's no active event, only run Tick every 5 seconds
            if (!HasActiveEvent && lastTickDateTime > DateTime.Now.AddSeconds(-5))
            {
                return;
            }

            if (HasActiveEvent)
            {
                log.Info($"ArenaLocation.Tick() - {this.ArenaName} is Active");

                //Drive the active arena event through its lifecycle
                switch (this.ActiveEvent.Status)
                {
                    case -1: //Event cancelled
                        log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = -1");
                        ClearPlayersFromArena();
                        this.ActiveEvent = null;
                        break;

                    case 1: //Not started                            

                        log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 1");

                        //Verify all players are still online, pk status and not pvp tagged
                        if (!ValidateArenaEventPlayers(out string resultMsg))
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 1 - Invalid Player State, canceling event. Reason = {resultMsg}");
                            this.ActiveEvent.CancelReason = resultMsg;
                            ArenaManager.CancelEvent(this.ActiveEvent);
                        }

                        //Broadcast to all players that a match was found and begin the countdown
                        log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 1 - Broadcasting that a match has been found");
                        foreach (var arenaPlayer in this.ActiveEvent.Players)
                        {
                            var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                            if (player != null)
                            {
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} - You have been matched for a {(this.ActiveEvent.EventType.Equals("ffa") ? "Free for All" : this.ActiveEvent.EventType)} arena event.  Prepare yourself, you will be teleported to the arena shortly.", ChatMessageType.System));
                            }
                        }

                        this.ActiveEvent.PreEventCountdownStartDateTime = DateTime.Now;

                        this.ActiveEvent.Status = 2;
                        break;

                    case 2://Pre-event countdown in progress

                        log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 2");

                        //Verify all players are still online, pk status and not pvp tagged
                        if (!ValidateArenaEventPlayers(out string resultMsg2))
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 1 - Invalid Player State, canceling event. Reason = {resultMsg2}");
                            this.ActiveEvent.CancelReason = resultMsg2;
                            ArenaManager.CancelEvent(this.ActiveEvent);
                        }

                        //Check if the pre-event countdown has completed
                        //if so, teleport all players to the arena and move to the next status
                        if (DateTime.Now.AddSeconds(-10) > this.ActiveEvent.PreEventCountdownStartDateTime)
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 2 - PreEventCountdown is complete");

                            //Pre-Event countdown is complete
                            List<Position> positions = ArenaLocation.GetArenaLocationStartingPositions(this.ActiveEvent.Location);
                            var playerList = new List<Player>();
                            foreach (var arenaPlayer in this.ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Players are now being teleported to the {(this.ActiveEvent.EventType.Equals("ffa") ? "Free for All" : this.ActiveEvent.EventType)} arena event.\nAfter a brief pause to allow everyone to arrive, the event will begin.", ChatMessageType.System));
                                    playerList.Add(player);
                                }
                                else
                                {
                                    //this shouldn't happen since we checked all players are online, but if it does, cancel
                                    log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 2 - PreEventCountdown is complete but player {arenaPlayer.CharacterName} is offline.  Canceling the event.");
                                    this.ActiveEvent.CancelReason = $"{arenaPlayer.CharacterName} logged off before being teleported to the arena";
                                    ArenaManager.CancelEvent(this.ActiveEvent);
                                    break;
                                }
                            }

                            //For team event types, add players to fellowships
                            if (this.ActiveEvent.EventType.Equals("2v2"))
                            {
                                CreateTeamFellowships();
                            }

                            //Teleport into the arena
                            for (int i = 0; i < playerList.Count; i++)
                            {                                
                                var j = i < positions.Count ? i : positions.Count - 1;
                                log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 2 - teleporting {playerList[i].Name} to position {positions[j].ToLOCString}");
                                playerList[i].Teleport(positions[j]);
                            }

                            this.ActiveEvent.CountdownStartDateTime = DateTime.Now;
                            this.ActiveEvent.Status = 3;
                        }
                        else
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 2 - PreEventCountdown has not yet completed");
                        }

                        break;

                    case 3://Event countdown in progress

                        log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 3");

                        //Countdown is complete, start the event
                        if (DateTime.Now.AddSeconds(-10) > this.ActiveEvent.CountdownStartDateTime)
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 3, countdown is complete, start the fight");
                            foreach (var arenaPlayer in this.ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The event has started!\nRemaining Event Time: 15m 0s", ChatMessageType.System));
                                    lastEventTimerMessage = DateTime.Now;
                                }
                            }

                            this.ActiveEvent.StartDateTime = DateTime.Now;
                            this.ActiveEvent.Status = 4;
                        }
                        else
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 3, countdown is not yet complete");
                        }

                        break;

                    case 4://Event started

                        log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 4");

                        //Check if there's a winner
                        if (this.ActiveEvent.WinningTeamGuid.HasValue)
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 4, WinningTeamGuid is already set, ending event with winner");
                            EndEventWithWinner(this.ActiveEvent.WinningTeamGuid.Value);
                            break;
                        }
                        else
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 4, WinningTeamGuid is not set, checking for winner");
                            if (CheckForArenaWinner(out Guid? winningTeamGuid) && winningTeamGuid.HasValue)
                            {
                                log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 4, winner found, ending event with winner");
                                EndEventWithWinner(winningTeamGuid.Value);
                                break;
                            }
                        }

                        //Check if the time limit has been exceeded
                        //TODO maybe set time limit per event type
                        if (!this.ActiveEvent.StartDateTime.HasValue || DateTime.Now > this.ActiveEvent.StartDateTime.Value.AddMinutes(15))
                        {
                            log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 4, event time limit exceeded, ending in draw");
                            EndEventTimelimitExceeded();
                            break;
                        }

                        //Message arena players with time updates every 30 seconds
                        if (DateTime.Now.AddSeconds(-30) > lastEventTimerMessage)
                        {
                            TimeSpan timeLeft = new TimeSpan();
                            if (this.ActiveEvent.StartDateTime.HasValue)
                                timeLeft = this.ActiveEvent.StartDateTime.Value.AddMinutes(15) - DateTime.Now;

                            foreach (var arenaPlayer in this.ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Remaining Event Time: {timeLeft.Minutes}m {timeLeft.Seconds}s", ChatMessageType.System));
                                }
                            }

                            lastEventTimerMessage = DateTime.Now;
                        }

                        break;

                    case 5://Event completed, in post-event countdown
                    case 6:
                        log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = {this.ActiveEvent.Status}");

                        //Check if the post-event countdown is completed
                        //If so, teleport any remaining players out of the arena and release the arena for the next event
                        if (DateTime.Now.AddSeconds(-20) > this.ActiveEvent.EndDateTime)
                        {
                            foreach (var arenaPlayer in this.ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null)
                                {
                                    if (player.CurrentLandblock.IsArenaLandblock)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Thank you for playing arenas.  You've loitered a bit too long after the event.  Have a nice trip to your Lifestone!", ChatMessageType.System));
                                        player.Teleport(player.Sanctuary);
                                    }
                                }
                            }

                            ClearPlayersFromArena();

                            this.ActiveEvent = null;
                        }                        
                        break;

                    default:
                        break;
                }
            }
            else
            {
                log.Info($"ArenaLocation.Tick() - {this.ArenaName} has no active event, finding match");
                ClearPlayersFromArena();
                MatchMake();
            }

            lastTickDateTime = DateTime.Now;
        }

        private void MatchMake()
        {
            log.Info($"ArenaLocation.MatchMake() - {this.ArenaName}");

            var arenaEvent = ArenaManager.MatchMake(this.SupportedEventTypes);
            if (arenaEvent != null)
            {
                this.ActiveEvent = arenaEvent;
                this.ActiveEvent.Location = this.LandblockId;
            }
        }

        private bool ValidateArenaEventPlayers(out string resultMsg)
        {
            var isPlayerNpk = false;
            var isPlayerPkTagged = false;
            var isPlayerMissing = false;
            resultMsg = "";
            foreach (var arenaPlayer in this.ActiveEvent.Players)
            {
                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                if (player != null)
                {
                    if (!player.IsPK)
                    {
                        isPlayerNpk = true;
                        resultMsg += $"\n{arenaPlayer.CharacterName} is not PK";
                    }
                    else if (player.PKTimerActive)
                    {
                        isPlayerPkTagged = true;
                        resultMsg += $"\n{arenaPlayer.CharacterName} is PvP tagged";
                    }
                }
                else
                {
                    isPlayerMissing = true;
                    resultMsg += $"\n{arenaPlayer.CharacterName} is not online";
                }
            }

            if(resultMsg.StartsWith("\n"))
                resultMsg = resultMsg.Remove(0, 1);

            return !isPlayerMissing && !isPlayerNpk && !isPlayerPkTagged;
        }

        public void CreateTeamFellowships()
        {
            if (this.ActiveEvent == null || this.ActiveEvent.Players == null || this.ActiveEvent.Players.Count < 4)
                return;

            //Get a distinct list of teams
            List<Guid> teamIds = new List<Guid>();
            foreach (var arenaPlayer in this.ActiveEvent.Players)
            {
                if (arenaPlayer.TeamGuid.HasValue && !teamIds.Contains(arenaPlayer.TeamGuid.Value))
                {
                    teamIds.Add(arenaPlayer.TeamGuid.Value);
                }
            }

            //For each team, pick a leader, create a fellow and recruit team members into the fellow
            foreach (var teamId in teamIds)
            {
                var teamArenaPlayers = this.ActiveEvent.Players.Where(x => x.TeamGuid == teamId);

                var teamLeadArenaPlayer = teamArenaPlayers?.FirstOrDefault();

                if (teamLeadArenaPlayer != null)
                {
                    var teamLeadPlayer = PlayerManager.GetOnlinePlayer(teamLeadArenaPlayer.CharacterId);
                    if (teamLeadPlayer != null)
                    {
                        teamLeadPlayer.FellowshipQuit(true);
                        teamLeadPlayer.FellowshipCreate(teamLeadPlayer.Name, false);

                        foreach (var teamArenaPlayer in teamArenaPlayers)
                        {
                            if (teamArenaPlayer.CharacterId == teamLeadArenaPlayer.CharacterId)
                                continue;

                            var teamPlayer = PlayerManager.GetOnlinePlayer(teamArenaPlayer.CharacterId);

                            if (teamPlayer != null)
                            {
                                teamPlayer.FellowshipQuit(true);
                                teamPlayer.SetCharacterOption(CharacterOption.AutomaticallyAcceptFellowshipRequests, true);
                                teamPlayer.SetCharacterOption(CharacterOption.IgnoreFellowshipRequests, false);
                                teamLeadPlayer.FellowshipRecruit(teamPlayer);
                            }
                        }
                    }
                }
            }
        }

        public bool CheckForArenaWinner(out Guid? winningTeamGuid)
        {
            winningTeamGuid = null;

            if (this.ActiveEvent == null || this.ActiveEvent.Players == null || this.ActiveEvent.Players.Count < 2 || this.ActiveEvent.Status < 4)
                return false;

            //Check to see if there's only one team still alive and in the arena
            //This approach is valid for XvX and FFA type events
            //In the future if we add things like kind of the hill, capture the flag, etc, there will be different logic to check for a winner

            //Get a distinct list of teams
            List<Guid> teamsStillAlive = new List<Guid>();
            foreach (var arenaPlayer in this.ActiveEvent.Players)
            {
                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                if (player != null && player.IsPK && (player.CurrentLandblock?.IsArenaLandblock ?? false))
                {
                    if (arenaPlayer.TeamGuid.HasValue && !teamsStillAlive.Contains(arenaPlayer.TeamGuid.Value))
                    {
                        teamsStillAlive.Add(arenaPlayer.TeamGuid.Value);
                    }
                }
            }

            if (teamsStillAlive.Count == 1)
            {
                winningTeamGuid = teamsStillAlive[0];
                return true;
            }

            if (teamsStillAlive.Count == 0)
            {
                winningTeamGuid = this.ActiveEvent.Players.First().TeamGuid;
                return true;
            }

            return false;
        }

        public void EndEventWithWinner(Guid winningTeamGuid)
        {
            log.Info($"ArenaLocation.EndEventWithWinner() - {this.ArenaName} - WinningTeamGuid = {winningTeamGuid}");

            //This method isn't really threadsafe, but we can guard against a race condition by just
            //making sure the location's tick event hasn't already ended the event before a character's death has ended it or vice versa
            if (this.ActiveEvent.Status > 4)
                return;

            this.ActiveEvent.Status = 5;

            this.ActiveEvent.EndDateTime = DateTime.Now;
            this.ActiveEvent.WinningTeamGuid = winningTeamGuid;

            DatabaseManager.Log.SaveArenaEvent(this.ActiveEvent);

            //Broadcast the win
            string winnerList = "";
            var winners = this.ActiveEvent.Players.Where(x => x.TeamGuid == winningTeamGuid)?.ToList();
            string loserList = "";
            var losers = this.ActiveEvent.Players.Where(x => x.TeamGuid != winningTeamGuid)?.ToList();

            winners.ForEach(x => winnerList += string.IsNullOrEmpty(winnerList) ? x.CharacterName : $", {x.CharacterName}");
            losers.ForEach(x => loserList += string.IsNullOrEmpty(loserList) ? x.CharacterName : $", {x.CharacterName}");

            foreach (var winner in winners)
            {
                var player = PlayerManager.GetOnlinePlayer(winner.CharacterId);
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Congratulations, you've won the {(this.ActiveEvent.EventType.Equals("ffa") ? "Free for All" : this.ActiveEvent.EventType)} arena event against {loserList}!\nSome blurb about your rewards.\nFeel free to recall or be teleported to your Lifestone in 30 seconds.", ChatMessageType.System));
                    //TODO reward the winners here
                }
            }

            //Broadcast the loss
            foreach (var loser in losers)
            {
                var player = PlayerManager.GetOnlinePlayer(loser.CharacterId);
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Tough luck, you've lost the {(this.ActiveEvent.EventType.Equals("ffa") ? "Free for All" : this.ActiveEvent.EventType)} arena event against {winnerList}\nSome blurb about your rewards.\nIf you're still in the arena, please recall or be teleported to your Lifestone in 30 seconds.", ChatMessageType.System));
                    //TODO reward the losers here
                }
            }

            //Global Broadcast
            PlayerManager.BroadcastToAll(new GameMessageSystemChat($"{winnerList} just won a {(this.ActiveEvent.EventType.Equals("ffa") ? "Free for All" : this.ActiveEvent.EventType)} arena event against {loserList}", ChatMessageType.Broadcast));
        }

        public void EndEventTimelimitExceeded()
        {
            log.Info($"ArenaLocation.EndEventTimelimitExceeded() - {this.ArenaName}");
            this.ActiveEvent.EndDateTime = DateTime.Now;
            this.ActiveEvent.Status = 6;

            DatabaseManager.Log.SaveArenaEvent(this.ActiveEvent);

            foreach (var arenaPlayer in this.ActiveEvent.Players)
            {
                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {(this.ActiveEvent.EventType.Equals("ffa") ? "Free for All" : this.ActiveEvent.EventType)} arena event has ended in a draw.  Please recall or be teleported to your Lifestone in 30 seconds.", ChatMessageType.System));
                    //TODO any rewards for a timeout would go here
                }
            }
        }

        public void EndEventCancel()
        {
            log.Info($"ArenaLocation.EndEventCancel() - {this.ArenaName}");
            this.ActiveEvent.EndDateTime = DateTime.Now;
            this.ActiveEvent.Status = -1;

            DatabaseManager.Log.SaveArenaEvent(this.ActiveEvent);

            foreach (var arenaPlayer in this.ActiveEvent.Players)
            {
                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {(this.ActiveEvent.EventType.Equals("ffa") ? "Free for All" : this.ActiveEvent.EventType)} arena event was cancelled before it started.  You will be placed back at the start of the queue.", ChatMessageType.System));                    
                }
            }
        }

        public void ClearPlayersFromArena()
        {
            log.Info($"ArenaLocation.ClearPlayersFromArena() - {this.ArenaName}");

            try
            {
                var arenaLandblock = LandblockManager.GetLandblock(new LandblockId(this.LandblockId << 16), false);
                var playerList = arenaLandblock.GetCurrentLandblockPlayers();
                foreach(var player in playerList)
                {
                    if (player.IsAdmin)
                        continue;

                    player.Teleport(player.Sanctuary);
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat("You've been teleported to your lifestone because you were inside an arena location without being an active participant in an arena event", ChatMessageType.System));
                }
            }
            catch(Exception ex)
            {
                log.Error($"Exception in ArenaLocation.ClearPlayersFromArena. ex: {ex}");
            }
        }

        public static Dictionary<uint, ArenaLocation> InitializeArenaLocations()
        {
            log.Info($"ArenaLocation.InitializeArenaLocations()");

            var locList = new Dictionary<uint, ArenaLocation>();

            //PKL Arena
            var pklArena = new ArenaLocation();
            pklArena.LandblockId = 0x0067;
            pklArena.SupportedEventTypes = new List<string>();
            pklArena.SupportedEventTypes.Add("1v1");            
            pklArena.SupportedEventTypes.Add("2v2");
            pklArena.SupportedEventTypes.Add("ffa");
            pklArena.ArenaName = "PKL Arena";
            locList.Add(pklArena.LandblockId, pklArena);

            //Binding Realm
            var bindRealm = new ArenaLocation();
            bindRealm.LandblockId = 0x007F;
            bindRealm.SupportedEventTypes = new List<string>();
            bindRealm.SupportedEventTypes.Add("1v1");
            bindRealm.SupportedEventTypes.Add("2v2");
            bindRealm.ArenaName = "Binding Realm";
            locList.Add(bindRealm.LandblockId, bindRealm);

            return locList;
        }

        private static List<uint> _arenaLandblocks;
        public static List<uint> ArenaLandblocks
        {
            get
            {
                if(_arenaLandblocks == null)
                {
                    _arenaLandblocks = new List<uint>()
                    {
                        0x0067, //PKL Arena
                        0x007F  //Binding Realm
                    };
                }

                return _arenaLandblocks;
            }
        }

        private static Dictionary<uint, List<Position>> _arenaLocationStartingPositions = null;
        private static Dictionary<uint, List<Position>> arenaLocationStartingPositions
        {
            get
            {
                if (_arenaLocationStartingPositions == null)
                {
                    _arenaLocationStartingPositions = new Dictionary<uint, List<Position>>();

                    _arenaLocationStartingPositions.Add(
                        0x0067,
                        new List<Position>()
                        {
                            new Position(0x00670103, 0f, -30f, 0.004999995f, 0f, 0f, -0.699713f, 0.714424f), //West
                            //0x00670103 [0 -30 0.004999995] 0.714424 0 0 -0.699713
                            new Position(0x00670127, 62.829544f, -28.933987f, 0.005000f, 0f, 0f, 0.654449f, 0.756106f), //East
                            //0x00670127 [62.829544 -28.933987 0.005000] 0.756106 0.000000 0.000000 0.654449
                            new Position(0x00670117, 29.432631f, -49.517181f, 0.005000f, 0f, 0f, -0.036533f, 0.999332f), //South
                            //0x00670117 [29.432631 -49.517181 0.005000] 0.999332 0.000000 0.000000 -0.036533
                            new Position(0x00670112, 29.911026f, -0.265875f, 0.005000f, 0f, 0f, 0.999908f, 0.013594f), //North
                            //0x00670112 [29.911026 -0.265875 0.005000] 0.013594 0.000000 0.000000 0.999908
                            new Position(0x00670105, 2.247503f, -48.398197f, 0.005000f, 0f, 0f, -0.440161f, 0.897919f), //SW
                            //0x00670105 [2.247503 -48.398197 0.005000] 0.897919 0.000000 0.000000 -0.440161
                            new Position(0x00670124, 58.684017f, -2.170248f, 0.005000f, 0f, 0f, 0.902263f, 0.431186f), //NE
                            //0x00670124 [58.684017 -2.170248 0.005000] 0.431186 0.000000 0.000000 0.902263
                            new Position(0x00670129, 58.118492f, -47.702240f, 0.005000f, 0f, 0f, 0.426291f, 0.904586f), //SE
                            //0x00670129 [58.118492 -47.702240 0.005000] 0.904586 0.000000 0.000000 0.426291
                            new Position(0x00670100, 1.902291f, -2.042494f, 0.005000f, 0f, 0f, 0.914624f, -0.404305f), //NW
                            //0x00670100 [1.902291 -2.042494 0.005000] -0.404305 0.000000 0.000000 0.914624
                            new Position(0x00670109, 14.541056f, -26.421442f, 0.005000f, 0f, 0f, 0.701743f, -0.712430f), //MidW
                            //0x00670109 [14.541056 -26.421442 0.005000] -0.712430 0.000000 0.000000 0.701743
                            new Position(0x0067011A, 37.242702f, -24.179514f, 0.005000f, 0f, 0f, 0.728694f, 0.684840f), //MidE
                            //0x0067011A [37.242702 -24.179514 0.005000] 0.684840 0.000000 0.000000 0.728694
                        }); //PKL Arena

                    _arenaLocationStartingPositions.Add(
                        0x007F,
                        new List<Position>()
                        {
                            new Position(0x007F0101, 236.75943f, -22.896727f, -59.995f, 0.000000f, 0.000000f, -0.6899546f, 0.72385263f), //West
                            //0x007F0101 [236.75943 -22.896727 -59.995] 0.72385263 0 0 -0.6899546
                            new Position(0x007F0107, 262.50168f, -16.419994f, -59.995f, 0.000000f, 0.000000f, -0.8109761f, -0.58507925f),//East
                            //0x007F0107 [262.50168 -16.419994 -59.995] -0.58507925 0 0 -0.8109761
                            new Position(0x007F0103, 250.935226f, -5.749045f, -59.994999f, 0.000000f, 0.000000f, -0.999384f, -0.035106f),//North
                            //0x007F0103 [250.935226 -5.749045 -59.994999] 0.035106 0.000000 0.000000 -0.999384
                            new Position(0x007F0105, 252.860062f, -33.350712f, -59.994999f, 0.000000f, 0.000000f, -0.043396f, -0.999058f),//South
                            //0x007F0105 [252.860062 -33.350712 -59.994999] -0.999058 0.000000 0.000000 -0.043396
                        }); //Binding Realm
                }

                return _arenaLocationStartingPositions;
            }
        }


        public static List<Position> GetArenaLocationStartingPositions(uint landblockId)
        {
            return arenaLocationStartingPositions.ContainsKey(landblockId) ? arenaLocationStartingPositions[landblockId] : new List<Position>();
        }

        public static bool IsArenaLandblock(uint landblockId)
        {
            return ArenaLandblocks.Contains(landblockId);
        }
    }
}
