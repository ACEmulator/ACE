using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Entity;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public static class PlayerManager
    {
        public static readonly ConcurrentDictionary<ObjectGuid, Player> OnlinePlayers = new ConcurrentDictionary<ObjectGuid, Player>();

        public static readonly ConcurrentDictionary<ObjectGuid, OfflinePlayer> OfflinePlayers = new ConcurrentDictionary<ObjectGuid, OfflinePlayer>();

        public static void Initialize()
        {
            LoadAllPlayersAsOffline();
        }

        private static void LoadAllPlayersAsOffline()
        {
            var results = DatabaseManager.Shard.GetAllPlayerBiotasInParallel();

            foreach (var result in results)
            {
                var offlinePlayer = new OfflinePlayer(result);
                OfflinePlayers[offlinePlayer.Guid] = offlinePlayer;
            }
        }


        /// <summary>
        /// This would be used when a new player is created after the server has started.
        /// When a new Player is created, they're created in an offline state, and then set to online shortly after as the login sequence continues.
        /// </summary>
        public static void AddOfflinePlayer(Player player)
        {
            var offlinePlayer = new OfflinePlayer(player.Biota);
            OfflinePlayers[offlinePlayer.Guid] = offlinePlayer;
        }


        /// <summary>
        /// This will return null if the name was not found.
        /// </summary>
        public static IPlayer FindByName(string name)
        {
            var onlinePlayer = OnlinePlayers.Values.FirstOrDefault(p => p.Name.Equals(name));

            if (onlinePlayer != null)
                return onlinePlayer;

            var offlinePlayer = OfflinePlayers.Values.FirstOrDefault(p => p.Name.Equals(name));

            if (offlinePlayer != null)
                return offlinePlayer;

            return null;
        }

        /// <summary>
        /// This will return null of the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(uint guid)
        {
            return FindByGuid(new ObjectGuid(guid));
        }

        /// <summary>
        /// This will return null of the guid was not found.
        /// </summary>
        public static IPlayer FindByGuid(ObjectGuid guid)
        {
            if (OnlinePlayers.TryGetValue(guid, out var onlinePlayer))
                return onlinePlayer;

            if (OfflinePlayers.TryGetValue(guid, out var offlinePlayer))
                return offlinePlayer;

            return null;
        }






        [Obsolete]
        // probably bugged when players are added/removed...
        public static readonly List<Player> AllPlayers = new List<Player>();

        /// <summary>
        /// Returns a list of all players who are under a monarch
        /// </summary>
        /// <param name="monarch">The monarch of an allegiance</param>
        [Obsolete]
        public static List<Player> GetAllegiance(ObjectGuid monarch)
        {
            return AllPlayers.Where(p => p.Monarch == monarch.Full).ToList();
        }

        /// <summary>
        /// Returns an offline player record from the AllPlayers list
        /// </summary>
        /// <param name="playerGuid"></param>
        [Obsolete]
        public static Player GetOfflinePlayer(ObjectGuid playerGuid)
        {
            return AllPlayers.FirstOrDefault(p => p.Guid.Equals(playerGuid));
        }

        /// <summary>
        /// Syncs the cached offline player fields
        /// </summary>
        /// <param name="player">An online player</param>
        [Obsolete]
        public static void SyncOffline(Player player)
        {
            var offlinePlayer = AllPlayers.FirstOrDefault(p => p.Guid.Full == player.Guid.Full);
            if (offlinePlayer == null) return;

            // FIXME: this is a placeholder for offline players
            offlinePlayer.Monarch = player.Monarch;
            offlinePlayer.Patron = player.Patron;

            offlinePlayer.AllegianceCPPool = player.AllegianceCPPool;
        }

        /// <summary>
        /// Syncs an online player with the cached offline fields
        /// </summary>
        /// <param name="player">An online player</param>
        [Obsolete]
        public static void SyncOnline(Player player)
        {
            var offlinePlayer = AllPlayers.FirstOrDefault(p => p.Guid.Full == player.Guid.Full);
            if (offlinePlayer == null) return;

            // FIXME: this is a placeholder for offline players
            player.AllegianceCPPool = offlinePlayer.AllegianceCPPool;
        }

        [Obsolete]
        public static Player GetOfflinePlayerByGuidId(uint playerId)
        {
            return AllPlayers.FirstOrDefault(p => p.Guid.Full.Equals(playerId));
        }
    }
}
