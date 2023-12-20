using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

using Biota = ACE.Entity.Models.Biota;
using ACE.Database.Models.Log;
using ACE.Server.Network.GameAction.Actions;
using ACE.Server.Entity.Chess;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using ACE.Server.Entity.Actions;
using Microsoft.Extensions.Logging;
using ACE.Server.Entity;

namespace ACE.Server.Managers
{
    public static class ArenaManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Dictionary<uint, ArenaPlayer> queuedPlayers = new Dictionary<uint, ArenaPlayer>();
        private static Dictionary<uint, ArenaLocation> arenaLocations = new Dictionary<uint, ArenaLocation>();

        public static void Initialize()
        {
            arenaLocations = ArenaLocation.InitializeArenaLocations();
        }

        private static DateTime LastTickDateTime = DateTime.MinValue;
        public static void Tick()
        {
            if (DateTime.Now.AddSeconds(-1) < LastTickDateTime)
                return;

            bool isArenasDisabled = PropertyManager.GetBool("disable_arenas").Item;
            if (isArenasDisabled)
            {
                foreach (var arena in arenaLocations)
                {
                    if (arena.Value.HasActiveEvent)
                    {
                        arena.Value.EndEventTimelimitExceeded();
                        arena.Value.ClearPlayersFromArena();
                        arena.Value.ActiveEvent = null;
                    }
                }

                if (queuedPlayers.Count() > 0)
                    queuedPlayers.Clear();

                LastTickDateTime = DateTime.Now;

                return;
            }

            //Run arena location tick for each arena
            //If there's no active event, the arena will look for queued players to match make
            //If there is an active event, call its tick method to process it thru its lifecycle
            var randomizedLocationList = arenaLocations.Values.OrderBy(x => Guid.NewGuid());
            foreach (var arena in randomizedLocationList)
            {
                arena.Tick();
            }

            //Arena Observers who are somehow still flagged as an observer without being an active event's observers list
            //var players = PlayerManager.GetAllOnline();
            //foreach (var player in players)
            //{
            //    if (player?.IsArenaObserver ?? false)
            //    {
            //        var arenaLoc = arenaLocations.Values.FirstOrDefault(x => x.ActiveEvent.Observers.Contains(player.Character.Id));
            //        if(arenaLoc == null)
            //            ExitArenaObserverMode(player);
            //    }
            //}

            LastTickDateTime = DateTime.Now;
        }

        public static List<ArenaEvent> GetActiveEvents()
        {
            var eventList = new List<ArenaEvent>();

            foreach (var arena in arenaLocations.Values)
            {
                if (arena.HasActiveEvent)
                {
                    eventList.Add(arena.ActiveEvent);
                }
            }

            return eventList;
        }

        public static bool IsActiveArenaPlayer(uint characterId)
        {
            bool isPlayerActive = false;

            foreach (var arena in arenaLocations.Values)
            {
                if (arena.HasActiveEvent && arena.ActiveEvent.Status >= 4)
                {
                    isPlayerActive = arena.ActiveEvent.Players.FirstOrDefault(x => x.CharacterId == characterId) != null;
                    if (isPlayerActive)
                        break;
                }
            }

            return isPlayerActive;
        }

