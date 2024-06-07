using System.Collections.Generic;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class AetheriaChance
    {
        private static ChanceTable<int> T5_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.60f ),
            ( 2, 0.40f ),
        };

        private static ChanceTable<int> T6_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.35f ),
            ( 2, 0.405f ),
            ( 3, 0.24f ),
            ( 4, 0.005f ),
        };

        // lack of data samples here for 4+
        private static ChanceTable<int> T7_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 3, 0.6f ),
            ( 4, 0.35f ),
            ( 5, 0.05f ),
        };

        // also lack of data samples for level 5,
        // there was possibly no indication it was more common than t7
        private static ChanceTable<int> T8_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 3, 0.4f ),
            ( 4, 0.45f ),
            ( 5, 0.15f ),
        };

        private static ChanceTable<int> T9_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 3, 0.375f ),
            ( 4, 0.45f ),
            ( 5, 0.175f ),
        };

        private static ChanceTable<int> T10_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 3, 0.375f ),
            ( 4, 0.40f ),
            ( 5, 0.225f ),
        };

        private static ChanceTable<int> T11_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 4, 0.5f ),
            ( 5, 0.5f ),
        };

        private static ChanceTable<int> T12_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 5, 1.0f ),
        };
        private static readonly List<ChanceTable<int>> itemMaxLevels = new List<ChanceTable<int>>()
        {
            T5_ItemMaxLevel,
            T6_ItemMaxLevel,
            T7_ItemMaxLevel,
            T8_ItemMaxLevel,
            T9_ItemMaxLevel,
            T10_ItemMaxLevel,
            T11_ItemMaxLevel,
            T12_ItemMaxLevel,
        };

        public static int Roll_ItemMaxLevel(TreasureDeath profile)
        {
            if (profile.Tier < 5)
                return 0;

            var table = itemMaxLevels[profile.Tier - 5];

            return table.Roll(profile.LootQualityMod);
        }
    }
}
