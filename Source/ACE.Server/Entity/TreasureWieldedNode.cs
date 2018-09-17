using System.Collections.Generic;
using ACE.Database.Models.World;

namespace ACE.Server.Entity
{
    public class TreasureWieldedNode
    {
        public TreasureWielded Item;

        public TreasureWieldedSet Subset;

        public TreasureWieldedNode(List<TreasureWielded> items, int idx)
        {
            Item = items[idx];
        }

        public int TotalNestedItems
        {
            get
            {
                var totalItems = 1;

                if (Subset != null)
                    totalItems += Subset.TotalNestedItems;

                return totalItems;
            }
        }

        public int TotalNestedSets { get => Subset != null ? Subset.TotalNestedSets : 0; }

        public int GetMaxDepth(int depth)
        {
            var subsetDepth = Subset != null ? Subset.GetMaxDepth(depth) : 0;
            return depth + subsetDepth + 1;
        }
    }
}
