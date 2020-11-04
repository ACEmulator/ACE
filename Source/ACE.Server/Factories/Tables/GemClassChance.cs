using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class GemClassChance
    {
        private static ChanceTable<int> T1_Chances = new ChanceTable<int>()
        {
            ( 1, 0.9f ),
            ( 2, 0.1f ),
        };

        private static ChanceTable<int> T2_Chances = new ChanceTable<int>()
        {
            ( 1, 0.6f ),
            ( 2, 0.3f ),
            ( 3, 0.1f ),
        };

        private static ChanceTable<int> T3_Chances = new ChanceTable<int>()
        {
            ( 1, 0.2f ),
            ( 2, 0.5f ),
            ( 3, 0.2f ),
            ( 4, 0.1f ),
        };

        private static ChanceTable<int> T4_Chances = new ChanceTable<int>()
        {
            ( 1, 0.05f ),
            ( 2, 0.25f ),
            ( 3, 0.3f ),
            ( 4, 0.25f ),
            ( 5, 0.15f ),
        };

        private static ChanceTable<int> T5_Chances = new ChanceTable<int>()
        {
            ( 3, 0.2f ),
            ( 4, 0.4f ),
            ( 5, 0.2f ),
            ( 6, 0.2f ),
        };

        private static ChanceTable<int> T6_Chances = new ChanceTable<int>()
        {
            ( 4, 0.2f ),
            ( 5, 0.3f ),
            ( 6, 0.5f ),
        };

        private static readonly List<ChanceTable<int>> gemClassChances = new List<ChanceTable<int>>()
        {
            T1_Chances,
            T2_Chances,
            T3_Chances,
            T4_Chances,
            T5_Chances,
            T6_Chances,
        };

        /// <summary>
        /// Rolls for a GemClass value between 1-6 for a tier
        /// </summary>
        public static int Roll(int tier)
        {
            // todo: add t7 / t8
            tier = Math.Clamp(tier, 1, 6);

            var gemClassChanceTable = gemClassChances[tier - 1];

            return gemClassChanceTable.Roll();
        }
    }
}
