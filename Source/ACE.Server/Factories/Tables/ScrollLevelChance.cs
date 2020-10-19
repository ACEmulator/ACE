using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Database.Models.World;

namespace ACE.Server.Factories.Tables
{
    public static class ScrollLevelChance
    {
        private static ChanceTable<int> T1_ScrollLevelChances = new ChanceTable<int>()
        {
            ( 1, 0.25f ),
            ( 2, 0.50f ),
            ( 3, 0.25f )
        };

        private static ChanceTable<int> T2_ScrollLevelChances = new ChanceTable<int>()
        {
            ( 3, 0.25f ),
            ( 4, 0.50f ),
            ( 5, 0.25f )
        };

        private static ChanceTable<int> T3_ScrollLevelChances = new ChanceTable<int>()
        {
            ( 5, 0.25f ),
            ( 6, 0.50f ),
            ( 7, 0.25f )
        };

        private static ChanceTable<int> T4_ScrollLevelChances = new ChanceTable<int>()
        {
            ( 6, 0.50f ),
            ( 7, 0.50f )
        };

        private static ChanceTable<int> T5_T8_ScrollLevelChances = new ChanceTable<int>()
        {
            ( 7, 1.00f )
        };

        private static readonly List<ChanceTable<int>> scrollLevelChances = new List<ChanceTable<int>>()
        {
            T1_ScrollLevelChances,
            T2_ScrollLevelChances,
            T3_ScrollLevelChances,
            T4_ScrollLevelChances,
            T5_T8_ScrollLevelChances,
            T5_T8_ScrollLevelChances,
            T5_T8_ScrollLevelChances,
            T5_T8_ScrollLevelChances,
        };

        public static int Roll(TreasureDeath profile)
        {
            // shortcut
            if (profile.Tier >= 5)
                return 7;

            var table = scrollLevelChances[profile.Tier - 1];

            return table.Roll(profile.LootQualityMod);
        }
    }
}
