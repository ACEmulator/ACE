using System;
using System.Collections.Generic;

using ACE.Server.Managers;

namespace ACE.Server.Entity
{
    public class Allegiance
    {
        /// <summary>
        /// The top of the AllegianceNode tree
        /// </summary>
        public readonly AllegianceNode Monarch;

        /// <summary>
        /// The total # of players in the Allegiance
        /// </summary>
        public int TotalMembers => Members.Count;

        /// <summary>
        /// A lookup table of Players => AllegianceNodes
        /// </summary>
        public Dictionary<IPlayer, AllegianceNode> Members;

        /// <summary>
        /// Constructs a new Allegiance from a Monarch
        /// </summary>
        public Allegiance(IPlayer monarch)
        {
            Monarch = new AllegianceNode(monarch, this);

            // find all players with this monarch
            var members = AllegianceManager.FindAllPlayers(monarch);

            Monarch.BuildChain(this, members);
            BuildMembers(Monarch);

            //Console.WriteLine("TotalMembers: " + TotalMembers);
        }

        /// <summary>
        /// Builds the lookup table of Players => AllegianceNodes
        /// </summary>
        public void BuildMembers(AllegianceNode node)
        {
            if (Monarch.Player.Equals(node.Player))
                Members = new Dictionary<IPlayer, AllegianceNode>();

            Members.Add(node.Player, node);

            foreach (var vassal in node.Vassals)
                BuildMembers(vassal);
        }

        /// <summary>
        /// An allegiance is defined by its monarch
        /// </summary>
        public override bool Equals(Object obj)
        {
            if (obj is Allegiance allegiance)
                return Monarch.Player.Guid.Full == allegiance.Monarch.Player.Guid.Full;

            return false;
        }

        public override int GetHashCode()
        {
            return Monarch.Player.Guid.Full.GetHashCode();
        }
    }
}
