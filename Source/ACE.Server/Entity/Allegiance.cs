using System;
using System.Collections.Generic;
using System.Text;
using ACE.Server.WorldObjects;
using ACE.Server.Managers;

namespace ACE.Server.Entity
{
    public class Allegiance
    {
        /// <summary>
        /// The top of the AllegianceNode tree
        /// </summary>
        public AllegianceNode Monarch;

        /// <summary>
        /// The total # of players in the Allegiance
        /// </summary>
        public int TotalMembers { get => Members.Count; }

        /// <summary>
        /// A lookup table of Players => AllegianceNodes
        /// </summary>
        public Dictionary<Player, AllegianceNode> Members;

        /// <summary>
        /// Constructs a new Allegiance from a Monarch
        /// </summary>
        public Allegiance(Player monarch)
        {
            Monarch = new AllegianceNode(monarch, this);

            // find all players with this monarch
            var members = AllegianceManager.FindAllPlayers(monarch);

            Monarch.BuildChain(this, members);
            BuildMembers(Monarch);

            Console.WriteLine("TotalMembers: " + TotalMembers);
        }

        /// <summary>
        /// Builds the lookup table of Players => AllegianceNodes
        /// </summary>
        public void BuildMembers(AllegianceNode node)
        {
            if (Monarch.Player.Equals(node.Player))
                Members = new Dictionary<Player, AllegianceNode>();

            Members.Add(node.Player, node);

            foreach (var vassal in node.Vassals)
                BuildMembers(vassal);
        }
    }
}