        public static bool AddPlayerToQueue(uint characterId, string characterName, int? characterLevel, string eventType, uint monarchId, string monarchName, string playerIP, out string returnMsg)
        {
            returnMsg = string.Empty;

            if (queuedPlayers.ContainsKey(characterId))
            {
                returnMsg = $"You are actively queued for an arena event, you cannot queue twice";
                return false;
            }

            var existingArenaPlayer = ArenaManager.GetArenaPlayerByCharacterId(characterId);
            if (existingArenaPlayer != null)
            {
                returnMsg = $"You are currently in an active {existingArenaPlayer.EventTypeDisplay} arena event.  You must wait until your current event is over before queuing for another one.";
                 
                return false;
            }

            ArenaPlayer player = new ArenaPlayer();
            player.CharacterId = characterId;
            player.CharacterName = characterName;
            player.CharacterLevel = (uint)(characterLevel.HasValue ? characterLevel.Value : 0);
            player.EventType = eventType;
            player.MonarchId = monarchId;
            player.MonarchName = monarchName;
            player.CreateDateTime = DateTime.Now;
            player.PlayerIP = playerIP;

            queuedPlayers.Add(characterId, player);

            var queueCount = queuedPlayers.Values.Count(x => x.EventType.Equals(eventType));

            PlayerManager.BroadcastToAll(new GameMessageSystemChat($"A new player has queued for a{(eventType.ToLower().Equals("ffa") ? "n" : "")} {eventType} arena match. There {(queueCount > 1 ? "are" : "is")} currently {queueCount} player{(queueCount > 1 ? "s" : "")} queued for {eventType}", ChatMessageType.Broadcast));

            return true;
        }

        public static void ReQueuePlayer(ArenaPlayer player)
        {
            player.EventId = null;
            player.TeamGuid = null;
            if (!queuedPlayers.ContainsKey(player.CharacterId))
            {
                queuedPlayers.Add(player.CharacterId, player);
            }
        }

        public static ArenaEvent MatchMake(List<string> supportedEventTypes)
        {
            //log.Info($"ArenaManager.MatchMake()");

            return MatchMake(supportedEventTypes, new List<uint>());
        }

