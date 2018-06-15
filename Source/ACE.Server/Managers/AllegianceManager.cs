using System;
using System.Collections.Generic;
using System.Text;
using ACE.Server.WorldObjects;
using ACE.Entity.Enum.Properties;
using ACE.Server.Managers;
using ACE.Server.Entity;

namespace ACE.Server.Managers
{
    /// <summary>
    /// Allegiance helper methods
    /// </summary>
    public class AllegianceManager
    {
        /// <summary>
        /// A mapping of all Players on the server => their AllegianceNodes
        /// </summary>
        public static Dictionary<Player, AllegianceNode> Players;

        static AllegianceManager()
        {
            Players = new Dictionary<Player, AllegianceNode>();
        }

        /// <summary>
        /// Returns the monarch for a player
        /// </summary>
        public static Player GetMonarch(Player player)
        {
            // find the monarch by walking the allegiance chain
            var currentPlayer = player;
            do
            {
                var patron = currentPlayer.GetProperty(PropertyInstanceId.Patron);
                if (patron != null)
                    currentPlayer = WorldManager.GetPlayerByGuidId(patron.Value);
                else
                    return currentPlayer;
            }
            while (true);
        }

        /// <summary>
        /// Returns the full allegiance structure for any player
        /// </summary>
        /// <param name="player">A player at any level of an allegiance</param>
        public static Allegiance GetAllegiance(Player player)
        {
            var monarch = GetMonarch(player);

            var allegiance = new Allegiance(monarch);
            if (allegiance.TotalMembers == 1)
                return null;
            else
            {
                AddPlayers(allegiance);
                return allegiance;
            }
        }

        /// <summary>
        /// Returns the AllegianceNode for a Player
        /// </summary>
        public static AllegianceNode GetAllegianceNode(Player player)
        {
            Players.TryGetValue(player, out var allegianceNode);
            return allegianceNode;
        }

        /// <summary>
        /// Returns a list of all players under a monarch
        /// </summary>
        public static List<Player> FindAllPlayers(Player monarch)
        {
            return WorldManager.GetAllegiance(monarch);
        }

        /// <summary>
        /// Loads the Allegiance and AllegianceNode for a Player
        /// </summary>
        public static void LoadPlayer(Player player)
        {
            player.Allegiance = GetAllegiance(player);
            player.AllegianceNode = GetAllegianceNode(player);
        }

        /// <summary>
        /// Appends the Players lookup table with the members of an Allegiance
        /// </summary>
        public static void AddPlayers(Allegiance allegiance)
        {
            foreach (var member in allegiance.Members)
            {
                var player = member.Key;
                var allegianceNode = member.Value;

                if (!Players.ContainsKey(player))
                    Players.Add(player, allegianceNode);
                else
                    Players[player] = allegianceNode;
            }
        }
    }
}
