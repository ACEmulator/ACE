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
using ACE.Server.Entity;
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
            List<ArenaPlayer> queue = new List<ArenaPlayer>();
            queue.AddRange(queuedPlayers.Values);

            foreach (var arenaPlayer in queue)
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
                }
                else
                {
                    isPlayerValidState = false;
                }

                if (!isPlayerValidState)
                {
                    //If player is not in a valid state, message them and remove them from the queue
                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} - You have been removed from the arena queue because during match making you were found to be either not PK status or you are PK tagged.  Please join the queue again when you're in a valid state.", ChatMessageType.System));

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
                        x.CharacterLevel <= firstArenaPlayer.CharacterLevel + 50 &&
                        x.CharacterLevel >= firstArenaPlayer.CharacterLevel - 50 &&
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
                                weHaveEnoughPlayers = true;
                                finalPlayerList.Add(firstArenaPlayer);
                                finalPlayerList.AddRange(otherPlayers.Take(3));

                                firstArenaPlayer.TeamGuid = Guid.NewGuid();
                                var firstPlayerTeamMate = finalPlayerList.Where(x => x.CharacterId != firstArenaPlayer.CharacterId).OrderBy(x => Guid.NewGuid()).First();
                                firstPlayerTeamMate.TeamGuid = firstArenaPlayer.TeamGuid;

                                var secondTeam = finalPlayerList.Where(x => !x.TeamGuid.HasValue);
                                var secondTeamGuid = Guid.NewGuid();
                                foreach (var secondTeamPlayer in secondTeam)
                                {
                                    secondTeamPlayer.TeamGuid = secondTeamGuid;
                                }
                            }
                            break;
                        case "ffa":

                            if (otherPlayers.Count() >= 9 || (otherPlayers.Count() >= 6 && firstArenaPlayer.CreateDateTime > DateTime.Now.AddMinutes(-3)))
                            {
                                finalPlayerList.Add(firstArenaPlayer);

                                //if we have 10 total players, start the match, or if we have at least 7 total players after having waited for 3 minutes, start the match
                                //Don't allow more than 3 players from the same clan                                
                                foreach (var player in otherPlayers)
                                {
                                    if (finalPlayerList.Count(x => x.MonarchId == player.MonarchId) <= 1)
                                    {
                                        finalPlayerList.Add(player);
                                    }

                                    if (finalPlayerList.Count() >= 10)
                                        break;
                                }

                                if (finalPlayerList.Count() >= 10 || (finalPlayerList.Count() >= 7 && firstArenaPlayer.CreateDateTime > DateTime.Now.AddMinutes(-3)))
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
                        arenaLocation = arena;
                    }

                    if (killer == null)
                    {
                        killer = arena.ActiveEvent.Players.FirstOrDefault(x => x.CharacterId == killerId);
                    }
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

                //Set the finish place                 
                var notEliminatedPlayers = arenaLocation.ActiveEvent.Players.Where(x => !x.IsEliminated);
                if(notEliminatedPlayers != null)
                {
                    victim.FinishPlace = notEliminatedPlayers.Count() + 1; //If there's 5 players still in the game after you just got eliminated, you're 6th place
                }

                var victimPlayer = PlayerManager.GetOnlinePlayer(victim.CharacterId);
                if(victimPlayer != null)
                {
                    victimPlayer.Session.Network.EnqueueSend(new GameMessageSystemChat($"You've been eliminated from an arena event by way of death.  Your finish place is {victim.FinishPlace}.  Stay online until the end of the match to receive rewards.", ChatMessageType.System));
                }
            }

            //TODO for future events dieing won't eliminate you
            //may have rules like 3rd death you're eliminated

            if (victim != null && !victim.IsEliminated)
            {
                victim.TotalDeaths++;
            }

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
                player.EnqueueBroadcast(new GameMessageSystemChat("Your arena match is already started and cannot be cancelled.  To forfeit, you can leave the arena or log off.", ChatMessageType.Broadcast));
                return;
            }

            if (queuedPlayers.ContainsKey(characterId))
            {
                queuedPlayers.Remove(characterId);
                player?.EnqueueBroadcast(new GameMessageSystemChat("You have cancelled and been removed from the arena queue.", ChatMessageType.Broadcast));
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
    }
}