        /// <summary>
        /// Attempts to find a match for the first player in the queue for a given arena's supported event types
        /// If a match can't be found for that player's event type, will add the player to the excludedPlayers list and call the method again recursively
        /// If no players are found queued for the given event types, returns null
        /// </summary>
        /// <param name="supportedEventTypes"></param>
        /// <param name="excludedPlayers"></param>
        /// <returns></returns>
        public static ArenaEvent MatchMake(List<string> supportedEventTypes, List<uint> excludedPlayers)
        {
            if (excludedPlayers == null)
            {
                excludedPlayers = new List<uint>();
            }

            //log.Info($"ArenaManager.MatchMake() - excludedPlayers.Count = {excludedPlayers.Count}");

            //Trim out any players from the queue that aren't online, aren't pk status, or are pk tagged
            List<ArenaPlayer> queueCopy = new List<ArenaPlayer>();
            queueCopy.AddRange(queuedPlayers.Values);

            foreach (var arenaPlayer in queueCopy)
            {
                var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                bool isPlayerValidState = true;
                if (player != null)
                {
                    if (!player.IsPK)
                    {
                        isPlayerValidState = false;
                    }
                    else if (player.PKTimerActive)
                    {
                        isPlayerValidState = false;
                    }
                    else if (player.IsArenaObserver || player.IsPendingArenaObserver)
                    {
                        isPlayerValidState = false;
                    }
                }
                else
                {
                    isPlayerValidState = false;
                }

                if (!isPlayerValidState)
                {
                    //If player is not in a valid state, message them and remove them from the queue
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been removed from the arena queue because during match making you were found to be either watching another arena event, are not PK status or you are PK tagged.  Please join the queue again when you're in a valid state.", ChatMessageType.System));

                    queuedPlayers.Remove(arenaPlayer.CharacterId);
                }
            }

            //Find the first queued player waiting for an event type that this arena location supports
            var firstArenaPlayer = queuedPlayers.Values?
                .Where(x => supportedEventTypes.Contains(x.EventType.ToLower()) && !excludedPlayers.Contains(x.CharacterId))?
                .OrderBy(x => x.CreateDateTime)?
                .FirstOrDefault();

            if (firstArenaPlayer != null)
            {
                //log.Info($"ArenaManager.MatchMake() - First Player = {firstArenaPlayer.CharacterName}, EventType = {firstArenaPlayer.EventType}");

                //See if there's enough other players waiting for the same event type to create a match
                //must be within 50 levels
                var otherPlayers = queuedPlayers.Values?
                .Where(x =>
                        firstArenaPlayer.EventType.Equals(x.EventType) &&
                        x.CharacterId != firstArenaPlayer.CharacterId &&
                        x.CharacterLevel <= firstArenaPlayer.CharacterLevel + 75 &&
                        x.CharacterLevel >= firstArenaPlayer.CharacterLevel - 75 &&
                        (PropertyManager.GetBool("arena_allow_same_ip_match").Item || !firstArenaPlayer.PlayerIP.Equals(x.PlayerIP)))?
                .OrderBy(x => x.CreateDateTime);

                bool weHaveEnoughPlayers = false;
                List<ArenaPlayer> finalPlayerList = new List<ArenaPlayer>();

                if (otherPlayers != null && otherPlayers.Count() > 0)
                {
                    //log.Info($"ArenaManager.MatchMake() - otherPlayers.Count = {otherPlayers.Count()}");

                    switch (firstArenaPlayer.EventType.ToLower())
                    {
                        case "1v1":
                            weHaveEnoughPlayers = true;
                            finalPlayerList.Add(firstArenaPlayer);
                            finalPlayerList.Add(otherPlayers.First());
                            foreach (var player in finalPlayerList)
                            {
                                player.TeamGuid = Guid.NewGuid();
                            }
                            break;
                        case "2v2":
                            if (otherPlayers.Count() >= 3)
                            {
                                //Match up team 1 with other queued fellow or allegiance members as first priority
                                var foundFirstTeamMatch = false;

                                //Check to see if any other queued players are in same fellow as first player
                                var firstPlayer = PlayerManager.GetOnlinePlayer(firstArenaPlayer.CharacterId);
                                Fellowship firstPlayerFellowship = null;
                                if(firstPlayer != null)
                                {
                                    firstPlayerFellowship = firstPlayer.Fellowship;
                                }

                                if(firstPlayerFellowship != null)
                                {
                                    foreach(var otherArenaPlayer in otherPlayers)
                                    {
                                        var otherPlayer = PlayerManager.GetOnlinePlayer(otherArenaPlayer.CharacterId);
                                        if(otherPlayer != null && firstPlayerFellowship.FellowshipMembers.ContainsKey(otherPlayer.Guid.Full))
                                        {
                                            foundFirstTeamMatch = true;
                                            firstArenaPlayer.TeamGuid = Guid.NewGuid();
                                            otherArenaPlayer.TeamGuid = firstArenaPlayer.TeamGuid;
                                            finalPlayerList.Add(firstArenaPlayer);
                                            finalPlayerList.Add(otherArenaPlayer);
                                            break;
                                        }
                                    }
                                }

                                if (!foundFirstTeamMatch)
                                {
                                    //Check to see if any other queued players are in a allegiance as first player
                                    var sameAllegQueuedPlayers = otherPlayers.Where(x => x.MonarchId == firstArenaPlayer.MonarchId)?.OrderBy(x => x.CreateDateTime);
                                    if (sameAllegQueuedPlayers != null && sameAllegQueuedPlayers.Any())
                                    {
                                        foundFirstTeamMatch = true;
                                        var allegTeamMate = sameAllegQueuedPlayers.First();
                                        firstArenaPlayer.TeamGuid = Guid.NewGuid();
                                        allegTeamMate.TeamGuid = firstArenaPlayer.TeamGuid;
                                        finalPlayerList.Add(firstArenaPlayer);
                                        finalPlayerList.Add(allegTeamMate);
                                    }
                                }

                                //Match up team 2 with other queued fellow or allegiance members as first priority
                                var foundSecondTeamMatch = false;
                                var secondTeamCandidates = otherPlayers.Where(x => x.CharacterId != firstArenaPlayer.CharacterId && !x.TeamGuid.HasValue)?.OrderBy(x => x.CreateDateTime);
                                
                                foreach(var secondTeamCandidate in secondTeamCandidates)
                                {
                                    //Check to see if there's another queued player in the same fellow
                                    var secondTeamLeaderPlayer = PlayerManager.GetOnlinePlayer(secondTeamCandidate.CharacterId);
                                    Fellowship secondTeamLeaderFellowship = null;
                                    if (secondTeamLeaderPlayer != null)
                                    {
                                        secondTeamLeaderFellowship = secondTeamLeaderPlayer.Fellowship;
                                    }

                                    if (secondTeamLeaderFellowship != null)
                                    {
                                        foreach (var otherArenaPlayer in secondTeamCandidates.Where(x => x.CharacterId != secondTeamCandidate.CharacterId))
                                        {
                                            var otherPlayer = PlayerManager.GetOnlinePlayer(otherArenaPlayer.CharacterId);
                                            if (otherPlayer != null && secondTeamLeaderFellowship.FellowshipMembers.ContainsKey(otherPlayer.Guid.Full))
                                            {
                                                foundSecondTeamMatch = true;
                                                secondTeamCandidate.TeamGuid = Guid.NewGuid();
                                                otherArenaPlayer.TeamGuid = secondTeamCandidate.TeamGuid;
                                                finalPlayerList.Add(secondTeamCandidate);
                                                finalPlayerList.Add(otherArenaPlayer);
                                                break;
                                            }
                                        }
                                    }

                                    //Check to see if there's another queued player in the same allegiance
                                    if(!foundSecondTeamMatch)
                                    {
                                        var sameAllegQueuedPlayers = secondTeamCandidates.Where(x => x.CharacterId != secondTeamCandidate.CharacterId && x.MonarchId == secondTeamCandidate.MonarchId)?.OrderBy(x => x.CreateDateTime);
                                        if (sameAllegQueuedPlayers != null && sameAllegQueuedPlayers.Any())
                                        {
                                            foundSecondTeamMatch = true;
                                            var allegTeamMate = sameAllegQueuedPlayers.First();
                                            secondTeamCandidate.TeamGuid = Guid.NewGuid();
                                            allegTeamMate.TeamGuid = secondTeamCandidate.TeamGuid;
                                            finalPlayerList.Add(secondTeamCandidate);
                                            finalPlayerList.Add(allegTeamMate);
                                            break;
                                        }
                                    }
                                }

                                //If we didn't find a match for Team 1 based on fellowship or allegiance, assign a team mate at random
                                if (!foundFirstTeamMatch)
                                {
                                    firstArenaPlayer.TeamGuid = Guid.NewGuid();
                                    var firstPlayerTeamMate = otherPlayers.Where(x => x.CharacterId != firstArenaPlayer.CharacterId && !x.TeamGuid.HasValue).OrderBy(x => Guid.NewGuid()).First();
                                    firstPlayerTeamMate.TeamGuid = firstArenaPlayer.TeamGuid;
                                    finalPlayerList.Add(firstArenaPlayer);
                                    finalPlayerList.Add(firstPlayerTeamMate);
                                    foundFirstTeamMatch = true;
                                }

                                //If we didn't find a match for Team 2 based on fellowship or allegiance, assign a team at random
                                if (!foundSecondTeamMatch)
                                {
                                    var secondTeamPlayers = otherPlayers.Where(x => !x.TeamGuid.HasValue).Take(2);
                                    var secondTeamGuid = Guid.NewGuid();
                                    foreach(var secondTeamPlayer in secondTeamPlayers)
                                    {
                                        secondTeamPlayer.TeamGuid = secondTeamGuid;
                                        finalPlayerList.Add(secondTeamPlayer);
                                    }

                                    foundSecondTeamMatch = true;
                                }

                                if(foundFirstTeamMatch && foundSecondTeamMatch)
                                {
                                    weHaveEnoughPlayers = true;
                                }
                            }
                            break;
                        case "ffa":

                            if (otherPlayers.Count() >= 9 ||
                                (otherPlayers.Count() >= 6 && firstArenaPlayer.CreateDateTime < DateTime.Now.AddMinutes(-1)) ||
                                (otherPlayers.Count() >= 5 && firstArenaPlayer.CreateDateTime < DateTime.Now.AddMinutes(-2)) ||
                                (otherPlayers.Count() >= 4 && firstArenaPlayer.CreateDateTime < DateTime.Now.AddMinutes(-3)))
                            {
                                finalPlayerList.Add(firstArenaPlayer);

                                //if we have 10 total players, start the match, or if we have at least 5 total players after having waited for 3 minutes, start the match
                                //Don't allow more than 2 players from the same clan                                
                                foreach (var player in otherPlayers)
                                {
                                    if (finalPlayerList.Count(x => x.MonarchId == player.MonarchId) <= 1)
                                    {
                                        finalPlayerList.Add(player);
                                    }

                                    if (finalPlayerList.Count() >= 10)
                                        break;
                                }

                                if (finalPlayerList.Count() >= 10 ||
                                    (finalPlayerList.Count() >= 7 && firstArenaPlayer.CreateDateTime < DateTime.Now.AddMinutes(-1)) ||
                                    (finalPlayerList.Count() >= 6 && firstArenaPlayer.CreateDateTime < DateTime.Now.AddMinutes(-2)) ||
                                    (finalPlayerList.Count() >= 5 && firstArenaPlayer.CreateDateTime < DateTime.Now.AddMinutes(-3)))
                                {
                                    weHaveEnoughPlayers = true;
                                    foreach (var player in finalPlayerList)
                                    {
                                        player.TeamGuid = Guid.NewGuid();
                                    }
                                }
                            }

                            break;
                    }

                    if (weHaveEnoughPlayers)
                    {
                        log.Info($"ArenaManager.MatchMake() - we have enough players to start the match");

                        //Start an event
                        var arenaEvent = new ArenaEvent();
                        arenaEvent.EventType = firstArenaPlayer.EventType;
                        arenaEvent.Players = finalPlayerList;
                        arenaEvent.Status = 1;
                        arenaEvent.CreatedDateTime = DateTime.Now;

                        foreach (var player in finalPlayerList)
                        {
                            queuedPlayers.Remove(player.CharacterId);
                        }

                        return arenaEvent;
                    }
                    else
                    {
                        //log.Info($"ArenaManager.MatchMake() - not enough players, adding {firstArenaPlayer.CharacterName} to exclude list and calling MatchMake again");
                        //There's not enough players to make a match for the first queued player's event type,
                        //so mark that player excluded and try to matchmake with the next player in the queue
                        excludedPlayers.Add(firstArenaPlayer.CharacterId);
                        return MatchMake(supportedEventTypes, excludedPlayers);
                    }
                }
                else
                {
                    //log.Info($"ArenaManager.MatchMake() - not enough players, adding {firstArenaPlayer.CharacterName} to exclude list and calling MatchMake again");
                    //There's not enough players to make a match for the first queued player's event type,
                    //so mark that player excluded and try to matchmake with the next player in the queue
                    excludedPlayers.Add(firstArenaPlayer.CharacterId);
                    return MatchMake(supportedEventTypes, excludedPlayers);
                }
            }
            else
            {
                //There's no players in the queue that match a supported event type for this arena location
                //log.Info($"ArenaManager.MatchMake() - no queued players for supported event types");
                return null;
            }

            return null;
        }

