using System;
using System.Collections.Generic;
using ACE.Database.Models.World;

namespace ACE.Server.Entity
{
    public class TreasureWieldedTable
    {
        public List<TreasureWieldedSet> Sets;

        public int TotalNestedSets
        {
            get
            {
                var cnt = 0;

                foreach (var set in Sets)
                    cnt += set.TotalNestedSets;

                return cnt;
            }
        }

        public int MaxDepth
        {
            get
            {
                var maxDepth = 0;

                foreach (var set in Sets)
                {
                    var depth = set.GetMaxDepth();

                    if (depth > maxDepth)
                        maxDepth = depth;
                }
                return maxDepth;
            }
        }

        public TreasureWieldedTable(List<TreasureWielded> items)
        {
            Sets = new List<TreasureWieldedSet>();

            TreasureWieldedSet currentSet = null;

            for (var idx = 0; idx < items.Count; idx++)
            {
                var item = items[idx];
                if (item.SetStart)
                {
                    currentSet = new TreasureWieldedSet(items, idx);
                    Sets.Add(currentSet);
                    var totalNestedItems = currentSet.TotalNestedItems;
                    idx += totalNestedItems - 1;
                }
                else
                    Console.WriteLine($"Warning: started parsing set with no SetStart on line {idx}");
            }
        }
    }
}
