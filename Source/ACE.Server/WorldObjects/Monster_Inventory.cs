using System;
using System.Collections.Generic;
using System.Text;
using ACE.Database.Models.World;
using ACE.Database;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public void GetWieldedTreasure()
        {
            if (WieldedTreasure == null) return;

            Console.WriteLine($"{Name} wielded treasure:");
            var total = 0.0f;
            foreach (var item in WieldedTreasure)
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(item.WeenieClassId);
                var probability = Math.Round(item.Probability * 100, 2);
                total += item.Probability;
                Console.WriteLine($"{weenie.ClassName} - {probability}%");
            }
            Console.WriteLine("Total probability: " + Math.Round(total * 100, 2));
        }

        /// <summary>
        /// Returns the sum of the probabilities
        /// from the wielded treasure for this monster
        /// </summary>
        /// <returns></returns>
        public float GetTotalProbability()
        {
            if (WieldedTreasure == null) return 0.0f;

            var total = 0.0f;
            foreach (var item in WieldedTreasure)
                total += item.Probability;

            return total;
        }

        /// <summary>
        /// Selects a random item from the monster's WieldedTreasure table
        /// </summary>
        public TreasureWielded SelectWieldedTreasure()
        {
            if (WieldedTreasure == null)
                return null;

            var total = GetTotalProbability();

            var rng = Physics.Common.Random.RollDice(0, Math.Max(total, 1.0f));

            var current = 0.0f;
            foreach (var item in WieldedTreasure)
            {
                current += item.Probability;
                if (rng < current)
                    return item;
            }
            return null;
        }
    }
}
