using System;
using System.Collections.Generic;
using ACE.Database.Models.World;

namespace ACE.Server.Entity
{
    /// <summary>
    /// Represents 1 TreasureWielded set from a TreasuredWielded table
    /// A roll is made for each set, from 0-TotalProbability (min. 1)
    /// </summary>
    public class TreasureWieldedSet
    {
        /// <summary>
        /// The list of item directly in this set
        /// Each item in turn can link to a subset
        /// </summary>
        public List<TreasureWieldedNode> Items;

        /// <summary>
        /// Returns the total nested items
        /// from set items + subsets
        /// </summary>
        public int TotalNestedItems
        {
            get
            {
                var totalItems = 0;

                foreach (var item in Items)
                    totalItems += item.TotalNestedItems;

                return totalItems;
            }
        }

        /// <summary>
        /// Returns the total number of nested sets,
        /// including item subsets
        /// </summary>
        public int TotalNestedSets
        {
            get
            {
                var totalSets = 1;

                foreach (var item in Items)
                    totalSets += item.TotalNestedSets;

                return totalSets;
            }
        }

        /// <summary>
        /// Returns the maximum depth of the nested sets
        /// </summary>
        public int GetMaxDepth(int setDepth = 0)
        {
            var maxDepth = setDepth;

            foreach (var item in Items)
            {
                var depth = item.GetMaxDepth(setDepth);

                if (depth > maxDepth)
                    maxDepth = depth;
            }
            return maxDepth;
        }

        /// <summary>
        /// Returns the total probability of the direct items
        /// minimum 100%
        /// </summary>
        public float TotalProbability
        {
            get
            {
                var totalProbability = 0.0f;

                foreach (var item in Items)
                    totalProbability += item.Item.Probability;

                return Math.Max(totalProbability, 1.0f);
            }
        }

        /// <summary>
        /// Parses 1 TreasureWielded set from a TreasuredWielded table
        /// </summary>
        /// <param name="items">The list of items in the complete TreasureWielded table</param>
        /// <param name="startIdx">The start index of the set to parse</param>
        public TreasureWieldedSet(List<TreasureWielded> items, int startIdx, int depth = 0)
        {
            Items = new List<TreasureWieldedNode>();

            // add set start item
            var item = items[startIdx];

            if (!item.SetStart)
            {
                Console.WriteLine($"Warning: TreasureWieldedSet constructor called with startIdx {startIdx}, and SetStart=0");
            }

            var node = new TreasureWieldedNode(items, startIdx);
            Items.Add(node);
            //Console.WriteLine($"Parsing startSet item {item.WeenieClassId} @ idx {startIdx}, depth {depth}");

            // continue parsing
            for (var idx = startIdx + 1; idx < items.Count; idx++)
            {
                var prevItem = items[idx - 1];
                item = items[idx];

                // handle subsets
                if (prevItem.HasSubSet)
                {
                    if (item.SetStart)
                    {
                        var subset = new TreasureWieldedSet(items, idx, depth + 1);
                        node.Subset = subset;
                        var totalItems = subset.TotalNestedItems;
                        idx += totalItems - 1;
                    }
                    else
                        Console.WriteLine($"Warning: Subset detected on line {idx - 1}, but next idx is not a set start");
                }
                else
                {
                    // handle regular items
                    if (item.SetStart)
                    {
                        if (item.ContinuesPreviousSet)
                        {
                            // ensure subset parsed
                            if (node.Subset != null)
                            {
                                node = new TreasureWieldedNode(items, idx);
                                Items.Add(node);
                            }
                            else
                            {
                                if (depth == 0)
                                    Console.WriteLine($"Warning: continuing a previous set on line {idx}, but no subset found!");
                                else
                                    break;  // back to parent
                            }
                        }
                        else
                        {
                            // set completed
                            break;
                        }
                    }
                    else
                    {
                        // normal set continues...
                        node = new TreasureWieldedNode(items, idx);
                        Items.Add(node);
                    }
                }
            }
        }
    }
}