        public static void CancelEvent(ArenaEvent arenaEvent)
        {
            log.Info($"ArenaManager.CancelEvent() - ArenaEventId = {arenaEvent.Id}, Location = {arenaEvent.Location}");

            try
            {
                arenaEvent.Status = -1;
                arenaEvent.EndDateTime = DateTime.Now;
                DatabaseManager.Log.SaveArenaEvent(arenaEvent);

                foreach (var arenaPlayer in arenaEvent.Players)
                {
                    var player = PlayerManager.GetOnlinePlayer(arenaPlayer.CharacterId);
                    if (player != null)
                    {
                        player.EnqueueBroadcast(new GameMessageSystemChat($"Your pending arena event has been cancelled.\nCancel Reason: {arenaEvent.CancelReason}", ChatMessageType.Broadcast));
                        if (player.IsPK && !player.PKTimerActive)
                        {
                            //For any player who is still online, pk and not tagged, put them back at the front of the queue
                            ReQueuePlayer(arenaPlayer);
                            player.EnqueueBroadcast(new GameMessageSystemChat("You have been added back to the front of the arena queue.", ChatMessageType.Broadcast));
                        }

                        //If player is in an arena, teleport player to their LS
                        if (player.CurrentLandblock?.IsArenaLandblock ?? false)
                        {
                            player.Teleport(player.Sanctuary);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in ArenaManager.CancelEvent. Ex: {ex}");
            }
        }

        public static void HandlePlayerDeath(uint victimId, uint killerId)
        {
            log.Info($"ArenaManager.HandlePlayerDeath victimId = {victimId}, killerId = {killerId}");
            ArenaPlayer victim = null;
            ArenaPlayer killer = null;
            ArenaLocation arenaLocation = null;
            foreach (var arena in arenaLocations.Values)
            {
                if (arena.HasActiveEvent)
                {
                    if (victim == null)
                    {
                        victim = arena.ActiveEvent.Players.FirstOrDefault(x => x.CharacterId == victimId);
                        killer = arena.ActiveEvent.Players.FirstOrDefault(x => x.CharacterId == killerId);
                        arenaLocation = arena;
                    }

                    if (victim != null && killer != null)
                        break;
                }
            }

            //In 1v1, 2v2 and FFA, dieing eliminates you from the event
            if (victim != null &&
                arenaLocation != null &&
                (arenaLocation.ActiveEvent.EventType.Equals("1v1")) ||
                (arenaLocation.ActiveEvent.EventType.Equals("1v1")) ||
                (arenaLocation.ActiveEvent.EventType.Equals("ffa")))
            {
                victim.IsEliminated = true;
                victim.TotalDeaths++;

                //Set the finish place                 
                var notEliminatedPlayers = arenaLocation.ActiveEvent.Players.Where(x => !x.IsEliminated);
                if(notEliminatedPlayers != null)
                {
                    victim.FinishPlace = notEliminatedPlayers.Count() + 1; //If there's 5 players still in the game after you just got eliminated, you're 6th place
                }

                var victimPlayer = PlayerManager.GetOnlinePlayer(victim.CharacterId);
                if(victimPlayer != null)
                {
                    victimPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You've been eliminated from an arena event by way of death.  You finished in {victim.FinishPlaceDisplay} place.  Stay online until the end of the match to receive rewards.", ChatMessageType.System));
                }
            }

            //TODO for future events dieing won't eliminate you
            //may have rules like 3rd death you're eliminated

            if (killer != null && !killer.IsEliminated)
            {
                killer.TotalKills++;
            }

            arenaLocation.CheckForArenaWinner(out Guid? winningTeamGuid);
            if (winningTeamGuid.HasValue)
            {
                arenaLocation.EndEventWithWinner(winningTeamGuid.Value);
            }
        }

        public static List<ArenaPlayer> GetQueuedPlayers()
        {
            return queuedPlayers.Values.ToList();
        }

        public static void PlayerCancel(uint characterId)
        {
            //Check if player is in an active event                    
            //if not, remove player from queue
            //if in active event
            //  if event isn't started yet cancel and add all other players back to queue
            //  if event is started, message that they can finish the event or recall to forfeit

            var player = PlayerManager.GetOnlinePlayer(characterId);

            var arenaPlayer = ArenaManager.GetArenaPlayerByCharacterId(characterId);

            if(arenaPlayer != null)
            {
                player?.EnqueueBroadcast(new GameMessageSystemChat("Your arena match is already started and cannot be cancelled.  To forfeit, you can leave the arena or log off.", ChatMessageType.Broadcast));
                return;
            }

            if (queuedPlayers.ContainsKey(characterId))
            {
                queuedPlayers.Remove(characterId);
                player?.EnqueueBroadcast(new GameMessageSystemChat("You have cancelled and been removed from the arena queue.", ChatMessageType.Broadcast));
            }

            //Arena Observers need to be able to cancel out of observer mode
            if(player?.IsArenaObserver ?? false)
            {
                ExitArenaObserverMode(player);
                var arenaLoc = arenaLocations.Values.FirstOrDefault(x => x.HasActiveEvent && (x.ActiveEvent.Observers?.Contains(player.Character.Id) ?? false));
                if(arenaLoc != null)
                {
                    arenaLoc.ActiveEvent.Observers.Remove(player.Character.Id);
                }
            }
        }


        public static ArenaPlayer GetArenaPlayerByCharacterId(uint characterId)
        {
            foreach (ArenaLocation loc in arenaLocations.Values)
            {
                if (loc.HasActiveEvent)
                {
                    foreach (var player in loc.ActiveEvent.Players)
                    {
                        if (player.CharacterId == characterId)
                        {
                            return player;
                        }
                    }
                }
            }

            return null;
        }

        public static ArenaEvent GetArenaEventByLandblock(uint landblockId)
        {
            if (arenaLocations.ContainsKey(landblockId))
            {
                if (arenaLocations[landblockId].HasActiveEvent)
                {
                    return arenaLocations[landblockId].ActiveEvent;
                }
            }

            return null;
        }

        public static string GetArenaNameByLandblock(uint landblockId)
        {
            if (arenaLocations.ContainsKey(landblockId))
            {
                return arenaLocations[landblockId].ArenaName;
            }

            return "";
        }

        public static bool IsValidEventType(string eventType)
        {
            switch (eventType.ToLower())
            {
                case "1v1":
                    return true;
                case "2v2":
                    return true;
                case "ffa":
                    return true;
                default:
                    return false;
            }
        }

        public static void ClearQueue(string eventType)
        {
            List<ArenaPlayer> playersToRemove = new List<ArenaPlayer>();

            foreach (var arenaPlayer in queuedPlayers.Values)
            {
                if(string.IsNullOrEmpty(eventType) || arenaPlayer.EventType.ToLower().Equals(eventType))
                {
                    playersToRemove.Add(arenaPlayer);
                }
            }

            foreach(var removedPlayer in playersToRemove)
            {
                queuedPlayers.Remove(removedPlayer.CharacterId);
                var player = PlayerManager.GetOnlinePlayer(removedPlayer.CharacterId);
                if(player != null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been removed from the arena queue because an admin had to reset the queue for your event type.  Sorry for the inconvenience.", ChatMessageType.System));
                }
            }
        }

        public static void ObserveEvent(Player player, int eventID)
        {
            //Verify the player is online and not PK tagged
            var onlinePlayer = PlayerManager.GetOnlinePlayer(player.Character.Id);
            if (onlinePlayer != null)
            {
                if (player.PKTimerActive)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have been prevented from observing an arena event because you are currently PK tagged.  Please wait until you are not PK tagged to join an event.", ChatMessageType.System));
                    return;
                }
            }
            else
            {
                return;
            }

            //Verify the player is not in an active event
            if (ArenaManager.GetArenaPlayerByCharacterId(player.Character.Id) != null)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You cannot watch an arena match when you are already a player in an active arena match.", ChatMessageType.System));
                return;
            }

            //Verify the player is not already observing another arena event
            if (player.IsArenaObserver)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You are already observing an arena event.  Type /arena cancel to leave the event.", ChatMessageType.System));
                return;
            }

            //Verify the event is not at its max capacity of observers


            //Verify the event for the given EventID is valid and active
            var arenaEvent = ArenaManager.GetActiveEvents()?.FirstOrDefault(x => x.Id == eventID);
            if(arenaEvent == null)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"There is no active arena match with EventID = {eventID}.", ChatMessageType.System));
                return;
            }

            //Add the player to the event's Observers list
            if (arenaEvent.Observers == null)
                arenaEvent.Observers = new List<uint>();

            arenaEvent.Observers.Add(player.Character.Id);

            //Change player properties to be frozen for 10 seconds, then invisible and gagged, then ported into the arena.
            EnterArenaObserverMode(player, arenaEvent);            
        }

        public static void EnterArenaObserverMode(Player player, ArenaEvent arenaEvent)
        {
            if (player == null || arenaEvent == null)
                return;

            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You are about to enter Arena Observer mode. You will be frozen in place for a bit before you are teleported to the arena.", ChatMessageType.System));
           
            var actionChain = new ActionChain();

            actionChain.AddAction(player, () =>
            {
                player.IsPendingArenaObserver = true;
                player.IsFrozen = true;
                player.EnqueueBroadcastPhysicsState();
            });
            actionChain.AddDelaySeconds(10);
            actionChain.AddAction(player, () =>
            {
                player.HandleCloak();
            });
            actionChain.AddDelaySeconds(.5);
            actionChain.AddAction(player, () =>
            {
                player.IsGagged = true;
                player.IsFrozen = false;
                player.Attackable = false;
                player.EnqueueBroadcastPhysicsState();
            });
            actionChain.AddDelaySeconds(.5);
            actionChain.AddAction(player, () =>
            {
                var startingPositions = ArenaLocation.GetArenaLocationStartingPositions(arenaEvent.Location);
                if (startingPositions != null)
                {
                    //Teleport to a random starting position
                    player.Teleport(startingPositions[new Random().Next(startingPositions.Count)]);                    
                }
            });
            actionChain.AddAction(player, () =>
            {
                player.IsPendingArenaObserver = false;
                player.IsArenaObserver = true;
                player.RecallsDisabled = true;
                player.HandleActionChangeCombatMode(CombatMode.NonCombat);
                player.EnqueueBroadcastPhysicsState();
            });
            actionChain.EnqueueChain();
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have entered Arena Observer mode. You can watch an arena match, but are not visible, cannot talk, and cannot interact with the world.\nTo exit use the command /arena cancel", ChatMessageType.System));                        
        }

        public static void ExitArenaObserverMode(Player player)
        {
            if (player == null)
                return;

            var actionChain = new ActionChain();

            actionChain.AddAction(player, () =>
            {
                player.IsFrozen = true;
                player.EnqueueBroadcastPhysicsState();
            });
            actionChain.AddDelaySeconds(3);
            actionChain.AddAction(player, () =>
            {
                player.Teleport(player.Sanctuary);
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You've exited observer mode for an arena match and are being teleported to your lifestone.", ChatMessageType.System));                
            });
            actionChain.AddDelaySeconds(0.5);
            actionChain.AddAction(player, () =>
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
            });
            
            actionChain.EnqueueChain();            
        }

        public static void DispelArenaRares(Player player)
        {
            if (player == null)
                return;

            if (player.HasArenaRareDmgBuff)
            {
                if (player.EnchantmentManager.HasSpell(5978))
                {
                    var enchantment = player.EnchantmentManager.GetEnchantment(5978);
                    if (enchantment != null)
                    {
                        player.EnchantmentManager.Dispel(enchantment);
                    }
                }

                player.HasArenaRareDmgBuff = false;
            }

            if (player.HasArenaRareDmgReductionBuff)
            {
                if (player.EnchantmentManager.HasSpell(5192))
                {
                    var enchantment = player.EnchantmentManager.GetEnchantment(5192);
                    if (enchantment != null)
                    {
                        player.EnchantmentManager.Dispel(enchantment);
                    }
                }

                player.HasArenaRareDmgReductionBuff = false;
            }
        }
    }
}
