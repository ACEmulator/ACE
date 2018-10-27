using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Server.Entity;
using ACE.Server.Network;
using ACE.Server.WorldObjects;

namespace ACE.Server.Managers
{
    public static class PlayerManager
    {
        [Obsolete]
        // probably bugged when players are added/removed...
        public static readonly List<Player> AllPlayers = new List<Player>();

        public static readonly ConcurrentDictionary<uint, Player> OnlinePlayers = new ConcurrentDictionary<uint, Player>();

        public static readonly ConcurrentDictionary<uint, OfflinePlayer> OfflinePlayers = new ConcurrentDictionary<uint, OfflinePlayer>();

        public static void Initialize()
        {
            LoadOfflinePlayers();
        }

        private static void LoadOfflinePlayers()
        {
            var results = DatabaseManager.Shard.GetAllPlayerBiotasInParallel();

            foreach (var result in results)
            {
                var offlinePlayer = new OfflinePlayer(result);
                OfflinePlayers[offlinePlayer.Guid.Full] = offlinePlayer;
            }
        }


        public static void AddOfflinePlayer(Player player)
        {
            var offlinePlayer = new OfflinePlayer(player.Biota);
            OfflinePlayers[offlinePlayer.Guid.Full] = offlinePlayer;
        }







        /// <summary>
        /// Returns an offline player record from the AllPlayers list
        /// </summary>
        /// <param name="playerGuid"></param>
        /// <returns></returns>
        public static Player GetOfflinePlayer(ObjectGuid playerGuid)
        {
            return AllPlayers.FirstOrDefault(p => p.Guid.Equals(playerGuid));
        }

        /// <summary>
        /// Syncs the cached offline player fields
        /// </summary>
        /// <param name="player">An online player</param>
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
        public static void SyncOnline(Player player)
        {
            var offlinePlayer = AllPlayers.FirstOrDefault(p => p.Guid.Full == player.Guid.Full);
            if (offlinePlayer == null) return;

            // FIXME: this is a placeholder for offline players
            player.AllegianceCPPool = offlinePlayer.AllegianceCPPool;
        }

        public static Player GetOfflinePlayerByGuidId(uint playerId)
        {
            return AllPlayers.FirstOrDefault(p => p.Guid.Full.Equals(playerId));
        }

        /// <summary>
        /// Returns a list of all players who are under a monarch
        /// </summary>
        /// <param name="monarch">The monarch of an allegiance</param>
        public static List<Player> GetAllegiance(Player monarch)
        {
            return AllPlayers.Where(p => p.Monarch == monarch.Guid.Full).ToList();
        }
    }
}
