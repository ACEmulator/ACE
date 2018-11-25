using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public static class PlayerManager
    {
        private static readonly ReaderWriterLockSlim playersLock = new ReaderWriterLockSlim();
        public static readonly Dictionary<ObjectGuid, Player> OnlinePlayers = new Dictionary<ObjectGuid, Player>();
        public static readonly Dictionary<ObjectGuid, OfflinePlayer> OfflinePlayers = new Dictionary<ObjectGuid, OfflinePlayer>();

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
                OfflinePlayers[offlinePlayer.Guid] = offlinePlayer;
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
                foreach (var player in OfflinePlayers.Values)
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
                OfflinePlayers[offlinePlayer.Guid] = offlinePlayer;
            }
            finally
            {
                playersLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// This will return null if the player wasn't found.
        /// </summary>
        public static OfflinePlayer GetOfflinePlayer(uint guid)
        {
            return GetOfflinePlayer(new ObjectGuid(guid));
        }

        /// <summary>
        /// This will return null if the player wasn't found.
        /// </summary>
        public static OfflinePlayer GetOfflinePlayer(ObjectGuid guid)
        {
            playersLock.EnterReadLock();
            try
            {
                if (OfflinePlayers.TryGetValue(guid, out var value))
                    return value;
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            return null;
        }

        public static List<OfflinePlayer> GetAllOffline()
        {
            var results = new List<OfflinePlayer>();

            playersLock.EnterReadLock();
            try
            {
                foreach (var player in OfflinePlayers.Values)
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
        public static Player GetOnlinePlayer(uint guid)
        {
            return GetOnlinePlayer(new ObjectGuid(guid));
        }

        /// <summary>
        /// This will return null if the player wasn't found.
        /// </summary>
        public static Player GetOnlinePlayer(ObjectGuid guid)
        {
            playersLock.EnterReadLock();
            try
            {
                if (OnlinePlayers.TryGetValue(guid, out var value))
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
            playersLock.EnterReadLock();
            try
            {
                var onlinePlayer = OnlinePlayers.Values.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

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
                foreach (var player in OnlinePlayers.Values)
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
        /// This will always be preceded by a call to GetOfflinePlayer()<para />
        /// </summary>
        public static bool SwitchPlayerFromOfflineToOnline(Player player)
        {
            playersLock.EnterWriteLock();
            try
            {
                if (!OfflinePlayers.Remove(player.Guid, out var offlinePlayer))
                    return false; // This should never happen

                if (offlinePlayer.ChangesDetected)
                    player.ChangesDetected = true;

                player.Allegiance = offlinePlayer.Allegiance;
                player.AllegianceNode = offlinePlayer.AllegianceNode;

                return OnlinePlayers.TryAdd(player.Guid, player);
            }
            finally
            {
                playersLock.ExitWriteLock();
            }
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
                if (!OnlinePlayers.Remove(player.Guid, out _))
                    return false; // This should never happen

                var offlinePlayer = new OfflinePlayer(player.Biota);

                offlinePlayer.Allegiance = player.Allegiance;
                offlinePlayer.AllegianceNode = player.AllegianceNode;

                return OfflinePlayers.TryAdd(offlinePlayer.Guid, offlinePlayer);
            }
            finally
            {
                playersLock.ExitWriteLock();
            }
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
                var onlinePlayer = OnlinePlayers.Values.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (onlinePlayer != null)
                {
                    isOnline = true;
                    return onlinePlayer;
                }

                isOnline = false;

                var offlinePlayer = OfflinePlayers.Values.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

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
        public static IPlayer FindByGuid(uint guid)
        {
            return FindByGuid(guid, out _);
        }

        /// <summary>
        /// This will return null of the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(uint guid, out bool isOnline)
        {
            return FindByGuid(new ObjectGuid(guid), out isOnline);
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
            playersLock.EnterReadLock();
            try
            {
                if (OnlinePlayers.TryGetValue(guid, out var onlinePlayer))
                {
                    isOnline = true;
                    return onlinePlayer;
                }

                isOnline = false;

                if (OfflinePlayers.TryGetValue(guid, out var offlinePlayer))
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
            IEnumerable<Player> onlinePlayers;
            IEnumerable<OfflinePlayer> offlinePlayers;

            playersLock.EnterReadLock();
            try
            {
                onlinePlayers = OnlinePlayers.Values.Where(p => p.Monarch == monarch.Full);
                offlinePlayers = OfflinePlayers.Values.Where(p => p.Monarch == monarch.Full);
            }
            finally
            {
                playersLock.ExitReadLock();
            }

            var results = new List<IPlayer>();
            results.AddRange(onlinePlayers);
            results.AddRange(offlinePlayers);

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
                foreach (var player in OnlinePlayers.Values)
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
    }
}
