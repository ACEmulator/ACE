using System.Collections.Generic;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class AetheriaChance
    {
        private static ChanceTable<int> T5_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.4286f ),
            ( 2, 0.2857f ),
            ( 3, 0.2857f ),
        };

        private static ChanceTable<int> T6_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.3428f ),
            ( 2, 0.2286f ),
            ( 3, 0.2286f ),
            ( 4, 0.2000f ),
        };

        // lack of data samples here for 4+
        private static ChanceTable<int> T7_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.2286f ),
            ( 2, 0.2857f ),
            ( 3, 0.2857f ),
            ( 4, 0.1600f ),
            ( 5, 0.0400f ),
        };

        // also lack of data samples for level 5,
        // there was possibly no indication it was more common than t7
        private static ChanceTable<int> T8_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 2, 0.4000f ),
            ( 3, 0.4000f ),
            ( 4, 0.1600f ),
            ( 5, 0.0400f ),
        };

        private static readonly List<ChanceTable<int>> itemMaxLevels = new List<ChanceTable<int>>()
        {
            T5_ItemMaxLevel,
            T6_ItemMaxLevel,
            T7_ItemMaxLevel,
            T8_ItemMaxLevel
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
