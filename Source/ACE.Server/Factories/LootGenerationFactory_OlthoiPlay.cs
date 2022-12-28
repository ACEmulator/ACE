using System;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static partial class LootGenerationFactory
    {
        public static readonly WeenieClassName slagWcid = WeenieClassName.coinolthoi;

        // https://asheron.fandom.com/wiki/Pitted_Slag

        private static ChanceTable<int> numSlagChance = new ChanceTable<int>()
        {
            ( 1, 0.65f ),
            ( 2, 0.25f ),
            ( 3, 0.10f ),
        };

        private static ChanceTable<bool> rareSlagChance = new ChanceTable<bool>()
        {
            ( false, 0.95f ),
            (  true, 0.05f )
        };

        /// <summary>
        /// Rolls to generate slag for a monster killed by an OlthoiPlayer
        /// </summary>
        public static WorldObject RollSlag(TreasureDeath profile)
        {
            var tier = profile.Tier;

            // https://asheron.fandom.com/wiki/Pitted_Slag
            // If killed by an Olthoi, a small amount may be dropped by creatures level 100 or greater.

            // interpreting this to mean t5+

            if (tier < 5) return null;

            // The chance of the killed creature having slag on their corpse is based exclusively on level
            // (ie. a level 220 creature will drop slag far more often than a level 100)

            // T5 = 1 in 5 chance
            // T6 = 1 in 4 chance
            // T7 = 1 in 3 chance
            // T8 = 1 in 2 chance
            var slagChance = tier switch
            {
                5 => 5,
                6 => 4,
                7 => 3,
                _ => 2
            };

            var rng = ThreadSafeRandom.Next(1, slagChance);

            if (rng < slagChance) return null;

            var slag = WorldObjectFactory.CreateNewWorldObject((uint)slagWcid);

            // roll for # of slag

            // It is commonly 1-2 slag, rarely 3-6.

            // It is possible that high level boss monsters such as the Tower Guardian
            // can yield a very high amount of slag compared to other creatures.

            // taking LootQualityMod into account for this part

            var numSlag = numSlagChance.Roll(profile.LootQualityMod);

            // 1 in 20 chance of dropping a rare amount
            var rareChance = rareSlagChance.Roll(profile.LootQualityMod);

            if (rareChance) numSlag += 3;

            if (numSlag > 1)
                slag.SetStackSize(numSlag);

            return slag;
        }

        /// <summary>
        /// The amount of time that must elapse since player was last killed by an OlthoiPlayer
        /// before they are dropping full amounts of slag again
        /// </summary>
        private static readonly TimeSpan pvpSlagTimer = TimeSpan.FromHours(1);

        /// <summary>
        /// Rolls to generate slag for a player killed by an OlthoiPlayer
        /// </summary>
        public static WorldObject RollSlag(Player player, bool hadVitae)
        {
            // https://asheron.fandom.com/wiki/Pitted_Slag

            // If killed by an Olthoi, a large amount may be dropped by players.
            //
            // It is commonly 0-15, but there are reports of up to 100 from a single player. Players only drop slag under the following conditions:
            //
            // - The player must be over a certain level. The current estimate is level 100+. Players under level 180 will not yield much slag, if at all.
            // - Each time a player is killed they are less likely to drop pitted slag. This chance goes back up with time.
            // - Players don't drop slag if they have vitae.

            // can't simply use HasVitae here
            // a freshly-killed player already has vitae by this point
            //if (player.HasVitae) return null;
            if (hadVitae) return null;

            // determine preliminary scale of slag to drop based on Player level

            // divvy player levels up into "tiers"

            var tier = GetTierHeuristic(player.Level ?? 0);

            if (tier < 5) return null;  // if less than level 100, never drop any slag

            var maxRoll = tier switch
            {
                5 => 5,
                6 => 10,
                _ => 15
            };

            var totalSlag = 0;
            do
            {
                totalSlag += ThreadSafeRandom.Next(1, maxRoll);
            }
            while (ThreadSafeRandom.Next(0.0f, 1.0f) < 0.05f);  // 5% chance for another roll

            // scale totalSlag by last death to olthoi
            var olthoiLootTimestamp = player.OlthoiLootTimestamp ?? 0;
            var currentTime = (int)Time.GetUnixTime();
            var timeDiff = Math.Max(0, currentTime - olthoiLootTimestamp);  // clamp to 0 on lower end -- avoid negatives, such as from server clock being rewound back in time
            if (timeDiff < pvpSlagTimer.TotalSeconds)   
            {
                var timeScale = (float)(timeDiff / pvpSlagTimer.TotalSeconds);
                totalSlag = (totalSlag * timeScale).Round();
            }

            if (totalSlag <= 0) return null;

            var slag = WorldObjectFactory.CreateNewWorldObject((uint)slagWcid);

            slag.SetStackSize(totalSlag);

            player.OlthoiLootTimestamp = currentTime;

            return slag;
        }

        /// <summary>
        /// Returns an approximate tier for level
        /// </summary>
        public static int GetTierHeuristic(int level)
        {
            // based on http://acpedia.org/wiki/Loot

            switch (level)
            {
                case < 20:  return 1;   // 1-19
                case < 40:  return 2;   // 20-39
                case < 60:  return 3;   // 40-59
                case < 100: return 4;   // 60-99
                case < 135: return 5;   // 100-134
                case < 185: return 6;   // 135-184
                case < 275: return 7;   // 185-274
                default:    return 8;   // 275
            }
        }

        public static readonly WeenieClassName glandWcid = WeenieClassName.olthoipvpcurrency;

        /// <summary>
        /// Rolls to generate a gland for a player that killed an OlthoiPlayer
        /// </summary>
        public static WorldObject RollGland(Player player, bool hadVitae)
        {
            // http://acpedia.org/wiki/Mutated_Olthoi_Gland

            // - OlthoiPlayers don't drop Glands if they have vitae.

            // can't simply use HasVitae here
            // a freshly-killed player already has vitae by this point
            //if (player.HasVitae) return null;

            if (hadVitae) return null;

            var rng = ThreadSafeRandom.Next(1, 100);

            if (rng <= 50)
                return null;

            var gland = WorldObjectFactory.CreateNewWorldObject((uint)glandWcid);

            return gland;
        }
    }
}
