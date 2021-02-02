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

namespace ACE.Server.Managers
{
    public static class PlayerManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ReaderWriterLockSlim playersLock = new ReaderWriterLockSlim();
        private static readonly Dictionary<uint, Player> onlinePlayers = new Dictionary<uint, Player>();
        private static readonly Dictionary<uint, OfflinePlayer> offlinePlayers = new Dictionary<uint, OfflinePlayer>();

        /// <summary>
        /// OfflinePlayers will be saved to the database every 1 hour
        /// </summary>
        private static readonly TimeSpan databaseSaveInterval = TimeSpan.FromHours(1);

        private static DateTime lastDatabaseSave = DateTime.MinValue;

        /// <summary>
        /// This will load all the players from the database into the OfflinePlayers dictionary. It should be called before WorldManager is initialized.
        /// </summary>
        public static void Initialize()
        {
            var results = DatabaseManager.Shard.BaseDatabase.GetAllPlayerBiotasInParallel();

            Parallel.ForEach(results, ConfigManager.Config.Server.Threading.DatabaseParallelOptions, result =>
            {
                var offlinePlayer = new OfflinePlayer(result);

                lock (offlinePlayers)
                    offlinePlayers[offlinePlayer.Guid.Full] = offlinePlayer;
            });
        }

        private static readonly LinkedList<Player> playersPendingLogoff = new LinkedList<Player>();

        public static void AddPlayerToLogoffQueue(Player player)
        {
            if (!playersPendingLogoff.Contains(player))
                playersPendingLogoff.AddLast(player);
        }

