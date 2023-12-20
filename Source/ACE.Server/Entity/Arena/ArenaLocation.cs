using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Log;
using ACE.Database.Models.TownControl;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Handlers;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Managers;
using log4net;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace ACE.Server.Entity
{
    public class ArenaLocation
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

            //If there's no active event, only run Tick every 5 seconds
            if (!HasActiveEvent && lastTickDateTime > DateTime.Now.AddSeconds(-5))
            {
                return;
            }

            if (HasActiveEvent)
            {
                //log.Info($"ArenaLocation.Tick() - {this.ArenaName} is Active");

                //If arenas are disabled
                if (PropertyManager.GetBool("disable_arenas").Item)
                {
                    ActiveEvent.Status = -1;
                    EndEventCancel();
                    ClearPlayersFromArena();
                    ActiveEvent = null;
                    return;
                }

                //If arenas observers are disabled
                if (!PropertyManager.GetBool("arena_allow_observers").Item)
                {
                    if ((ActiveEvent.Observers?.Count ?? 0) > 0)
                    {
                        List<uint> activeObservers = new List<uint>();
                        foreach (var observer in ActiveEvent.Observers)
                        {
                            var observerPlayer = PlayerManager.GetOnlinePlayer(observer);
                            if (observerPlayer != null)
                            {
                                activeObservers.Add(observerPlayer.Character.Id);
                            }
                        }

                        foreach (var observer in activeObservers)
                        {
                            ActiveEvent.Observers.Remove(observer);
                            var observerPlayer = PlayerManager.GetOnlinePlayer(observer);
                            if (observerPlayer != null)
                            {
                                ArenaManager.ExitArenaObserverMode(observerPlayer);
                            }
                        }
                    }
                }

                //Drive the active arena event through its lifecycle
                switch (ActiveEvent.Status)
                {
                    case -1: //Event cancelled
                        //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = -1");
                        ClearPlayersFromArena();
                        EndEventCancel();
                        ActiveEvent = null;
                        break;

                    case 1: //Not started                            

                        //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 1");

                        //Verify all players are still online, pk status and not pvp tagged
                        if (!ValidateArenaEventPlayers(out string resultMsg))
                        {
                            log.Info($"ArenaLocation.Tick() - {ArenaName} status = 1 - Invalid Player State, canceling event. Reason = {resultMsg}");
                            ActiveEvent.CancelReason = resultMsg;
                            ArenaManager.CancelEvent(ActiveEvent);
                        }

                        //Broadcast to all players that a match was found and begin the countdown
                        //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 1 - Broadcasting that a match has been found");
                        foreach (var arenaPlayer in ActiveEvent.Players)
                        {
                            var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                            if (player != null)
                            {
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} - You have been matched for a {ActiveEvent.EventTypeDisplay} arena event.  Prepare yourself, you will be teleported to the arena shortly.", ChatMessageType.System));
                            }
                        }

                        ActiveEvent.PreEventCountdownStartDateTime = DateTime.Now;

                        ActiveEvent.Status = ActiveEvent.Status == -1 ? -1 : 2;
                        break;

                    case 2://Pre-event countdown in progress

                        //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 2");

                        //Verify all players are still online, pk status and not pvp tagged
                        if (!ValidateArenaEventPlayers(out string resultMsg2))
                        {
                            log.Info($"ArenaLocation.Tick() - {ArenaName} status = 1 - Invalid Player State, canceling event. Reason = {resultMsg2}");
                            ActiveEvent.CancelReason = resultMsg2;
                            ArenaManager.CancelEvent(ActiveEvent);
                            return;
                        }

                        //Check if the pre-event countdown has completed
                        //if so, teleport all players to the arena and move to the next status
                        if (DateTime.Now.AddSeconds(-10) > ActiveEvent.PreEventCountdownStartDateTime)
                        {
                            //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 2 - PreEventCountdown is complete");

                            //Pre-Event countdown is complete
                            List<Position> positions = GetArenaLocationStartingPositions(ActiveEvent.Location);
                            var playerList = new List<Player>();
                            foreach (var arenaPlayer in ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Players are now being teleported to the {ActiveEvent.EventTypeDisplay} arena event.\nAfter a brief pause to allow everyone to arrive, the event will begin.", ChatMessageType.System));
                                    playerList.Add(player);
                                }
                                else
                                {
                                    //this shouldn't happen since we checked all players are online, but if it does, cancel
                                    log.Info($"ArenaLocation.Tick() - {ArenaName} status = 2 - PreEventCountdown is complete but player {arenaPlayer.CharacterName} is offline.  Canceling the event.");
                                    ActiveEvent.CancelReason = $"{arenaPlayer.CharacterName} logged off before being teleported to the arena";
                                    ArenaManager.CancelEvent(ActiveEvent);
                                    break;
                                }
                            }

                            //For team event types, add players to fellowships
                            if (ActiveEvent.EventType.Equals("2v2"))
                            {
                                CreateTeamFellowships();
                            }

                            //Make sure there's no way someone can remain as an observer while entering a match
                            foreach (var player in playerList)
                            {
                                if (player.IsArenaObserver || player.IsPendingArenaObserver || player.CloakStatus == CloakStatus.On)
                                {
                                    player.RecallsDisabled = false;
                                    player.IsFrozen = false;
                                    player.Attackable = true;
                                    if (player.GagDuration <= 0)
                                    {
                                        player.IsGagged = false;
                                    }
                                    player.DeCloak();
                                    player.IsPendingArenaObserver = false;
                                    player.IsArenaObserver = false;
                                }
                            }

                            //Teleport into the arena
                            for (int i = 0; i < playerList.Count; i++)
                            {
                                var j = i < positions.Count ? i : positions.Count - 1;
                                log.Info($"ArenaLocation.Tick() - {ArenaName} status = 2 - teleporting {playerList[i].Name} to position {positions[j].ToLOCString}");
                                playerList[i].Teleport(positions[j]);
                            }                            

                            ActiveEvent.CountdownStartDateTime = DateTime.Now;
                            ActiveEvent.Status = ActiveEvent.Status == -1 ? -1 : 3;
                        }
                        else
                        {
                            //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 2 - PreEventCountdown has not yet completed");
                        }

                        break;

                    case 3://Event countdown in progress

                        //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 3");

                        //Countdown is complete, start the event
                        if (DateTime.Now.AddSeconds(-15) > ActiveEvent.CountdownStartDateTime)
                        {
                            log.Info($"ArenaLocation.Tick() - {ArenaName} status = 3, countdown is complete, start the fight");
                            foreach (var arenaPlayer in ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The event has started!\nRemaining Event Time: 15m 0s", ChatMessageType.System));
                                    lastEventTimerMessage = DateTime.Now;
                                }
                            }

                            StartEvent();
                        }
                        else
                        {
                            //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 3, countdown is not yet complete");
                        }

                        break;

                    case 4://Event started

                        //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 4");

                        //Check if any players are no longer in the arena
                        var notEliminatedPlayers = ActiveEvent.Players.Where(x => !x.IsEliminated)?.ToList() ?? new List<ArenaPlayer>();
                        foreach (var arenaPlayer in ActiveEvent.Players)
                        {
                            if (!arenaPlayer.IsEliminated)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player == null)
                                {
                                    //if the player is not online they're eliminated
                                    arenaPlayer.IsEliminated = true;
                                    arenaPlayer.IsDisqualified = true;
                                    notEliminatedPlayers.Remove(arenaPlayer);
                                }
                                else
                                {
                                    //if the player is not PK they're eliminated
                                    if (!player.IsPK)
                                    {
                                        arenaPlayer.IsEliminated = true;
                                        ArenaManager.DispelArenaRares(player);
                                        notEliminatedPlayers.Remove(arenaPlayer);
                                    }

                                    //if the player is not on the arena landblock they're eliminated
                                    if (player.Location.Landblock != ActiveEvent.Location)
                                    {
                                        arenaPlayer.IsEliminated = true;
                                        ArenaManager.DispelArenaRares(player);
                                        notEliminatedPlayers.Remove(arenaPlayer);
                                        if (!player.IsInDeathProcess)
                                        {
                                            arenaPlayer.IsDisqualified = true;
                                            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been disqualified from the {arenaPlayer.EventType} arena match because you left the arena.", ChatMessageType.System));
                                        }
                                    }
                                }

                                //
                                if (arenaPlayer.IsEliminated && !arenaPlayer.IsDisqualified)
                                {
                                    arenaPlayer.FinishPlace = notEliminatedPlayers.Count() + 1;

                                    if (player != null)
                                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been eliminated from the {ActiveEvent.EventTypeDisplay} arena match in {ArenaName}.  You finished in {arenaPlayer.FinishPlaceDisplay} place.", ChatMessageType.System));
                                }
                            }
                        }

                        //Check if there's a winner
                        if (ActiveEvent.WinningTeamGuid.HasValue)
                        {
                            log.Info($"ArenaLocation.Tick() - {ArenaName} status = 4, WinningTeamGuid is already set, ending event with winner");
                            EndEventWithWinner(ActiveEvent.WinningTeamGuid.Value);
                            break;
                        }
                        else
                        {
                            //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = 4, WinningTeamGuid is not set, checking for winner");
                            if (CheckForArenaWinner(out Guid? winningTeamGuid) && winningTeamGuid.HasValue)
                            {
                                log.Info($"ArenaLocation.Tick() - {ArenaName} status = 4, winner found, ending event with winner");
                                EndEventWithWinner(winningTeamGuid.Value);
                                break;
                            }
                        }

                        //For overtime testing, push the start date back so overtime starts 30 seconds into the match
                        //if (this.ActiveEvent.StartDateTime < DateTime.Now.AddSeconds(-30) && this.ActiveEvent.StartDateTime > DateTime.Now.AddMinutes(-15))
                        //{
                        //    switch (this.ActiveEvent.EventType)
                        //    {
                        //        case "1v1":
                        //        case "2v2":
                        //            this.ActiveEvent.StartDateTime = this.ActiveEvent.StartDateTime.Value.AddMinutes(-15);
                        //            break;
                        //        case "ffa":
                        //            this.ActiveEvent.StartDateTime = this.ActiveEvent.StartDateTime.Value.AddMinutes(-25);
                        //            break;
                        //    }
                        //}                        

                        //Check if the time limit has been exceeded
                        if (!ActiveEvent.IsOvertime && ActiveEvent.TimeRemaining <= TimeSpan.Zero)
                        {
                            log.Info($"ArenaLocation.Tick() - {ArenaName} status = 4, event time limit exceeded, going to overtime");
                            ActiveEvent.IsOvertime = true;
                            foreach (var arenaPlayer in ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"OVERTIME! Chugs are disabled and all healing will be incrementally less effective as time goes on.  Overtime Remaining: {ActiveEvent.OvertimeRemainingDisplay}", ChatMessageType.System));
                                }
                            }

                            if (ActiveEvent.Observers != null)
                            {
                                foreach (var observer in ActiveEvent.Observers)
                                {
                                    var player = PlayerManager.GetOnlinePlayer(observer);
                                    if (player != null)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"OVERTIME! Chugs are disabled and all healing will be incrementally less effective as time goes on.  Overtime Remaining: {ActiveEvent.OvertimeRemainingDisplay}", ChatMessageType.System));
                                    }
                                }
                            }
                            break;
                        }

                        //Check if overtime has been exceeded
                        if (ActiveEvent.IsOvertime && ActiveEvent.OvertimeRemaining <= TimeSpan.Zero)
                        {
                            log.Info($"ArenaLocation.Tick() - {ArenaName} status = 4, event time limit exceeded, ending in draw");
                            EndEventTimelimitExceeded();
                            break;
                        }

                        //Message arena players with time updates every 30 seconds
                        if (DateTime.Now.AddSeconds(-30) > lastEventTimerMessage)
                        {
                            foreach (var arenaPlayer in ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null)
                                {
                                    if (ActiveEvent.IsOvertime)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Overtime Remaining: {ActiveEvent.OvertimeRemainingDisplay}\nHealing Reduction: {ActiveEvent.OvertimeHealingModifierDisplay}", ChatMessageType.System));
                                    }
                                    else
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Remaining Event Time: {ActiveEvent.TimeRemainingDisplay}", ChatMessageType.System));
                                    }
                                }
                            }

                            if (ActiveEvent.Observers != null)
                            {
                                foreach (var observer in ActiveEvent.Observers)
                                {
                                    var player = PlayerManager.GetOnlinePlayer(observer);
                                    if (player != null)
                                    {
                                        if (ActiveEvent.IsOvertime)
                                        {
                                            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Overtime Remaining: {ActiveEvent.OvertimeRemainingDisplay}\nHealing Reduction: {ActiveEvent.OvertimeHealingModifierDisplay}", ChatMessageType.System));
                                        }
                                        else
                                        {
                                            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Remaining Event Time: {ActiveEvent.TimeRemainingDisplay}", ChatMessageType.System));
                                        }
                                    }
                                }
                            }

                            lastEventTimerMessage = DateTime.Now;
                        }

                        break;

                    case 5://Event completed, in post-event countdown
                    case 6:
                        //log.Info($"ArenaLocation.Tick() - {this.ArenaName} status = {this.ActiveEvent.Status}");

                        //Check if the post-event countdown is completed
                        //If so, teleport any remaining players out of the arena and release the arena for the next event
                        if (DateTime.Now.AddSeconds(-45) > ActiveEvent.EndDateTime)
                        {
                            foreach (var arenaPlayer in ActiveEvent.Players)
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

                            ActiveEvent = null;
                        }
                        else //if there's no players in the arena go ahead and end the event early
                        {
                            bool hasPlayers = false;
                            foreach (var arenaPlayer in ActiveEvent.Players)
                            {
                                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                                if (player != null && player.Location.Landblock == LandblockId)
                                {
                                    hasPlayers = true;
                                }
                            }

                            if (!hasPlayers)
                            {
                                ActiveEvent = null;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                //log.Info($"ArenaLocation.Tick() - {this.ArenaName} has no active event, finding match");
                ClearPlayersFromArena();
                MatchMake();
            }

            lastTickDateTime = DateTime.Now;
        }

        private void MatchMake()
        {
            //log.Info($"ArenaLocation.MatchMake() - {this.ArenaName}");

            var arenaEvent = ArenaManager.MatchMake(SupportedEventTypes);
            if (arenaEvent != null)
            {
                ActiveEvent = arenaEvent;
                ActiveEvent.Location = LandblockId;
            }
        }

        private bool ValidateArenaEventPlayers(out string resultMsg)
        {
            var isPlayerNpk = false;
            var isPlayerPkTagged = false;
            var isPlayerMissing = false;
            resultMsg = "";
            foreach (var arenaPlayer in ActiveEvent.Players)
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

            if (resultMsg.StartsWith("\n"))
                resultMsg = resultMsg.Remove(0, 1);

            return !isPlayerMissing && !isPlayerNpk && !isPlayerPkTagged;
        }

        public void CreateTeamFellowships()
        {
            if (ActiveEvent == null || ActiveEvent.Players == null || ActiveEvent.Players.Count < 4)
                return;

            //Get a distinct list of teams
            List<Guid> teamIds = new List<Guid>();
            foreach (var arenaPlayer in ActiveEvent.Players)
            {
                if (arenaPlayer.TeamGuid.HasValue && !teamIds.Contains(arenaPlayer.TeamGuid.Value))
                {
                    teamIds.Add(arenaPlayer.TeamGuid.Value);
                }
            }

            //For each team, pick a leader, create a fellow and recruit team members into the fellow
            foreach (var teamId in teamIds)
            {
                var teamArenaPlayers = ActiveEvent.Players.Where(x => x.TeamGuid == teamId);

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

            if (ActiveEvent == null || ActiveEvent.Players == null || ActiveEvent.Players.Count < 2 || ActiveEvent.Status < 4)
                return false;

            //Check to see if there's only one team still alive and in the arena
            //This approach is valid for XvX and FFA type events
            //In the future if we add things like kind of the hill, capture the flag, etc, there will be different logic to check for a winner

            //Get a distinct list of teams
            List<Guid> teamsStillAlive = new List<Guid>();
            foreach (var arenaPlayer in ActiveEvent.Players)
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
                if (ActiveEvent.EventType.Equals("ffa"))
                {
                    var winner = ActiveEvent.Players.FirstOrDefault(x => x.TeamGuid == teamsStillAlive[0]);
                    if (winner != null)
                    {
                        winner.FinishPlace = 1;
                    }
                }

                winningTeamGuid = teamsStillAlive[0];
                return true;
            }

            if (teamsStillAlive.Count == 0)
            {
                winningTeamGuid = ActiveEvent.Players.First().TeamGuid;
                return true;
            }

            return false;
        }

        public void StartEvent()
        {
            ActiveEvent.StartDateTime = DateTime.Now;
            ActiveEvent.Status = ActiveEvent.Status == -1 ? -1 : 4;

            DatabaseManager.Log.SaveArenaEvent(ActiveEvent);

            var msg = $"Arena Match Started: Event Type = {ActiveEvent.EventTypeDisplay}, Players = {ActiveEvent.PlayersDisplay}, EventID = {ActiveEvent.Id}. To watch the event, type /arena watch {ActiveEvent.Id}";
            PlayerManager.BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
            try
            {
                var webhookUrl = PropertyManager.GetString("arena_globals_webhook").Item;
                if (!string.IsNullOrEmpty(webhookUrl))
                {
                    _ = TurbineChatHandler.SendWebhookedChat("Arenas", msg, webhookUrl, "Global");
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed sending Arena global message to webhook. Ex:{0}", ex);
            }

        }

        public void EndEventWithWinner(Guid winningTeamGuid)
        {
            log.Info($"ArenaLocation.EndEventWithWinner() - {ArenaName} - WinningTeamGuid = {winningTeamGuid}");

            //This method isn't really threadsafe, but we can guard against a race condition by just
            //making sure the location's tick event hasn't already ended the event before a character's death has ended it or vice versa
            if (ActiveEvent.Status > 4)
                return;

            ActiveEvent.Status = 5;

            ActiveEvent.EndDateTime = DateTime.Now;
            ActiveEvent.WinningTeamGuid = winningTeamGuid;

            var livingWinners = ActiveEvent.Players.Where(x => x.TeamGuid == winningTeamGuid && !x.IsEliminated && !x.IsDisqualified);
            if (livingWinners != null)
            {
                foreach (var winner in livingWinners)
                {
                    winner.FinishPlace = 1;
                }
            }

            DatabaseManager.Log.SaveArenaEvent(ActiveEvent);

            string winnerList = "";
            var winners = ActiveEvent.Players.Where(x => x.TeamGuid == winningTeamGuid)?.ToList();
            string loserList = "";
            var losers = ActiveEvent.Players.Where(x => x.TeamGuid != winningTeamGuid)?.ToList();

            winners.ForEach(x => winnerList += string.IsNullOrEmpty(winnerList) ? x.CharacterName : $", {x.CharacterName}");
            losers.ForEach(x => loserList += string.IsNullOrEmpty(loserList) ? x.CharacterName : $", {x.CharacterName}");

            bool underageViolation = false;
            var underageCount = 0;
            foreach (var arenaPlayer in ActiveEvent.Players)
            {
                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                if (player != null && player.Age <= PropertyManager.GetLong("arenas_reward_min_age").Item) //days in seconds
                {
                    underageCount++;
                }
            }

            if (underageCount > 0 && (ActiveEvent.EventType.Equals("1v1") || ActiveEvent.EventType.Equals("2v2")))
            {
                underageViolation = true;
            }
            else if (underageCount > 2 && ActiveEvent.EventType.Equals("ffa"))
            {
                underageViolation = true;
            }

            Dictionary<uint,uint> newRankings = new Dictionary<uint,uint>();
            if(ActiveEvent.EventType.Equals("1v1"))
            {
                var winner = winners.FirstOrDefault();
                var loser = losers.FirstOrDefault();
                if (winner != null && loser != null)
                {
                    var winnerCurrentRank = DatabaseManager.Log.GetCharacterArenaStatsByEvent(winner.CharacterId, "1v1")?.RankPoints ?? 1500;
                    var loserCurrentRank = DatabaseManager.Log.GetCharacterArenaStatsByEvent(loser.CharacterId, "1v1")?.RankPoints ?? 1500;

                    var rankChange = ArenaRanking.GetRankChange(winnerCurrentRank, loserCurrentRank, 32);

                    var winnerNewRank = (int)winnerCurrentRank + rankChange > 0 ? (uint)(winnerCurrentRank + rankChange) : default(uint);
                    var loserNewRank = (int)loserCurrentRank - rankChange > 0 ? (uint)(loserCurrentRank - rankChange) : default(uint);

                    if (newRankings.ContainsKey(winner.CharacterId))
                    {
                        newRankings[winner.CharacterId] = winnerNewRank;
                    }
                    else
                    {
                        newRankings.Add(winner.CharacterId, winnerNewRank);
                    }

                    if (newRankings.ContainsKey(loser.CharacterId))
                    {
                        newRankings[loser.CharacterId] = loserNewRank;
                    }
                    else
                    {
                        newRankings.Add(loser.CharacterId, loserNewRank);
                    }
                }    
            }

            foreach (var winner in winners)
            {
                uint? newRank = null;
                if(newRankings.ContainsKey(winner.CharacterId))
                {
                    newRank = newRankings[winner.CharacterId];
                }

                //Add to stats
                DatabaseManager.Log.AddToArenaStats(
                    winner.CharacterId,
                    winner.CharacterName,
                    winner.EventType,
                    1,
                    1,
                    0,
                    0,
                    0,
                    winner.TotalDeaths,
                    winner.TotalKills,
                    winner.TotalDmgDealt,
                    winner.TotalDmgReceived,
                    newRank);

                var player = PlayerManager.GetOnlinePlayer(winner.CharacterId);
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Congratulations, you've won the {ActiveEvent.EventTypeDisplay} arena event against {loserList}!\nIf you're still in {ArenaName} you have a short period before you're teleported to your Lifestone so hurry up and loot.", ChatMessageType.System));

                    //Reward the winners
                    var shouldReward = IsPlayerRewardEligible(player, winner, ActiveEvent.Players) && !underageViolation;

                    if (shouldReward)
                    {
                        switch (ActiveEvent.EventType)
                        {
                            case "1v1":
                            case "2v2":

                                //Give % xp to next level
                                if (player.Level > 0 && player.Level < 50)
                                {
                                    //25% xp to next level, min of 100k
                                    player.GrantLevelProportionalXp(0.25, 100000, long.MaxValue, true);
                                }
                                else if (player.Level >= 50 && player.Level < 150)
                                {
                                    //15% xp to next level, min of 1 mil
                                    player.GrantLevelProportionalXp(0.15, 1000000, long.MaxValue, true);
                                }
                                else if (player.Level >= 150)
                                {
                                    //10% xp to next level, min of 1 mil
                                    player.GrantLevelProportionalXp(0.1, 1000000, long.MaxValue, true);
                                }

                                //Give 8k lum
                                if (player.MaximumLuminance != null)
                                {
                                    player.GrantLuminance(8000, XpType.Quest, ShareType.None);
                                }

                                //Give 5 PK trophies
                                var pkTrophy = WorldObjectFactory.CreateNewWorldObject(1000002); //PK Trophy
                                pkTrophy.SetStackSize(5);
                                var pkTrophyCreateResult = player.TryCreateInInventoryWithNetworking(pkTrophy);
                                if (pkTrophyCreateResult)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageCreateObject(pkTrophy));
                                    var msg = new GameMessageSystemChat($"You have received 5 PK Trophies", ChatMessageType.Broadcast);
                                    player.Session.Network.EnqueueSend(msg);
                                }

                                //Give 1 Phial of Bloody Tears
                                var arenaTrophy = WorldObjectFactory.CreateNewWorldObject(1000003); //Phial of Bloody Tears
                                arenaTrophy.SetStackSize(1);
                                var arenaTrophyCreateResult = player.TryCreateInInventoryWithNetworking(arenaTrophy);
                                if (arenaTrophyCreateResult)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageCreateObject(arenaTrophy));
                                    var msg = new GameMessageSystemChat($"You have received a Phial of Bloody Tears", ChatMessageType.Broadcast);
                                    player.Session.Network.EnqueueSend(msg);
                                }

                                //Give 1 Darkbeat's Lost Storage Keys
                                var arenaKey = WorldObjectFactory.CreateNewWorldObject(480608); //Darkbeat's Lost Storage Keys
                                arenaKey.SetStackSize(1);
                                var arenaKeyCreateResult = player.TryCreateInInventoryWithNetworking(arenaKey);
                                if (arenaKeyCreateResult)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageCreateObject(arenaKey));
                                    var msg = new GameMessageSystemChat($"You have received one of Darkbeat's Lost Storage Keys", ChatMessageType.Broadcast);
                                    player.Session.Network.EnqueueSend(msg);
                                }
                                break;

                            case "ffa":

                                //This is the first place player only

                                //Give % xp to next level
                                if (player.Level > 0 && player.Level < 50)
                                {
                                    player.GrantLevelProportionalXp(1, 1000000, long.MaxValue, true);
                                }
                                else if (player.Level >= 50 && player.Level < 150)
                                {
                                    player.GrantLevelProportionalXp(0.75, 1000000, long.MaxValue, true);
                                }
                                else if (player.Level >= 150)
                                {
                                    player.GrantLevelProportionalXp(0.5, 1000000, long.MaxValue, true);
                                }

                                //Give 20k lum
                                if (player.MaximumLuminance != null)
                                {
                                    player.GrantLuminance(20000, XpType.Quest, ShareType.None);
                                }

                                //Give 5 PK trophies
                                var ffaWinnerPkTrophy = WorldObjectFactory.CreateNewWorldObject(1000002); //PK Trophy
                                ffaWinnerPkTrophy.SetStackSize(5);
                                var ffaWinnerPkTrophyCreateResult = player.TryCreateInInventoryWithNetworking(ffaWinnerPkTrophy);
                                if (ffaWinnerPkTrophyCreateResult)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaWinnerPkTrophy));
                                    var msg = new GameMessageSystemChat($"You have received 5 PK Trophies", ChatMessageType.Broadcast);
                                    player.Session.Network.EnqueueSend(msg);
                                }


                                //Give 3 Phial of Bloody Tears
                                var ffaWinner_arenaTrophy = WorldObjectFactory.CreateNewWorldObject(1000003); //Phial of Bloody Tears
                                ffaWinner_arenaTrophy.SetStackSize(3);
                                var ffaWinner_arenaTrophyCreateResult = player.TryCreateInInventoryWithNetworking(ffaWinner_arenaTrophy);
                                if (ffaWinner_arenaTrophyCreateResult)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaWinner_arenaTrophy));
                                    var msg = new GameMessageSystemChat($"You have received 3 Phials of Bloody Tears", ChatMessageType.Broadcast);
                                    player.Session.Network.EnqueueSend(msg);
                                }

                                //Give 1 Darkbeat's Lost Storage Keys
                                var ffaWinner_arenaKey = WorldObjectFactory.CreateNewWorldObject(480608); //Darkbeat's Lost Storage Keys
                                ffaWinner_arenaKey.SetStackSize(1);
                                var ffaWinner_arenaKeyCreateResult = player.TryCreateInInventoryWithNetworking(ffaWinner_arenaKey);
                                if (ffaWinner_arenaKeyCreateResult)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaWinner_arenaKey));
                                    var msg = new GameMessageSystemChat($"You have received one of Darkbeat's Lost Storage Keys", ChatMessageType.Broadcast);
                                    player.Session.Network.EnqueueSend(msg);
                                }
                                break;
                        }

                        SetPlayerRewardLimitProperties(player, winner);
                    }

                    ArenaManager.DispelArenaRares(player);
                }
            }

            //Process losers
            foreach (var loser in losers)
            {
                bool isFFA = loser.EventType.Equals("ffa");                
                bool isOvertime = this.ActiveEvent.IsOvertime;
                bool isDraw = (!isFFA && isOvertime) || (isFFA && loser.FinishPlace <= 3 && loser.FinishPlace > 0);

                uint? newRank = null;
                if (newRankings.ContainsKey(loser.CharacterId))
                {
                    newRank = newRankings[loser.CharacterId];
                }

                //Add to stats
                DatabaseManager.Log.AddToArenaStats(
                    loser.CharacterId,
                    loser.CharacterName,
                    loser.EventType,
                    1,
                    0,
                    isDraw || isOvertime ? 1 : (uint)0,
                    isDraw || isOvertime ? 0 : (uint)1,
                    loser.FinishPlace == -1 ? 1 : (uint)0,
                    loser.TotalDeaths,
                    loser.TotalKills,
                    loser.TotalDmgDealt,
                    loser.TotalDmgReceived,
                    newRank);

                var player = PlayerManager.GetOnlinePlayer(loser.CharacterId);
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Tough luck, you've lost the {ActiveEvent.EventTypeDisplay} arena event to {winnerList}\nIf you're still in the {ArenaName} arena you have a short period before you're teleported to your lifestone.", ChatMessageType.System));

                    if (loser.EventType.Equals("ffa"))
                    {
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The Free for All arena match in {ArenaName} has finished and you placed {loser.FinishPlaceDisplay}\nIf you're still in the {ArenaName} arena you have a short period before you're teleported to your lifestone.", ChatMessageType.System));
                    }

                    var shouldReward = IsPlayerRewardEligible(player, loser, ActiveEvent.Players) && !underageViolation;

                    if (shouldReward)
                    {
                        //Reward the losers here
                        switch (ActiveEvent.EventType)
                        {
                            case "1v1":
                            case "2v2":

                                //Give 2.5% xp to next level
                                player.GrantLevelProportionalXp(0.025, 250000, long.MaxValue, true);

                                //Give 3k lum
                                if (player.MaximumLuminance != null)
                                {
                                    player.GrantLuminance(3000, XpType.Quest, ShareType.None);
                                }

                                //Give 1 PK trophy
                                var pkTrophy = WorldObjectFactory.CreateNewWorldObject(1000002); //PK Trophy
                                pkTrophy.SetStackSize(1);
                                var pkTrophyCreateResult = player.TryCreateInInventoryWithNetworking(pkTrophy);
                                if (pkTrophyCreateResult)
                                {
                                    player.Session.Network.EnqueueSend(new GameMessageCreateObject(pkTrophy));
                                    var msg = new GameMessageSystemChat($"You have received a PK Trophy", ChatMessageType.Broadcast);
                                    player.Session.Network.EnqueueSend(msg);
                                }

                                //Give 25% chance for 1 Darkbeat's Lost Storage Keys
                                if (new Random().NextDouble() > 0.75)
                                {
                                    var arenaKey = WorldObjectFactory.CreateNewWorldObject(480608); //Darkbeat's Lost Storage Keys
                                    arenaKey.SetStackSize(1);
                                    var arenaKeyCreateResult = player.TryCreateInInventoryWithNetworking(arenaKey);
                                    if (arenaKeyCreateResult)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageCreateObject(arenaKey));
                                        var msg = new GameMessageSystemChat($"You have received one of Darkbeat's Lost Storage Keys", ChatMessageType.Broadcast);
                                        player.Session.Network.EnqueueSend(msg);
                                    }
                                }

                                break;

                            case "ffa":

                                if (loser.FinishPlace == 2)
                                {
                                    //Give % xp to next level
                                    if (player.Level > 0 && player.Level < 50)
                                    {
                                        player.GrantLevelProportionalXp(0.75, 1000000, long.MaxValue, true);
                                    }
                                    else if (player.Level >= 50 && player.Level < 150)
                                    {
                                        player.GrantLevelProportionalXp(0.25, 1000000, long.MaxValue, true);
                                    }
                                    else if (player.Level >= 150)
                                    {
                                        player.GrantLevelProportionalXp(0.15, 1000000, long.MaxValue, true);
                                    }

                                    //Give 8k lum
                                    if (player.MaximumLuminance != null)
                                    {
                                        player.GrantLuminance(8000, XpType.Quest, ShareType.None);
                                    }

                                    //Give 3 PK trophies
                                    var ffaLoserPkTrophy = WorldObjectFactory.CreateNewWorldObject(1000002); //PK Trophy
                                    ffaLoserPkTrophy.SetStackSize(3);
                                    var ffaLoserPkTrophyCreateResult = player.TryCreateInInventoryWithNetworking(ffaLoserPkTrophy);
                                    if (ffaLoserPkTrophyCreateResult)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaLoserPkTrophy));
                                        var msg = new GameMessageSystemChat($"You have received 3 PK Trophies", ChatMessageType.Broadcast);
                                        player.Session.Network.EnqueueSend(msg);
                                    }

                                    //Give 2 Phial of Bloody Tears
                                    var ffaLoser_arenaTrophy = WorldObjectFactory.CreateNewWorldObject(1000003); //Phial of Bloody Tears
                                    ffaLoser_arenaTrophy.SetStackSize(2);
                                    var ffaLoser_arenaTrophyCreateResult = player.TryCreateInInventoryWithNetworking(ffaLoser_arenaTrophy);
                                    if (ffaLoser_arenaTrophyCreateResult)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaLoser_arenaTrophy));
                                        var msg = new GameMessageSystemChat($"You have received 2 Phials of Bloody Tears", ChatMessageType.Broadcast);
                                        player.Session.Network.EnqueueSend(msg);
                                    }

                                    //Give 1 Darkbeat's Lost Storage Keys
                                    var ffaLoser_arenaKey = WorldObjectFactory.CreateNewWorldObject(480608); //Darkbeat's Lost Storage Keys
                                    ffaLoser_arenaKey.SetStackSize(1);
                                    var ffaLoser_arenaKeyCreateResult = player.TryCreateInInventoryWithNetworking(ffaLoser_arenaKey);
                                    if (ffaLoser_arenaKeyCreateResult)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaLoser_arenaKey));
                                        var msg = new GameMessageSystemChat($"You have received one of Darkbeat's Lost Storage Keys", ChatMessageType.Broadcast);
                                        player.Session.Network.EnqueueSend(msg);
                                    }
                                }
                                else if (loser.FinishPlace == 3)
                                {
                                    //Give % xp to next level
                                    if (player.Level > 0 && player.Level < 50)
                                    {
                                        player.GrantLevelProportionalXp(0.25, 1000000, long.MaxValue, true);
                                    }
                                    else if (player.Level >= 50 && player.Level < 150)
                                    {
                                        player.GrantLevelProportionalXp(0.15, 1000000, long.MaxValue, true);
                                    }
                                    else if (player.Level >= 150)
                                    {
                                        player.GrantLevelProportionalXp(0.05, 1000000, long.MaxValue, true);
                                    }

                                    //Give 5k lum
                                    if (player.MaximumLuminance != null)
                                    {
                                        player.GrantLuminance(5000, XpType.Quest, ShareType.None);
                                    }

                                    //Give 1 PK trophies
                                    var ffaLoserPkTrophy = WorldObjectFactory.CreateNewWorldObject(1000002); //PK Trophy
                                    ffaLoserPkTrophy.SetStackSize(1);
                                    var ffaLoserPkTrophyCreateResult = player.TryCreateInInventoryWithNetworking(ffaLoserPkTrophy);
                                    if (ffaLoserPkTrophyCreateResult)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaLoserPkTrophy));
                                        var msg = new GameMessageSystemChat($"You have received a PK Trophy", ChatMessageType.Broadcast);
                                        player.Session.Network.EnqueueSend(msg);
                                    }

                                    //Give 1 Phial of Bloody Tears
                                    var ffaLoser_arenaTrophy = WorldObjectFactory.CreateNewWorldObject(1000003); //Phial of Bloody Tears
                                    ffaLoser_arenaTrophy.SetStackSize(1);
                                    var ffaLoser_arenaTrophyCreateResult = player.TryCreateInInventoryWithNetworking(ffaLoser_arenaTrophy);
                                    if (ffaLoser_arenaTrophyCreateResult)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaLoser_arenaTrophy));
                                        var msg = new GameMessageSystemChat($"You have received a Phial of Bloody Tears", ChatMessageType.Broadcast);
                                        player.Session.Network.EnqueueSend(msg);
                                    }

                                    //Give 1 Darkbeat's Lost Storage Keys
                                    var ffaLoser_arenaKey = WorldObjectFactory.CreateNewWorldObject(480608); //Darkbeat's Lost Storage Keys
                                    ffaLoser_arenaKey.SetStackSize(1);
                                    var ffaLoser_arenaKeyCreateResult = player.TryCreateInInventoryWithNetworking(ffaLoser_arenaKey);
                                    if (ffaLoser_arenaKeyCreateResult)
                                    {
                                        player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaLoser_arenaKey));
                                        var msg = new GameMessageSystemChat($"You have received one of Darkbeat's Lost Storage Keys", ChatMessageType.Broadcast);
                                        player.Session.Network.EnqueueSend(msg);
                                    }
                                }
                                else
                                {
                                    //Give % xp to next level
                                    if (player.Level > 0 && player.Level < 50)
                                    {
                                        player.GrantLevelProportionalXp(0.1, 1000000, long.MaxValue, true);
                                    }
                                    else if (player.Level >= 50 && player.Level < 150)
                                    {
                                        player.GrantLevelProportionalXp(0.05, 1000000, long.MaxValue, true);
                                    }
                                    else if (player.Level >= 150)
                                    {
                                        player.GrantLevelProportionalXp(0.025, 1000000, long.MaxValue, true);
                                    }

                                    //Give 3k lum
                                    if (player.MaximumLuminance != null)
                                    {
                                        player.GrantLuminance(3000, XpType.Quest, ShareType.None);
                                    }

                                    //25% chance to give 1 Darkbeat's Lost Storage Keys
                                    if (new Random().NextDouble() > 0.75)
                                    {
                                        var ffaLoser_arenaKey = WorldObjectFactory.CreateNewWorldObject(480608); //Darkbeat's Lost Storage Keys
                                        ffaLoser_arenaKey.SetStackSize(1);
                                        var ffaLoser_arenaKeyCreateResult = player.TryCreateInInventoryWithNetworking(ffaLoser_arenaKey);
                                        if (ffaLoser_arenaKeyCreateResult)
                                        {
                                            player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaLoser_arenaKey));
                                            var msg = new GameMessageSystemChat($"You have received one of Darkbeat's Lost Storage Keys", ChatMessageType.Broadcast);
                                            player.Session.Network.EnqueueSend(msg);
                                        }
                                    }
                                }
                                break;
                        }

                        SetPlayerRewardLimitProperties(player, loser);
                    }

                    ArenaManager.DispelArenaRares(player);
                }
            }

            //Global Broadcast
            var globalMsg = $"{winnerList} just won a {ActiveEvent.EventTypeDisplay} arena event against {loserList} in {ArenaName}";
            PlayerManager.BroadcastToAll(new GameMessageSystemChat(globalMsg, ChatMessageType.Broadcast));
            try
            {
                var webhookUrl = PropertyManager.GetString("arena_globals_webhook").Item;
                if (!string.IsNullOrEmpty(webhookUrl))
                {
                    _ = TurbineChatHandler.SendWebhookedChat("Arenas", globalMsg, webhookUrl, "Global");
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed sending Arena global message to webhook. Ex:{0}", ex);
            }

        }

        public void SetPlayerRewardLimitProperties(Player player, ArenaPlayer arenaPlayer)
        {
            //Set player reward limit properties
            //Daily reward time stamp and count
            if (!player.ArenaDailyRewardTimestamp.HasValue || Time.GetDateTimeFromTimestamp(player.ArenaDailyRewardTimestamp.Value) < DateTime.Today)
            {
                player.ArenaDailyRewardTimestamp = Time.GetUnixTime(DateTime.Today);
                player.ArenaDailyRewardCount = 0;
                player.ArenaSameClanDailyRewardCount = 0;
                player.ArenaRewardsByOpponent = null;
            }

            player.ArenaDailyRewardCount++;

            //Same clan opponents count
            var sameClanOpponents = ActiveEvent.Players.Where(x => x.MonarchId == arenaPlayer.MonarchId && x.TeamGuid != arenaPlayer.TeamGuid);
            if (sameClanOpponents != null && sameClanOpponents.Count() > 0)
                player.ArenaSameClanDailyRewardCount++;

            //Hourly reward time stamp and count
            DateTime thisHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            if (!player.ArenaHourlyTimestamp.HasValue || Time.GetDateTimeFromTimestamp(player.ArenaHourlyTimestamp.Value) < thisHour)
            {
                player.ArenaHourlyTimestamp = Time.GetUnixTime(thisHour);
                player.ArenaHourlyCount = 0;
            }

            player.ArenaHourlyCount++;

            //Opponents Log
            var opponents = ActiveEvent.Players.Where(x => x.TeamGuid != arenaPlayer.TeamGuid);
            if (opponents != null)
            {
                var opponentRewards = player.ArenaRewardsByOpponent;
                foreach (var opponent in opponents)
                {
                    if (opponentRewards != null && opponentRewards.ContainsKey(opponent.CharacterId))
                    {
                        opponentRewards[opponent.CharacterId]++;
                    }
                    else
                    {
                        opponentRewards.Add(opponent.CharacterId, 1);
                    }
                }
                player.ArenaRewardsByOpponent = opponentRewards;
            }
        }

        public bool IsPlayerRewardEligible(Player player, ArenaPlayer arenaPlayer, List<ArenaPlayer> allArenaPlayers)
        {
            if (arenaPlayer.FinishPlace == -1)
                return false;

            if (player == null || player.Age <= PropertyManager.GetLong("arenas_reward_min_age").Item) //days in seconds
            {
                return false;
            }

            //You can't get more than 20 rewards per day
            if (player.ArenaDailyRewardCount >= 20 && (player.ArenaDailyRewardTimestamp ?? 0) >= Time.GetUnixTime(DateTime.Today))
            {
                return false;
            }

            //You can't get more than 6 rewards per hour
            if (player.ArenaHourlyCount >= 6 && (player.ArenaHourlyTimestamp ?? 0) >= Time.GetUnixTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0)))
            {
                return false;
            }

            //You can't get more than 5 rewards playing against a clanmate per day
            if (player.ArenaSameClanDailyRewardCount >= 5 && (player.ArenaDailyRewardTimestamp ?? 0) >= Time.GetUnixTime(DateTime.Today))
            {
                var sameClanOpponents = allArenaPlayers
                    .Where(x =>
                      x.CharacterId != arenaPlayer.CharacterId &&
                      x.TeamGuid != arenaPlayer.TeamGuid &&
                      x.MonarchId == arenaPlayer.MonarchId);

                if (sameClanOpponents != null && sameClanOpponents.Count() > 0)
                {
                    return false;
                }
            }

            //You can't get more than 3 rewards from same person per day
            if ((player.ArenaDailyRewardTimestamp ?? 0) >= Time.GetUnixTime(DateTime.Today))
            {
                var todaysArenaRewardsByOpponent = player.ArenaRewardsByOpponent;
                foreach (var opponent in allArenaPlayers)
                {
                    if (arenaPlayer.CharacterId != opponent.CharacterId &&
                        arenaPlayer.TeamGuid != opponent.TeamGuid &&
                        todaysArenaRewardsByOpponent.ContainsKey(opponent.CharacterId) &&
                        todaysArenaRewardsByOpponent[opponent.CharacterId] >= 3)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void EndEventTimelimitExceeded()
        {
            log.Info($"ArenaLocation.EndEventTimelimitExceeded() - {ArenaName}");
            ActiveEvent.EndDateTime = DateTime.Now;
            ActiveEvent.Status = 6;

            //Set FinishPlace for all living players 
            var remainingPlayers = ActiveEvent.Players.Where(x => !x.IsDisqualified && !x.IsEliminated);
            if (remainingPlayers != null && remainingPlayers.Count() > 0)
            {
                foreach (var arenaPlayer in remainingPlayers)
                {
                    arenaPlayer.FinishPlace = remainingPlayers.Count();
                }
            }

            DatabaseManager.Log.SaveArenaEvent(ActiveEvent);

            bool underageViolation = false;
            var underageCount = 0;
            foreach (var arenaPlayer in ActiveEvent.Players)
            {
                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);

                if (player.Age <= PropertyManager.GetLong("arenas_reward_min_age").Item) //days in seconds
                {
                    underageCount++;
                }
            }

            if (underageCount > 0 && (ActiveEvent.EventType.Equals("1v1") || ActiveEvent.EventType.Equals("2v2")))
            {
                underageViolation = true;
            }
            else if (underageCount > 2 && ActiveEvent.EventType.Equals("ffa"))
            {
                underageViolation = true;
            }

            foreach (var arenaPlayer in ActiveEvent.Players)
            {
                var isLoss = arenaPlayer.FinishPlace > 3 || arenaPlayer.FinishPlace < 1;
                var isDq = arenaPlayer.FinishPlace == -1;

                DatabaseManager.Log.AddToArenaStats(
                    arenaPlayer.CharacterId,
                    arenaPlayer.CharacterName,
                    arenaPlayer.EventType,
                    1,
                    0,
                    isLoss ? 0 : (uint)1,
                    isLoss ? 1 : (uint)0,
                    isDq ? 1 : (uint)0,
                    arenaPlayer.TotalDeaths,
                    arenaPlayer.TotalKills,
                    arenaPlayer.TotalDmgDealt,
                    arenaPlayer.TotalDmgReceived);

                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {ActiveEvent.EventTypeDisplay} arena event has ended in a draw.  If you are still in the arena you can recall now or have a short period before you are teleported to your lifestone.", ChatMessageType.System));

                    var shouldReward = IsPlayerRewardEligible(player, arenaPlayer, ActiveEvent.Players) && !underageViolation;

                    if (shouldReward)
                    {
                        //Give % xp to next level
                        if (player.Level > 0 && player.Level < 50)
                        {
                            player.GrantLevelProportionalXp(0.035, 1, long.MaxValue, true);
                        }
                        else if (player.Level >= 50 && player.Level < 150)
                        {
                            player.GrantLevelProportionalXp(0.025, 1, long.MaxValue, true);
                        }
                        else if (player.Level >= 150)
                        {
                            player.GrantLevelProportionalXp(0.01, 1, long.MaxValue, true);
                        }

                        //25% chance to give 1 Darkbeat's Lost Storage Keys
                        if (new Random().NextDouble() > 0.75)
                        {
                            var ffaLoser_arenaKey = WorldObjectFactory.CreateNewWorldObject(480608); //Darkbeat's Lost Storage Keys
                            ffaLoser_arenaKey.SetStackSize(1);
                            var ffaLoser_arenaKeyCreateResult = player.TryCreateInInventoryWithNetworking(ffaLoser_arenaKey);
                            if (ffaLoser_arenaKeyCreateResult)
                            {
                                player.Session.Network.EnqueueSend(new GameMessageCreateObject(ffaLoser_arenaKey));
                                var msg = new GameMessageSystemChat($"You have received one of Darkbeat's Lost Storage Keys", ChatMessageType.Broadcast);
                                player.Session.Network.EnqueueSend(msg);
                            }
                        }

                        SetPlayerRewardLimitProperties(player, arenaPlayer);
                    }

                    ArenaManager.DispelArenaRares(player);
                }
            }

            var drawMsg = $"Arena event ended in a draw: {ActiveEvent.EventTypeDisplay} - {ActiveEvent.PlayersDisplay} - {ArenaName}";
            PlayerManager.BroadcastToAll(new GameMessageSystemChat(drawMsg, ChatMessageType.Broadcast));
            try
            {
                var webhookUrl = PropertyManager.GetString("arena_globals_webhook").Item;
                if (!string.IsNullOrEmpty(webhookUrl))
                {
                    _ = TurbineChatHandler.SendWebhookedChat("Arenas", drawMsg, webhookUrl, "Global");
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed sending Arena global message to webhook. Ex:{0}", ex);
            }
        }

        public void EndEventCancel()
        {
            log.Info($"ArenaLocation.EndEventCancel() - {ArenaName}");
            ActiveEvent.EndDateTime = DateTime.Now;
            ActiveEvent.Status = -1;

            DatabaseManager.Log.SaveArenaEvent(ActiveEvent);

            foreach (var arenaPlayer in ActiveEvent.Players)
            {
                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                if (player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {ActiveEvent.EventTypeDisplay} arena event was cancelled before it started.", ChatMessageType.System));
                    ArenaManager.DispelArenaRares(player);
                }
            }

            ActiveEvent = null;
        }

        public void ClearPlayersFromArena()
        {
            //log.Info($"ArenaLocation.ClearPlayersFromArena() - {this.ArenaName}");

            try
            {
                var arenaLandblock = LandblockManager.GetLandblock(new LandblockId(LandblockId << 16), false);
                var playerList = arenaLandblock.GetCurrentLandblockPlayers();
                foreach (var player in playerList)
                {
                    if (player.IsAdmin)
                        continue;

                    if (player.IsPendingArenaObserver || player.IsArenaObserver)
                    {
                        ArenaManager.ExitArenaObserverMode(player);
                        continue;
                    }

                    player.Teleport(player.Sanctuary);
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat("You've been teleported to your lifestone because you were inside an arena location without being an active participant in an arena event", ChatMessageType.System));
                }
            }
            catch (Exception ex)
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

            ////Binding Realm
            //var bindRealm = new ArenaLocation();
            //bindRealm.LandblockId = 0x007F;
            //bindRealm.SupportedEventTypes = new List<string>();
            //bindRealm.SupportedEventTypes.Add("1v1");
            //bindRealm.SupportedEventTypes.Add("2v2");
            //bindRealm.ArenaName = "Binding Realm";
            //locList.Add(bindRealm.LandblockId, bindRealm);

            //0x0145, //Bone Lair            
            var boneLair = new ArenaLocation();
            boneLair.LandblockId = 0x0145;
            boneLair.SupportedEventTypes = new List<string>();
            boneLair.SupportedEventTypes.Add("ffa");
            boneLair.ArenaName = "Bone Lair";
            locList.Add(boneLair.LandblockId, boneLair);

            //0x01AD, //Dungeon Galley Tower            
            var galleyTower = new ArenaLocation();
            galleyTower.LandblockId = 0x01AD;
            galleyTower.SupportedEventTypes = new List<string>();
            galleyTower.SupportedEventTypes.Add("ffa");
            galleyTower.ArenaName = "Galley Tower";
            locList.Add(galleyTower.LandblockId, galleyTower);

            //0x02E3, //Yaraq PK Arena
            var ypk = new ArenaLocation();
            ypk.LandblockId = 0x02E3;
            ypk.SupportedEventTypes = new List<string>();
            ypk.SupportedEventTypes.Add("ffa");
            ypk.ArenaName = "Yaraq PK Arena";
            locList.Add(ypk.LandblockId, ypk);

            //0xECEC //Pyramid -Admin Island
            var pyramid = new ArenaLocation();
            pyramid.LandblockId = 0xECEC;
            pyramid.SupportedEventTypes = new List<string>();
            pyramid.SupportedEventTypes.Add("ffa");
            pyramid.ArenaName = "Pyramid";
            locList.Add(pyramid.LandblockId, pyramid);

            //Mad Cow Arena
            //var cow = new ArenaLocation();
            //cow.LandblockId = 0x00AF;
            //cow.SupportedEventTypes = new List<string>();
            //cow.SupportedEventTypes.Add("1v1");
            //cow.SupportedEventTypes.Add("2v2");
            //cow.ArenaName = "Mad Cow";
            //locList.Add(cow.LandblockId, cow);

            //Fowl Basement
            var fowl = new ArenaLocation();
            fowl.LandblockId = 0x596A;
            fowl.SupportedEventTypes = new List<string>();
            fowl.SupportedEventTypes.Add("1v1");
            fowl.SupportedEventTypes.Add("2v2");
            fowl.ArenaName = "Fowl Basement";
            locList.Add(fowl.LandblockId, fowl);

            //Landing Strip
            var ls = new ArenaLocation();
            ls.LandblockId = 0xD50E;
            ls.SupportedEventTypes = new List<string>();
            ls.SupportedEventTypes.Add("1v1");
            ls.SupportedEventTypes.Add("2v2");
            ls.ArenaName = "The Landing Strip";
            locList.Add(ls.LandblockId, ls);

            //The Heptagon
            var heptagon = new ArenaLocation();
            heptagon.LandblockId = 0x7222;
            heptagon.SupportedEventTypes = new List<string>();
            heptagon.SupportedEventTypes.Add("1v1");
            heptagon.SupportedEventTypes.Add("2v2");
            heptagon.ArenaName = "The Heptagon";
            locList.Add(heptagon.LandblockId, heptagon);

            return locList;
        }

        private static List<uint> _arenaLandblocks;
        public static List<uint> ArenaLandblocks
        {
            get
            {
                if (_arenaLandblocks == null)
                {
                    _arenaLandblocks = new List<uint>()
                    {
                        0x0067, //PKL Arena
                        //0x007F,  //Binding Realm
                        0x0145, //Bone Lair
                        0x01AD, //Dungeon Galley Tower
                        0x02E3, //Yaraq PK Arena
                        0xECEC, //Pyramid -Admin Island
                        //0x00AF, //Mad Cow Arena
                        0x596A, //Fowl Basement
                        0xD50E, //Landing Strip
                        0x7222  //The Heptagon
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

                    //_arenaLocationStartingPositions.Add(
                    //    0x007F,
                    //    new List<Position>()
                    //    {
                    //        new Position(0x007F0101, 236.75943f, -22.896727f, -59.995f, 0.000000f, 0.000000f, -0.6899546f, 0.72385263f), //West
                    //        //0x007F0101 [236.75943 -22.896727 -59.995] 0.72385263 0 0 -0.6899546
                    //        new Position(0x007F0107, 262.50168f, -16.419994f, -59.995f, 0.000000f, 0.000000f, -0.8109761f, -0.58507925f),//East
                    //        //0x007F0107 [262.50168 -16.419994 -59.995] -0.58507925 0 0 -0.8109761
                    //        new Position(0x007F0103, 250.935226f, -5.749045f, -59.994999f, 0.000000f, 0.000000f, -0.999384f, -0.035106f),//North
                    //        //0x007F0103 [250.935226 -5.749045 -59.994999] 0.035106 0.000000 0.000000 -0.999384
                    //        new Position(0x007F0105, 252.860062f, -33.350712f, -59.994999f, 0.000000f, 0.000000f, -0.043396f, -0.999058f),//South
                    //        //0x007F0105 [252.860062 -33.350712 -59.994999] -0.999058 0.000000 0.000000 -0.043396
                    //    }); //Binding Realm

                    _arenaLocationStartingPositions.Add(
                        0x02E3,
                        new List<Position>()
                        {
                            new Position(0x02E30100, 54.954102f, -53.559502f, 0.005000f, 0.000000f, 0.000000f, -0.706410f, 0.707802f),
                            //0x02E30100[54.954102 -53.559502 0.005000] 0.707802 0.000000 0.000000 -0.706410
                            new Position(0x02E30185, 87.640144f, -54.995811f, 12.004999f, 0.000000f, 0.000000f, -0.701326f, -0.712840f),
                            //0x02E30185[87.640144 -54.995811 12.004999] -0.712840 0.000000 0.000000 -0.701326
                            new Position(0x02E3014A, 54.859180f, -20.170738f, 12.004999f, 0.000000f, 0.000000f, -0.999738f, -0.022893f),
                            //0x02E3014A[54.859180 -20.170738 12.004999] -0.022893 0.000000 0.000000 -0.999738
                            new Position(0x02E30131, 21.610455f, -54.913197f, 12.004999f, 0.000000f, 0.000000f, -0.714008f, 0.700137f),
                            //0x02E30131[21.610455 -54.913197 12.004999] 0.700137 0.000000 0.000000 -0.714008
                            new Position(0x02E3015D, 55.055714f, -88.516785f, 12.004999f, 0.000000f, 0.000000f, 0.008961f, 0.999960f),
                            //0x02E3015D[55.055714 -88.516785 12.004999] 0.999960 0.000000 0.000000 0.008961
                            new Position(0x02E30107, 37.609787f, -55.014618f, 6.005000f, 0.000000f, 0.000000f, -0.714715f, 0.699416f),
                            //0x02E30107[37.609787 -55.014618 6.005000] 0.699416 0.000000 0.000000 -0.714715
                            new Position(0x02E30112, 55.120712f, -72.074440f, 6.005000f, 0.000000f, 0.000000f, 0.013379f, 0.999910f),
                            //0x02E30112[55.120712 -72.074440 6.005000] 0.999910 0.000000 0.000000 0.013379
                            new Position(0x02E30119, 71.587006f, -55.205040f, 6.005000f, 0.000000f, 0.000000f, 0.688900f, 0.724856f),
                            //0x02E30119[71.587006 -55.205040 6.005000] 0.724856 0.000000 0.000000 0.688900
                            new Position(0x02E3010F, 55.080246f, -37.959023f, 6.005000f, 0.000000f, 0.000000f, 0.999951f, 0.009948f),
                            //0x02E3010F[55.080246 -37.959023 6.005000] 0.009948 0.000000 0.000000 0.999951
                            new Position(0x02E3015A, 59.596912f, -55.734844f, 12.054999f, 0.000000f, 0.000000f, -0.717913f, -0.696133f),
                            //0x02E3015A[59.596912 -55.734844 12.054999] -0.696133 0.000000 0.000000 -0.717913
                        }); //Yaraq PK Arena

                    _arenaLocationStartingPositions.Add(
                        0xECEC,
                        new List<Position>()
                        {
                            new Position(0xECEC012D, 84.326401f, 37.399399f, 0.205000f, 0.000000f, 0.000000f, -0.010572f, -0.999944f),
                            //0xECEC012D[84.326401 37.399399 0.205000] -0.999944 0.000000 0.000000 -0.010572
                            new Position(0xECEC011F, 131.125320f, 36.631672f, 0.205000f, 0.000000f, 0.000000f, -0.333597f, -0.942716f),
                            //0xECEC011F[131.125320 36.631672 0.205000] -0.942716 0.000000 0.000000 -0.333597
                            new Position(0xECEC012C, 132.890320f, 84.706825f, 0.205000f, 0.000000f, 0.000000f, -0.717156f, -0.696913f),
                            //0xECEC012C[132.890320 84.706825 0.205000] -0.696913 0.000000 0.000000 -0.717156
                            new Position(0xECEC0122, 131.195908f, 130.575638f, 0.205000f, 0.000000f, 0.000000f, -0.916779f, -0.399395f),
                            //0xECEC0122[131.195908 130.575638 0.205000] -0.399395 0.000000 0.000000 -0.916779
                            new Position(0xECEC012B, 83.108978f, 131.664230f, 0.205000f, 0.000000f, 0.000000f, -0.999970f, 0.007704f),
                            //0xECEC012B[83.108978 131.664230 0.205000] 0.007704 0.000000 0.000000 -0.999970
                            new Position(0xECEC0121, 37.388470f, 130.349777f, 0.205000f, 0.000000f, 0.000000f, -0.926284f, 0.376825f),
                            //0xECEC0121[37.388470 130.349777 0.205000] 0.376825 0.000000 0.000000 -0.926284
                            new Position(0xECEC012E, 36.323086f, 83.237755f, 0.205000f, 0.000000f, 0.000000f, -0.679018f, 0.734121f),
                            //0xECEC012E[36.323086 83.237755 0.205000] 0.734121 0.000000 0.000000 -0.679018
                            new Position(0xECEC0120, 37.112888f, 37.668873f, 0.205000f, 0.000000f, 0.000000f, -0.401558f, 0.915833f),
                            //0xECEC0120[37.112888 37.668873 0.205000] 0.915833 0.000000 0.000000 -0.401558
                            new Position(0xECEC0105, 72.137177f, 84.142395f, 0.205000f, 0.000000f, 0.000000f, -0.702030f, 0.712147f),
                            //0xECEC0105[72.137177 84.142395 0.205000] 0.712147 0.000000 0.000000 -0.702030
                            new Position(0xECEC0110, 97.596474f, 83.986618f, 0.205000f, 0.000000f, 0.000000f, 0.715533f, 0.698579f),
                            //0xECEC0110[97.596474 83.986618 0.205000] 0.698579 0.000000 0.000000 0.715533
                        }); //Pyramid -Admin Island

                    _arenaLocationStartingPositions.Add(
                        0x01AD,
                        new List<Position>()
                        {
                            new Position(0x01AD0116, 29.940001f, -32.599998f, 0.005000f, 0.000000f, 0.000000f, -0.008727f, 0.999962f),
                            //0x01AD0116[29.940001 -32.599998 0.005000] 0.999962 0.000000 0.000000 -0.008727
                            new Position(0x01AD011A, 40.046265f, -1.053921f, 0.005000f, 0.000000f, 0.000000f, -0.902557f, -0.430570f),
                            //0x01AD011A[40.046265 -1.053921 0.005000] -0.430570 0.000000 0.000000 -0.902557
                            new Position(0x01AD0100, 0.060270f, 2.052596f, 0.005000f, 0.000000f, 0.000000f, -0.929969f, 0.367637f),
                            //0x01AD0100[0.060270 2.052596 0.005000] 0.367637 0.000000 0.000000 -0.929969
                            new Position(0x01AD0142, 39.877190f, -9.612027f, 6.005000f, 0.000000f, 0.000000f, -0.999993f, 0.003741f),
                            //0x01AD0142[39.877190 -9.612027 6.005000] 0.003741 0.000000 0.000000 -0.999993
                            new Position(0x01AD014B, 61.338745f, 1.616516f, 6.005000f, 0.000000f, 0.000000f, -0.923844f, -0.382769f),
                            //0x01AD014B[61.338745 1.616516 6.005000] -0.382769 0.000000 0.000000 -0.923844
                            new Position(0x01AD014E, 59.279133f, -31.289797f, 6.005000f, 0.000000f, 0.000000f, -0.310898f, -0.950443f),
                            //0x01AD014E[59.279133 -31.289797 6.005000] -0.950443 0.000000 0.000000 -0.310898
                            new Position(0x01AD0144, 40.064552f, -25.380503f, 6.005000f, 0.000000f, 0.000000f, -0.017525f, -0.999846f),
                            //0x01AD0144[40.064552 -25.380503 6.005000] -0.999846 0.000000 0.000000 -0.017525
                            new Position(0x01AD0135, 18.353422f, -20.375107f, 6.005000f, 0.000000f, 0.000000f, 0.872478f, -0.488653f),
                            //0x01AD0135[18.353422 -20.375107 6.005000] -0.488653 0.000000 0.000000 0.872478
                            new Position(0x01AD016E, 40.068432f, 0.782179f, 18.004999f, 0.000000f, 0.000000f, -0.919731f, -0.392550f),
                            //0x01AD016E[40.068432 0.782179 18.004999] -0.392550 0.000000 0.000000 -0.919731
                            new Position(0x01AD0155, 30.068464f, -20.947460f, 12.004999f, 0.000000f, 0.000000f, -0.999852f, 0.017178f),
                            //0x01AD0155[30.068464 -20.947460 12.004999] 0.017178 0.000000 0.000000 -0.999852
                        }); //Dungeon Galley Tower

                    _arenaLocationStartingPositions.Add(
                        0x0145,
                        new List<Position>()
                        {
                            new Position(0x014501A3, 96.569344f, -50.926197f, 6.005000f, 0.000000f, 0.000000f, -0.189269f, -0.981925f),
                            //0x014501A3[96.569344 -50.926197 6.005000] -0.981925 0.000000 0.000000 -0.189269
                            new Position(0x014501A1, 97.130882f, -27.609722f, 6.005000f, 0.000000f, 0.000000f, -0.953241f, -0.302211f),
                            //0x014501A1[97.130882 -27.609722 6.005000] -0.302211 0.000000 0.000000 -0.953241
                            new Position(0x0145010E, 100.117851f, -50.360348f, -5.995000f, 0.000000f, 0.000000f, 0.003506f, 0.999994f),
                            //0x0145010E[100.117851 -50.360348 -5.995000] 0.999994 0.000000 0.000000 0.003506
                            new Position(0x0145010B, 99.959366f, -29.854738f, -5.995000f, 0.000000f, 0.000000f, 0.999639f, 0.026883f),
                            //0x0145010B[99.959366 -29.854738 -5.995000] 0.026883 0.000000 0.000000 0.999639
                            new Position(0x01450149, 71.072929f, -26.698454f, 0.005000f, 0.000000f, 0.000000f, -0.996001f, 0.089337f),
                            //0x01450149[71.072929 -26.698454 0.005000] 0.089337 0.000000 0.000000 -0.996001
                            new Position(0x0145014B, 72.077400f, -52.935970f, 0.005000f, 0.000000f, 0.000000f, -0.170279f, 0.985396f),
                            //0x0145014B[72.077400 -52.935970 0.005000] 0.985396 0.000000 0.000000 -0.170279
                            new Position(0x01450117, 29.986937f, -13.254610f, 0.005000f, 0.000000f, 0.000000f, 0.999936f, 0.011354f),
                            //0x01450117[29.986937 -13.254610 0.005000] 0.011354 0.000000 0.000000 0.999936
                            new Position(0x01450185, 29.381468f, -40.134899f, 6.005000f, 0.000000f, 0.000000f, 0.686305f, -0.727314f),
                            //0x01450185[29.381468 -40.134899 6.005000] -0.727314 0.000000 0.000000 0.686305
                            new Position(0x01450121, 29.054979f, -57.933998f, 0.005000f, 0.000000f, 0.000000f, 0.015373f, 0.999882f),
                            //0x01450121[29.054979 -57.933998 0.005000] 0.999882 0.000000 0.000000 0.015373
                            new Position(0x01450114, 5.084766f, -76.363258f, 0.005000f, 0.000000f, 0.000000f, -0.394789f, 0.918772f),
                            //0x01450114[5.084766 -76.363258 0.005000] 0.918772 0.000000 0.000000 -0.394789
                        }); //Bone Lair

                    //_arenaLocationStartingPositions.Add(
                    //0x00AF,
                    //new List<Position>()
                    //{
                    //        new Position(0x00AF015D, 201.930344f, -47.542015f, 0.110000f, 0.000000f, 0.000000f, 0.000000f, 1.000000f),
                    //        //0x00AF015D [201.930344 -47.542015 0.110000] 1.000000 0.000000 0.000000 0.000000
                    //        new Position(0x00AF015B, 201.882141f, -32.338894f, 0.110000f, 0.000000f, 0.000000f, 0.999997f, -0.002561f),
                    //        //0x00AF015B [201.882141 -32.338894 0.110000] -0.002561 0.000000 0.000000 0.999997
                    //        new Position(0x00AF015D, 198.203186f, -47.391590f, 0.110000f, 0.000000f, 0.000000f, 0.010101f, 0.999949f),
                    //        //0x00AF015D [198.203186 -47.391590 0.110000] 0.999949 0.000000 0.000000 0.010101
                    //        new Position(0x00AF015B, 198.134445f, -32.373367f, 0.110000f, 0.000000f, 0.000000f, -0.999939f, -0.011021f),
                    //        //0x00AF015B [198.134445 -32.373367 0.110000] -0.011021 0.000000 0.000000 -0.999939   
                    //}); //Mad Cow Arena

                    _arenaLocationStartingPositions.Add(
                    0x596A,
                    new List<Position>()
                    {
                            new Position(0x596A010C, 35.238209f, -18.218103f, 0.005000f, 0.000000f, 0.000000f, -0.707107f, -0.707107f),
                            //0x596A010C [35.238209 -18.218103 0.005000] -0.707107 0.000000 0.000000 -0.707107
                            new Position(0x596A0102, 5.035136f, -18.326546f, 0.005000f, 0.000000f, 0.000000f, -0.695197f, 0.718819f),
                            //0x596A0102 [5.035136 -18.326546 0.005000] 0.718819 0.000000 0.000000 -0.695197
                            new Position(0x596A010A, 34.787258f, -21.811909f, 0.005000f, 0.000000f, 0.000000f, 0.713734f, 0.700416f),
                            //0x596A010A [34.787258 -21.811909 0.005000] 0.700416 0.000000 0.000000 0.713734
                            new Position(0x596A0102, 5.146578f, -21.661127f, 0.005000f, 0.000000f, 0.000000f, -0.695197f, 0.718819f),
                            //0x596A0102 [5.146578 -21.661127 0.005000] 0.718819 0.000000 0.000000 -0.695197
                    }); //Fowl Basement

                    _arenaLocationStartingPositions.Add(
                    0xD50E,
                    new List<Position>()
                    {
                            new Position(0xD50E0012, 48.137695f, 40.723217f, -0.095000f, 0.000000f, 0.000000f, 0.011873f, -0.999929f),
                            //0xD50E0012 [48.137695 40.723217 -0.095000] -0.999929 0.000000 0.000000 0.011873
                            new Position(0xD50E000E, 47.949497f, 130.815628f, -0.095000f, 0.000000f, 0.000000f, -0.999986f, 0.005301f),
                            //0xD50E000E [47.949497 130.815628 -0.095000] 0.005301 0.000000 0.000000 -0.999986
                            new Position(0xD50E000D, 36.021290f, 96.158844f, -0.095000f, 0.000000f, 0.000000f, -0.707318f, 0.706895f),
                            //0xD50E000D [36.021290 96.158844 -0.095000] 0.706895 0.000000 0.000000 -0.707318
                            new Position(0xD50E0015, 61.040390f, 96.038742f, -0.445000f, 0.000000f, 0.000000f, -0.707665f, -0.706548f),
                            //0xD50E0015 [61.040390 96.038742 -0.445000] -0.706548 0.000000 0.000000 -0.707665
                    }); //Landing Strip

                    _arenaLocationStartingPositions.Add(
                    0x7222,
                    new List<Position>()
                    {
                            new Position(0x7222002D, 131.824692f, 96.041451f, -0.445000f, 0.000000f, 0.000000f, -0.709499f, 0.704706f),
                            // 0x7222002D [131.824692 96.041451 -0.445000] 0.704706 0.000000 0.000000 -0.709499
                            new Position(0x72220034, 156.016724f, 95.997833f, -0.445000f, 0.000000f, 0.000000f, -0.719308f, -0.694692f),
                            // 0x72220034 [156.016724 95.997833 -0.445000] -0.694692 0.000000 0.000000 -0.719308
                            new Position(0x7222002D, 143.945587f, 110.087395f, -0.445000f, 0.000000f, 0.000000f, -0.999998f, -0.002127f),
                            // 0x7222002D [143.945587 110.087395 -0.445000] -0.002127 0.000000 0.000000 -0.999998
                            new Position(0x7222002C, 143.886292f, 81.059364f, -0.445000f, 0.000000f, 0.000000f, 0.004404f, -0.999990f),
                            // 0x7222002C [143.886292 81.059364 -0.445000] -0.999990 0.000000 0.000000 0.004404
                    }); //The Heptagon
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
