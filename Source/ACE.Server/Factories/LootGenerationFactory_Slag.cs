using ACE.Common;
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
        /// Rolls to generate slag for a player killed by an OlthoiPlayer
        /// </summary>
        public static WorldObject RollSlag(Player player)
        {
            if (player.Level < 100) return null;

            // determine preliminary scale of slag to drop based on Player level

            // just drop a static amount for testing for now
            var slag = WorldObjectFactory.CreateNewWorldObject((uint)slagWcid);

            slag.SetStackSize(3);

            return slag;
        }
    }
}