        public static void Tick()
        {
            // Database Save
            if (lastDatabaseSave + databaseSaveInterval <= DateTime.UtcNow)
                SaveOfflinePlayersWithChanges();

            var currentUnixTime = Time.GetUnixTime();

            while (playersPendingLogoff.Count > 0)
            {
                var first = playersPendingLogoff.First.Value;

                if (first.LogoffTimestamp <= currentUnixTime)
                {
                    playersPendingLogoff.RemoveFirst();
                    first.LogOut_Inner();
                    first.Session.logOffRequestTime = DateTime.UtcNow;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// This will save any player in the OfflinePlayers dictionary that has ChangesDetected. The biotas are saved in parallel.
        /// </summary>
        public static void SaveOfflinePlayersWithChanges()
        {
            lastDatabaseSave = DateTime.UtcNow;

            var biotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();

            playersLock.EnterReadLock();
            try
            {
                foreach (var player in offlinePlayers.Values)
                {
                    if (player.ChangesDetected)
                    {
                        player.SaveBiotaToDatabase(false);
                        biotas.Add((player.Biota, player.BiotaDatabaseLock));
                    }
                }
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            DatabaseManager.Shard.SaveBiotasInParallel(biotas, result => { });
        }
        

        /// <summary>
        /// This would be used when a new player is created after the server has started.
        /// When a new Player is created, they're created in an offline state, and then set to online shortly after as the login sequence continues.
        /// </summary>
        public static void AddOfflinePlayer(Player player)
        {
            playersLock.EnterWriteLock();
            try
            {
                var offlinePlayer = new OfflinePlayer(player.Biota);
                offlinePlayers[offlinePlayer.Guid.Full] = offlinePlayer;
            }
            finally
            {
                playersLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// This will return null if the player wasn't found.
        /// </summary>
        public static OfflinePlayer GetOfflinePlayer(ObjectGuid guid)
        {
            return GetOfflinePlayer(guid.Full);
        }

        /// <summary>
        /// This will return null if the player wasn't found.
        /// </summary>
        public static OfflinePlayer GetOfflinePlayer(uint guid)
        {
            playersLock.EnterReadLock();
            try
            {
                if (offlinePlayers.TryGetValue(guid, out var value))
                    return value;
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return null;
        }

        /// <summary>
        /// This will return null of the name was not found.
        /// </summary>
        public static OfflinePlayer GetOfflinePlayer(string name)
        {
            var admin = "+" + name;

            playersLock.EnterReadLock();
            try
            {
                var offlinePlayer = offlinePlayers.Values.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) || p.Name.Equals(admin, StringComparison.OrdinalIgnoreCase));

                if (offlinePlayer != null)
                    return offlinePlayer;
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return null;
        }

        public static List<IPlayer> GetAllPlayers()
        {
            var offlinePlayers = GetAllOffline();
            var onlinePlayers = GetAllOnline();

            var allPlayers = new List<IPlayer>();

            allPlayers.AddRange(offlinePlayers);
            allPlayers.AddRange(onlinePlayers);

            return allPlayers;
        }

        public static int GetOfflineCount()
        {
            playersLock.EnterReadLock();
            try
            {
                return offlinePlayers.Count;
            }
            finally
            {
                playersLock.ExitReadLock();
            }
        }

        public static List<OfflinePlayer> GetAllOffline()
        {
            var results = new List<OfflinePlayer>();

            playersLock.EnterReadLock();
            try
            {
                foreach (var player in offlinePlayers.Values)
                    results.Add(player);
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return results;
        }

        public static int GetOnlineCount()
        {
            playersLock.EnterReadLock();
            try
            {
                return onlinePlayers.Count;
            }
            finally
            {
                playersLock.ExitReadLock();
            }
        }

        /// <summary>
        /// This will return null if the player wasn't found.
        /// </summary>
        public static Player GetOnlinePlayer(ObjectGuid guid)
        {
            return GetOnlinePlayer(guid.Full);
        }

        /// <summary>
        /// This will return null if the player wasn't found.
        /// </summary>
        public static Player GetOnlinePlayer(uint guid)
        {
            playersLock.EnterReadLock();
            try
            {
                if (onlinePlayers.TryGetValue(guid, out var value))
                    return value;
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return null;
        }

        /// <summary>
        /// This will return null of the name was not found.
        /// </summary>
        public static Player GetOnlinePlayer(string name)
        {
            var admin = "+" + name;

            playersLock.EnterReadLock();
            try
            {
                var onlinePlayer = onlinePlayers.Values.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) || p.Name.Equals(admin, StringComparison.OrdinalIgnoreCase));

                if (onlinePlayer != null)
                    return onlinePlayer;
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return null;
        }

        public static List<Player> GetAllOnline()
        {
            var results = new List<Player>();

            playersLock.EnterReadLock();
            try
            {
                foreach (var player in onlinePlayers.Values)
                    results.Add(player);
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return results;
        }


        /// <summary>
        /// This will return true if the player was successfully added.
        /// It will return false if the player was not found in the OfflinePlayers dictionary (which should never happen), or player already exists in the OnlinePlayers dictionary (which should never happen).
        /// This will always be preceded by a call to GetOfflinePlayer()
        /// </summary>
        public static bool SwitchPlayerFromOfflineToOnline(Player player)
        {
            playersLock.EnterWriteLock();
            try
            {
                if (!offlinePlayers.Remove(player.Guid.Full, out var offlinePlayer))
                    return false; // This should never happen

                if (offlinePlayer.ChangesDetected)
                    player.ChangesDetected = true;

                player.Allegiance = offlinePlayer.Allegiance;
                player.AllegianceNode = offlinePlayer.AllegianceNode;

                if (!onlinePlayers.TryAdd(player.Guid.Full, player))
                    return false;
            }
            finally
            {
                playersLock.ExitWriteLock();
            }

            AllegianceManager.LoadPlayer(player);

            player.SendFriendStatusUpdates();

            return true;
        }

        /// <summary>
        /// This will return true if the player was successfully added.
        /// It will return false if the player was not found in the OnlinePlayers dictionary (which should never happen), or player already exists in the OfflinePlayers dictionary (which should never happen).
        /// </summary>
        public static bool SwitchPlayerFromOnlineToOffline(Player player)
        {
            playersLock.EnterWriteLock();
            try
            {
                if (!onlinePlayers.Remove(player.Guid.Full, out _))
                    return false; // This should never happen

                var offlinePlayer = new OfflinePlayer(player.Biota);

                offlinePlayer.Allegiance = player.Allegiance;
                offlinePlayer.AllegianceNode = player.AllegianceNode;

                if (!offlinePlayers.TryAdd(offlinePlayer.Guid.Full, offlinePlayer))
                    return false;
            }
            finally
            {
                playersLock.ExitWriteLock();
            }

            player.SendFriendStatusUpdates(false);
            player.HandleAllegianceOnLogout();

            return true;
        }

        /// <summary>
        /// Called when a character is initially deleted on the character select screen
        /// </summary>
        public static void HandlePlayerDelete(uint characterGuid)
        {
            AllegianceManager.HandlePlayerDelete(characterGuid);

            HouseManager.HandlePlayerDelete(characterGuid);
        }

        /// <summary>
        /// This will return true if the player was successfully found and removed from the OfflinePlayers dictionary.
        /// It will return false if the player was not found in the OfflinePlayers dictionary (which should never happen).
        /// </summary>
        public static bool ProcessDeletedPlayer(uint guid)
        {
            playersLock.EnterWriteLock();
            try
            {
                if (!offlinePlayers.Remove(guid, out var offlinePlayer))
                    return false; // This should never happen
            }
            finally
            {
                playersLock.ExitWriteLock();
            }

            return true;
        }


        /// <summary>
        /// This will return null if the name was not found.
        /// </summary>
        public static IPlayer FindByName(string name)
        {
            return FindByName(name, out _);
        }

        /// <summary>
        /// This will return null if the name was not found.
        /// </summary>
        public static IPlayer FindByName(string name, out bool isOnline)
        {
            playersLock.EnterReadLock();
            try
            {
                var onlinePlayer = onlinePlayers.Values.FirstOrDefault(p => p.Name.TrimStart('+').Equals(name.TrimStart('+'), StringComparison.OrdinalIgnoreCase));

                if (onlinePlayer != null)
                {
                    isOnline = true;
                    return onlinePlayer;
                }

                isOnline = false;

                var offlinePlayer = offlinePlayers.Values.FirstOrDefault(p => p.Name.TrimStart('+').Equals(name.TrimStart('+'), StringComparison.OrdinalIgnoreCase) && !p.IsPendingDeletion);

                if (offlinePlayer != null)
                    return offlinePlayer;
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return null;
        }

        /// <summary>
        /// This will return null if the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(ObjectGuid guid)
        {
            return FindByGuid(guid, out _);
        }

        /// <summary>
        /// This will return null if the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(ObjectGuid guid, out bool isOnline)
        {
            return FindByGuid(guid.Full, out isOnline);
        }

        /// <summary>
        /// This will return null if the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(uint guid)
        {
            return FindByGuid(guid, out _);
        }

        /// <summary>
        /// This will return null if the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(uint guid, out bool isOnline)
        {
            playersLock.EnterReadLock();
            try
            {
                if (onlinePlayers.TryGetValue(guid, out var onlinePlayer))
                {
                    isOnline = true;
                    return onlinePlayer;
                }

                isOnline = false;

                if (offlinePlayers.TryGetValue(guid, out var offlinePlayer))
                    return offlinePlayer;
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return null;
        }


        /// <summary>
        /// Returns a list of all players who are under a monarch
        /// </summary>
        /// <param name="monarch">The monarch of an allegiance</param>
        public static List<IPlayer> FindAllByMonarch(ObjectGuid monarch)
        {
            var results = new List<IPlayer>();

            playersLock.EnterReadLock();
            try
            {
                var onlinePlayersResult = onlinePlayers.Values.Where(p => p.MonarchId == monarch.Full);
                var offlinePlayersResult = offlinePlayers.Values.Where(p => p.MonarchId == monarch.Full);

                results.AddRange(onlinePlayersResult);
                results.AddRange(offlinePlayersResult);
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return results;
        }


        /// <summary>
        /// This will return a list of Players that have this guid as a friend.
        /// </summary>
        public static List<Player> GetOnlineInverseFriends(ObjectGuid guid)
        {
            var results = new List<Player>();

            playersLock.EnterReadLock();
            try
            {
                foreach (var player in onlinePlayers.Values)
                {
                    if (player.Character.HasAsFriend(guid.Full, player.CharacterDatabaseLock))
                        results.Add(player);
                }
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return results;
        }


        /// <summary>
        /// Broadcasts GameMessage to all online sessions.
        /// </summary>
        public static void BroadcastToAll(GameMessage msg)
        {
            foreach (var player in GetAllOnline())
                player.Session.Network.EnqueueSend(msg);
        }

        public static void BroadcastToAuditChannel(Player issuer, string message)
        {
            if (issuer != null)
                BroadcastToChannel(Channel.Audit, issuer, message, true, true);
            else
                BroadcastToChannelFromConsole(Channel.Audit, message);

            //if (PropertyManager.GetBool("log_audit", true).Item)
                //log.Info($"[AUDIT] {(issuer != null ? $"{issuer.Name} says on the Audit channel: " : "")}{message}");

            //LogBroadcastChat(Channel.Audit, issuer, message);
        }

        public static void BroadcastToChannel(Channel channel, Player sender, string message, bool ignoreSquelch = false, bool ignoreActive = false)
        {
            if ((sender.ChannelsActive.HasValue && sender.ChannelsActive.Value.HasFlag(channel)) || ignoreActive)
            {
                foreach (var player in GetAllOnline().Where(p => (p.ChannelsActive ?? 0).HasFlag(channel)))
                {
                    if (!player.SquelchManager.Squelches.Contains(sender) || ignoreSquelch)
                        player.Session.Network.EnqueueSend(new GameEventChannelBroadcast(player.Session, channel, sender.Guid == player.Guid ? "" : sender.Name, message));
                }

                LogBroadcastChat(channel, sender, message);
            }
        }

        public static void LogBroadcastChat(Channel channel, WorldObject sender, string message)
        {
            switch (channel)
            {
                case Channel.Abuse:
                    if (!PropertyManager.GetBool("chat_log_abuse").Item)
                        return;
                    break;
                case Channel.Admin:
                    if (!PropertyManager.GetBool("chat_log_admin").Item)
                        return;
                    break;
                case Channel.AllBroadcast: // using this to sub in for a WorldBroadcast channel which isn't technically a channel
                    if (!PropertyManager.GetBool("chat_log_global").Item)
                        return;
                    break;
                case Channel.Audit:
                    if (!PropertyManager.GetBool("chat_log_audit").Item)
                        return;
                    break;
                case Channel.Advocate1:
                case Channel.Advocate2:
                case Channel.Advocate3:
                    if (!PropertyManager.GetBool("chat_log_advocate").Item)
                        return;
                    break;
                case Channel.Debug:
                    if (!PropertyManager.GetBool("chat_log_debug").Item)
                        return;
                    break;
                case Channel.Fellow:
                case Channel.FellowBroadcast:
                    if (!PropertyManager.GetBool("chat_log_fellow").Item)
                        return;
                    break;
                case Channel.Help:
                    if (!PropertyManager.GetBool("chat_log_help").Item)
                        return;
                    break;
                case Channel.Olthoi:
                    if (!PropertyManager.GetBool("chat_log_olthoi").Item)
                        return;
                    break;
                case Channel.QA1:
                case Channel.QA2:
                    if (!PropertyManager.GetBool("chat_log_qa").Item)
                        return;
                    break;
                case Channel.Sentinel:
                    if (!PropertyManager.GetBool("chat_log_sentinel").Item)
                        return;
                    break;

                case Channel.SocietyCelHanBroadcast:
                case Channel.SocietyEldWebBroadcast:
                case Channel.SocietyRadBloBroadcast:
                    if (!PropertyManager.GetBool("chat_log_society").Item)
                        return;
                    break;

                case Channel.AllegianceBroadcast:
                case Channel.CoVassals:
                case Channel.Monarch:
                case Channel.Patron:
                case Channel.Vassals:
                    if (!PropertyManager.GetBool("chat_log_allegiance").Item)
                        return;
                    break;

                case Channel.AlArqas:
                case Channel.Holtburg:
                case Channel.Lytelthorpe:
                case Channel.Nanto:
                case Channel.Rithwic:
                case Channel.Samsur:
                case Channel.Shoushi:
                case Channel.Yanshi:
                case Channel.Yaraq:
                    if (!PropertyManager.GetBool("chat_log_townchans").Item)
                        return;
                    break;

                default:
                    return;
            }

            if (channel != Channel.AllBroadcast)
                log.Info($"[CHAT][{channel.ToString().ToUpper()}] {(sender != null ? sender.Name : "[SYSTEM]")} says on the {channel} channel, \"{message}\"");
            else
                log.Info($"[CHAT][GLOBAL] {(sender != null ? sender.Name : "[SYSTEM]")} issued a world broadcast, \"{message}\"");
        }

        public static void BroadcastToChannelFromConsole(Channel channel, string message)
        {
            foreach (var player in GetAllOnline().Where(p => (p.ChannelsActive ?? 0).HasFlag(channel)))
                player.Session.Network.EnqueueSend(new GameEventChannelBroadcast(player.Session, channel, "CONSOLE", message));

            LogBroadcastChat(channel, null, message);
        }

        public static void BroadcastToChannelFromEmote(Channel channel, string message)
        {
            foreach (var player in GetAllOnline().Where(p => (p.ChannelsActive ?? 0).HasFlag(channel)))
                player.Session.Network.EnqueueSend(new GameEventChannelBroadcast(player.Session, channel, "EMOTE", message));
        }

        public static bool GagPlayer(Player issuer, string playerName)
        {
            var player = FindByName(playerName);

            if (player == null)
                return false;

            player.SetProperty(ACE.Entity.Enum.Properties.PropertyBool.IsGagged, true);
            player.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagTimestamp, Common.Time.GetUnixTime());
            player.SetProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagDuration, 300);

            player.SaveBiotaToDatabase();

            BroadcastToAuditChannel(issuer, $"{issuer.Name} has gagged {player.Name} for five minutes.");

            return true;
        }

        public static bool UnGagPlayer(Player issuer, string playerName)
        {
            var player = FindByName(playerName);

            if (player == null)
                return false;

            player.RemoveProperty(ACE.Entity.Enum.Properties.PropertyBool.IsGagged);
            player.RemoveProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagTimestamp);
            player.RemoveProperty(ACE.Entity.Enum.Properties.PropertyFloat.GagDuration);

            player.SaveBiotaToDatabase();

            BroadcastToAuditChannel(issuer, $"{issuer.Name} has ungagged {player.Name}.");

            return true;
        }

        public static void BootAllPlayers()
        {
            foreach (var player in GetAllOnline().Where(p => p.Session.AccessLevel < AccessLevel.Advocate))
                player.Session.Terminate(SessionTerminationReason.WorldClosed, new GameMessageBootAccount(" because the world is now closed"), null, "The world is now closed");
        }

        public static void UpdatePKStatusForAllPlayers(string worldType, bool enabled)
        {
            switch (worldType)
            {
                case "pk_server":
                    if (enabled)
                    {
                        foreach (var player in GetAllOnline())
                            player.SetPlayerKillerStatus(PlayerKillerStatus.PK, true);

                        foreach (var player in GetAllOffline())
                        {
                            player.SetProperty(PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.NPK);
                            player.SetProperty(PropertyFloat.MinimumTimeSincePk, 0);
                        }

                        var msg = $"This world has been changed to a Player Killer world. All players will become Player Killers in {PropertyManager.GetDouble("pk_respite_timer").Item} seconds.";
                        BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
                        LogBroadcastChat(Channel.AllBroadcast, null, msg);
                    }
                    else
                    {
                        foreach (var player in GetAllOnline())
                            player.SetPlayerKillerStatus(PlayerKillerStatus.NPK, true);

                        foreach (var player in GetAllOffline())
                        {
                            player.SetProperty(PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.NPK);
                            player.SetProperty(PropertyFloat.MinimumTimeSincePk, 0);
                        }

                        var msg = "This world has been changed to a Non Player Killer world. All players are now Non-Player Killers.";
                        BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
                        LogBroadcastChat(Channel.AllBroadcast, null, msg);
                    }
                    break;
                case "pkl_server":
                    if (PropertyManager.GetBool("pk_server").Item)
                        return;
                    if (enabled)
                    {
                        foreach (var player in GetAllOnline())
                            player.SetPlayerKillerStatus(PlayerKillerStatus.PKLite, true);

                        foreach (var player in GetAllOffline())
                        {
                            player.SetProperty(PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.NPK);
                            player.SetProperty(PropertyFloat.MinimumTimeSincePk, 0);
                        }

                        var msg = $"This world has been changed to a Player Killer Lite world. All players will become Player Killer Lites in {PropertyManager.GetDouble("pk_respite_timer").Item} seconds.";
                        BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
                        LogBroadcastChat(Channel.AllBroadcast, null, msg);
                    }
                    else
                    {
                        foreach (var player in GetAllOnline())
                            player.SetPlayerKillerStatus(PlayerKillerStatus.NPK, true);

                        foreach (var player in GetAllOffline())
                        {
                            player.SetProperty(PropertyInt.PlayerKillerStatus, (int)PlayerKillerStatus.NPK);
                            player.SetProperty(PropertyFloat.MinimumTimeSincePk, 0);
                        }

                        var msg = "This world has been changed to a Non Player Killer world. All players are now Non-Player Killers.";
                        BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.WorldBroadcast));
                        LogBroadcastChat(Channel.AllBroadcast, null, msg);
                    }
                    break;
            }
        }
    }
}
