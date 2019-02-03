using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class AllegianceNode
    {
        public readonly ObjectGuid PlayerGuid;
        public IPlayer Player => PlayerManager.FindByGuid(PlayerGuid);

        public readonly Allegiance Allegiance;

        public readonly AllegianceNode Monarch;
        public readonly AllegianceNode Patron;
        public List<AllegianceNode> Vassals;

        public uint Rank;

        public bool IsMonarch => Patron == null;

        public bool HasVassals => TotalVassals > 0;

        public int TotalVassals => Vassals != null ? Vassals.Count : 0;

        public int TotalFollowers
        {
            get
            {
                var totalFollowers = 0;

                foreach (var vassal in Vassals)
                    totalFollowers += vassal.TotalFollowers + 1;

                return totalFollowers;
            }
        }

        public AllegianceNode(ObjectGuid playerGuid, Allegiance allegiance, AllegianceNode monarch = null, AllegianceNode patron = null)
        {
            PlayerGuid = playerGuid;
            Allegiance = allegiance;
            Monarch = monarch ?? this;
            Patron = patron;
        }

        public void BuildChain(Allegiance allegiance, List<IPlayer> players)
        {
            var vassals = players.Where(p => p.PatronId == PlayerGuid.Full).ToList();

            Vassals = new List<AllegianceNode>();

            foreach (var vassal in vassals)
            {
                var node = new AllegianceNode(vassal.Guid, allegiance, Monarch, this);
                node.BuildChain(allegiance, players);

                Vassals.Add(node);
            }

            CalculateRank();
        }

        public void CalculateRank()
        {
            // http://asheron.wikia.com/wiki/Rank

            // A player's allegiance rank is a function of the number of Vassals and how they are
            // oraganized. First, take the two highest ranked vassals. Now the Patron's rank will either be
            // one higher than the lower of the two, or equal to the highest rank vassal, whichever is greater.

            // sort vassals by rank
            var sortedVassals = Vassals.OrderBy(v => v.Rank).ToList();

            // get 2 highest rank vassals
            var r1 = sortedVassals.Count > 0 ? sortedVassals[0].Rank : 0;
            var r2 = sortedVassals.Count > 1 ? sortedVassals[1].Rank : 0;

            var lower = Math.Min(r1, r2);
            var higher = Math.Max(r1, r2);

            Rank = Math.Max(lower + 1, higher);
        }
    }
}
