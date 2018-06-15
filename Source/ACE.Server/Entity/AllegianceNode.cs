using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class AllegianceNode
    {
        public Player Player;

        public Allegiance Allegiance;

        public AllegianceNode Monarch;
        public AllegianceNode Patron;
        public List<AllegianceNode> Vassals;

        public uint Rank;

        public bool IsMonarch { get => Patron == null; }

        public bool HasVassals { get => Vassals != null && Vassals.Count > 0; }

        public int TotalVassals
        {
            get
            {
                if (Vassals == null)
                    return 0;

                return Vassals.Count;
            }
        }

        public AllegianceNode(Player player, Allegiance allegiance, AllegianceNode monarch = null, AllegianceNode patron = null, uint rank = 1)
        {
            Player = player;
            Allegiance = allegiance;
            Monarch = monarch != null ? monarch : this;
            Patron = patron;
            Rank = rank;
        }

        public void BuildChain(Allegiance allegiance, List<Player> players)
        {
            var vassals = players.Where(p => p.Patron == Player.Guid.Full).ToList();

            Vassals = new List<AllegianceNode>();

            foreach (var vassal in vassals)
            {
                var node = new AllegianceNode(vassal, allegiance, Monarch, this, Rank + 1);
                node.BuildChain(allegiance, players);

                Vassals.Add(node);
            }
        }
    }
}
