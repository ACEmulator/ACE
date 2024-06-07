using System.Collections.Generic;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class NumCantrips
    {
        private static ChanceTable<int> T1_NumCantrips = new ChanceTable<int>()
        {
            ( 1, 0.1f ),
            ( 2, 0.15f ),
            ( 3, 0.4f ),
            ( 4, 0.35f ),
        };

        private static ChanceTable<int> T2_NumCantrips = new ChanceTable<int>()
        {
            ( 1, 0.1f ),
            ( 2, 0.15f ),
            ( 3, 0.4f ),
            ( 4, 0.35f ),
        };

        private static ChanceTable<int> T3_NumCantrips = new ChanceTable<int>()
        {
            ( 1, 0.1f ),
            ( 2, 0.15f ),
            ( 3, 0.4f ),
            ( 4, 0.35f ),
        };

        private static ChanceTable<int> T4_NumCantrips = new ChanceTable<int>()
        {
            ( 1, 0.1f ),
            ( 2, 0.15f ),
            ( 3, 0.4f ),
            ( 4, 0.35f ),
        };

        private static ChanceTable<int> T5_NumCantrips = new ChanceTable<int>()
        {
            ( 2, 0.15f ),
            ( 3, 0.4f ),
            ( 4, 0.45f ),
        };

        private static ChanceTable<int> T6_NumCantrips = new ChanceTable<int>()
        {
            ( 2, 0.15f ),
            ( 3, 0.4f ),
            ( 4, 0.45f ),
        };

        private static ChanceTable<int> T7_T8_NumCantrips = new ChanceTable<int>()
        {
            ( 3, 0.55f ),
            ( 4, 0.45f ),
        };

        private static ChanceTable<int> T9_NumCantrips = new ChanceTable<int>()
        {
            //( 3, 0.50f ),
            //( 4, 0.45f ),
            //( 5, 0.05f ),
            ( 5, 1.0f ),
        };

        private static ChanceTable<int> T10_NumCantrips = new ChanceTable<int>()
        {
            ( 4, 0.50f ),
            ( 5, 0.425f ),
            ( 6, 0.075f ),
        };

        private static ChanceTable<int> T11_NumCantrips = new ChanceTable<int>()
        {
            ( 4, 0.33f ),
            ( 5, 0.34f ),
            ( 6, 0.33f ),
        };
        private static ChanceTable<int> T12_NumCantrips = new ChanceTable<int>()
        {
            ( 5, 0.33f ),
            ( 6, 0.34f ),
            ( 7, 0.33f ),
        };

        private static readonly List<ChanceTable<int>> numCantrips = new List<ChanceTable<int>>()
        {
            T1_NumCantrips,
            T2_NumCantrips,
            T3_NumCantrips,
            T4_NumCantrips,
            T5_NumCantrips,
            T6_NumCantrips,
            T7_T8_NumCantrips,
            T7_T8_NumCantrips,
            T9_NumCantrips,
            T10_NumCantrips,
            T11_NumCantrips,
            T12_NumCantrips,
        };

        public static int RollNumCantrips(TreasureDeath profile)
        {
            return numCantrips[profile.Tier - 1].Roll(profile.LootQualityMod);
        }

        private static ChanceTable<int> T1_T2_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 1.0f )
        };

        private static ChanceTable<int> T3_CantripLevel = new ChanceTable<int>()
        {
            ( 1, 1.0f )
        };

        private static ChanceTable<int> T4_CantripLevel = new ChanceTable<int>()
        {
            ( 2, 1.0f )
        };

        private static ChanceTable<int> T5_CantripLevel = new ChanceTable<int>()
        {
            ( 2, 1.0f )
        };

        private static ChanceTable<int> T6_CantripLevel = new ChanceTable<int>()
        {
            ( 3, 1.0f )
        };

        private static ChanceTable<int> T7_CantripLevel = new ChanceTable<int>()
        {
            ( 3, 1.0f )
        };

        private static ChanceTable<int> T8_CantripLevel = new ChanceTable<int>()
        {
            ( 4, 1.0f )
        };

        private static ChanceTable<int> T9_CantripLevel = new ChanceTable<int>()
        {
            ( 4, 1.0f )
        };

        private static ChanceTable<int> T10_CantripLevel = new ChanceTable<int>()
        {
            ( 4, 1.0f )
        };

        private static ChanceTable<int> T11_CantripLevel = new ChanceTable<int>()
        {
            ( 4, 1.0f )
        };

        private static ChanceTable<int> T12_CantripLevel = new ChanceTable<int>()
        {
            ( 4, 1.0f )
        };

        private static readonly List<ChanceTable<int>> cantripLevels = new List<ChanceTable<int>>()
        {
            T1_T2_CantripLevel,
            T1_T2_CantripLevel,
            T3_CantripLevel,
            T4_CantripLevel,
            T5_CantripLevel,
            T6_CantripLevel,
            T7_CantripLevel,
            T8_CantripLevel,
            T9_CantripLevel,
            T10_CantripLevel,
            T11_CantripLevel,
            T12_CantripLevel,

        };

        public static int RollCantripLevel(TreasureDeath profile)
        {
            return cantripLevels[profile.Tier - 1].Roll(profile.LootQualityMod);
        }
    }
}
