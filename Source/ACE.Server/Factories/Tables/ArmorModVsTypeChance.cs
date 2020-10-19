using System;
using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;

namespace ACE.Server.Factories.Tables
{
    public static class ArmorModVsTypeChance
    {
        // for items that have a PropertyInt.MutateFilter that contains ArmorModVsType,
        // a preliminary roll is performed for each elemental type, that determines if the ArmorModVsType will get mutated
        // this was a relatively rare mutation at lower tiers, and eor did not have extra chances beyond t6 for t7/t8
        private static readonly List<float> TierChances = new List<float>()
        {
            0.00f,  // T1
            0.01f,  // T2
            0.05f,  // T3
            0.08f,  // T4
            0.25f,  // T5
            0.40f,  // T6
            0.40f,  // T7
            0.40f,  // T8
        };

        public static bool Roll(int tier)
        {
            // shortcut
            if (tier < 2)
                return false;

            // quality mod?
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            return rng < TierChances[tier - 1];
        }

        // if the initial roll was successful for an elemental type,
        // this table determines the 1-5 quality level that could be returned per tier
        // the eor values for this seem odd, and i'm finding no cache tables that align with this
        // it seems heavily weighted towards the higher end, esp. at the higher tiers,
        // to the point where the rng roll is almost non-existent (95% + chance for the highest quality in that tier)

        // starting with a more conservative table found in the cache, until this data is verified more

        private static ChanceTable<int> ArmorModVsType_T2_QualityLevel = new ChanceTable<int>()
        {
            ( 1, 1.0f )
        };

        private static ChanceTable<int> ArmorModVsType_T3_QualityLevel = new ChanceTable<int>()
        {
            ( 1, 0.5f ),
            ( 2, 0.5f ),
        };

        private static ChanceTable<int> ArmorModVsType_T4_QualityLevel = new ChanceTable<int>()
        {
            ( 1, 0.25f ),
            ( 2, 0.50f ),
            ( 3, 0.25f ),
        };

        private static ChanceTable<int> ArmorModVsType_T5_QualityLevel = new ChanceTable<int>()
        {
            ( 2, 0.25f ),
            ( 3, 0.50f ),
            ( 4, 0.25f ),
        };

        private static ChanceTable<int> ArmorModVsType_T6_T8_QualityLevel = new ChanceTable<int>()
        {
            ( 2, 0.25f ),
            ( 3, 0.35f ),
            ( 4, 0.30f ),
            ( 5, 0.10f ),
        };

        private static readonly List<ChanceTable<int>> qualityLevels = new List<ChanceTable<int>>()
        {
            null,
            ArmorModVsType_T2_QualityLevel,
            ArmorModVsType_T3_QualityLevel,
            ArmorModVsType_T4_QualityLevel,
            ArmorModVsType_T5_QualityLevel,
            ArmorModVsType_T6_T8_QualityLevel,
            ArmorModVsType_T6_T8_QualityLevel,
            ArmorModVsType_T6_T8_QualityLevel,
        };

        public static int RollQualityLevel(TreasureDeath profile)
        {
            if (profile.Tier < 2)
                return 0;

            var table = qualityLevels[profile.Tier - 1];

            // quality mod?
            return table.Roll(profile.LootQualityMod);
        }
    }
}
