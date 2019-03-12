using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Server.Entity;
using ACE.Server.Network.GameMessages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public static class PlayerManager
    {
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
            var results = DatabaseManager.Shard.GetAllPlayerBiotasInParallel();

            foreach (var result in results)
            {
                var offlinePlayer = new OfflinePlayer(result);
                offlinePlayers[offlinePlayer.Guid.Full] = offlinePlayer;
            }
        }

        public static void Tick()
        {
            // Database Save
            if (lastDatabaseSave + databaseSaveInterval <= DateTime.UtcNow)
                SaveOfflinePlayersWithChanges();
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
            AddOfflinePlayer(player.Biota);
        }

        /// <summary>
        /// This would be used when a new player is created after the server has started.
        /// When a new Player is created, they're created in an offline state, and then set to online shortly after as the login sequence continues.
        /// </summary>
        public static void AddOfflinePlayer(Biota playerBiota)
        {
            playersLock.EnterWriteLock();
            try
            {
                var offlinePlayer = new OfflinePlayer(playerBiota);
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
            return GetAllOffline().Count;
        }

        public static int GetOnlineCount()
        {
            return GetAllOnline().Count;
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

                var offlinePlayer = offlinePlayers.Values.FirstOrDefault(p => p.Name.TrimStart('+').Equals(name.TrimStart('+'), StringComparison.OrdinalIgnoreCase));

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
        /// This will return null of the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(ObjectGuid guid)
        {
            return FindByGuid(guid, out _);
        }

        /// <summary>
        /// This will return null of the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(ObjectGuid guid, out bool isOnline)
        {
            return FindByGuid(guid.Full, out isOnline);
        }

        /// <summary>
        /// This will return null of the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(uint guid)
        {
            return FindByGuid(guid, out _);
        }

        /// <summary>
        /// This will return null of the guid was not found.
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
            IEnumerable<Player> onlinePlayersResult;
            IEnumerable<OfflinePlayer> offlinePlayersResult;

            playersLock.EnterReadLock();
            try
            {
                onlinePlayersResult = onlinePlayers.Values.Where(p => p.MonarchId == monarch.Full);
                offlinePlayersResult = offlinePlayers.Values.Where(p => p.MonarchId == monarch.Full);
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            var results = new List<IPlayer>();
            results.AddRange(onlinePlayersResult);
            results.AddRange(offlinePlayersResult);

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
    }
}
