using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class AetheriaChance
    {
        private static readonly List<WeenieClassName> aetheriaColors = new List<WeenieClassName>()
        {
            WeenieClassName.ace42635_aetheria,  // blue
            WeenieClassName.ace42637_aetheria,  // yellow
            WeenieClassName.ace42636_aetheria,  // red
        };

        public static WeenieClassName RollWcid(int tier)
        {
            switch (tier)
            {
                // blue only
                case 5:
                    return aetheriaColors[0];

                // even chance between blue / yellow
                case 6:
                    var rng = ThreadSafeRandom.Next(0, 1);
                    return aetheriaColors[rng];

                // even chance between blue / yellow / red
                case 7:
                case 8:
                    rng = ThreadSafeRandom.Next(0, 2);
                    return aetheriaColors[rng];     
            }
            return WeenieClassName.undef;
        }

        private static readonly ChanceTable<int> T5_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.60f ),
            ( 2, 0.40f ),
        };

        private static readonly ChanceTable<int> T6_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.550f ),
            ( 2, 0.345f ),
            ( 3, 0.100f ),
            ( 4, 0.005f ),
        };

        // lack of data samples here for 4+
        private static readonly ChanceTable<int> T7_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.55000f ),
            ( 2, 0.34475f ),
            ( 3, 0.10000f ),
            ( 4, 0.00500f ),
            ( 5, 0.00025f ),
        };

        // also lack of data samples for level 5,
        // there was possibly no indication it was more common than t7
        private static readonly ChanceTable<int> T8_ItemMaxLevel = new ChanceTable<int>()
        {
            ( 1, 0.0200f ),
            ( 2, 0.5500f ),
            ( 3, 0.4195f ),
            ( 4, 0.0100f ),
            ( 5, 0.0005f ),
        };

        private static readonly List<ChanceTable<int>> itemMaxLevels = new List<ChanceTable<int>>()
        {
            T5_ItemMaxLevel,
            T6_ItemMaxLevel,
            T7_ItemMaxLevel,
            T8_ItemMaxLevel
        };

        public static int RollItemMaxLevel(TreasureDeath profile)
        {
            if (profile.Tier < 5)
                return 0;

            var table = itemMaxLevels[profile.Tier - 5];

            return table.Roll(profile.LootQualityMod);
        }
    }
}
