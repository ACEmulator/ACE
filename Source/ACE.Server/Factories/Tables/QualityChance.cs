using System.Collections.Generic;

using log4net;

using ACE.Common;
using ACE.Database.Models.World;

namespace ACE.Server.Factories.Tables
{
    public static class QualityChance
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<float> QualityChancePerTier = new List<float>()
        {
            0.5f,
            0.6f,
            0.7f,
            0.8f,
            0.9f,
            1.0f,
            1.0f,
            1.0f
        };

        private static readonly List<float> QualityChancePerTier_ArmorModVsType = new List<float>()
        {
            0.0f,
            0.1f,
            0.2f,
            0.3f,
            0.5f,
            0.6f,
            0.7f,
            0.8f
        };

        private static readonly List<float> T1_QualityChances = new List<float>()
        {
            1.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
        };

        private static readonly List<float> T2_QualityChances = new List<float>()
        {
            0.75f,
            1.00f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
        };

        private static readonly List<float> T3_QualityChances = new List<float>()
        {
            0.20f,
            0.50f,
            0.80f,
            1.00f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
        };

        private static readonly List<float> T4_QualityChances = new List<float>()
        {
            0.0f,
            0.10f,
            0.30f,
            0.70f,
            0.90f,
            1.00f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
        };

        private static readonly List<float> T5_QualityChances = new List<float>()
        {
            0.0f,
            0.0f,
            0.10f,
            0.30f,
            0.70f,
            0.90f,
            1.00f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
        };

        private static readonly List<float> T6_QualityChances = new List<float>()
        {
            0.0f,
            0.0f,
            0.0f,
            0.10f,
            0.25f,
            0.50f,
            0.75f,
            0.90f,
            1.00f,
            0.0f,
            0.0f,
            0.0f,
        };

        private static readonly List<float> T7_QualityChances = new List<float>()
        {
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.10f,
            0.25f,
            0.50f,
            0.75f,
            0.90f,
            1.00f,
            0.0f,
            0.0f,
        };

        private static readonly List<float> T8_QualityChances = new List<float>()
        {
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.0f,
            0.10f,
            0.25f,
            0.50f,
            0.75f,
            0.90f,
            1.00f,
        };

        /// <summary>
        /// Returns the quality chance tables for a tier
        /// </summary>
        public static List<float> GetQualityChancesForTier(int tier)
        {
            switch (tier)
            {
                case 1:
                default:
                    return T1_QualityChances;
                case 2:
                    return T2_QualityChances;
                case 3:
                    return T3_QualityChances;
                case 4:
                    return T4_QualityChances;
                case 5:
                    return T5_QualityChances;
                case 6:
                    return T6_QualityChances;
                case 7:
                    return T7_QualityChances;
                case 8:
                    return T8_QualityChances;
            }
        }

        /// <summary>
        /// Rolls for the initial chance of getting a quality bonus for an item
        /// </summary>
        /// <param name="treasureDeath">The chances are based on treasureDeath.Tier, and can be increased with treasureDeath.LootQualityMod</param>
        /// <param name="isArmorModVsType">ArmorModVsType has a separate chance table</param>
        private static bool RollTierChance(TreasureDeath treasureDeath, bool isArmorModVsType = false)
        {
            var tierChance = isArmorModVsType ? QualityChancePerTier_ArmorModVsType[treasureDeath.Tier - 1] : QualityChancePerTier[treasureDeath.Tier - 1];

            // use for initial roll? logic seems backwards here...
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f) - treasureDeath.LootQualityMod;

            return rng < tierChance;
        }

        /// <summary>
        /// Returns a quality level between 0-12
        /// </summary>
        public static int Roll(TreasureDeath treasureDeath, bool isArmorModVsType = false)
        {
            // roll for the initial chance for any quality modification -- based on tier
            if (!RollTierChance(treasureDeath, isArmorModVsType))
                return 0;

            // if the initial roll succeeds, roll for the actual quality level -- also based on tier
            var chances = GetQualityChancesForTier(treasureDeath.Tier);

            var rng = ThreadSafeRandom.Next(treasureDeath.LootQualityMod, 1.0f);

            for (var i = 0; i < chances.Count; i++)
            {
                var curChance = chances[i];

                if (rng < curChance)
                    return i + 1;
            }
            log.Error($"QualityTables.Roll({treasureDeath.Tier}, {treasureDeath.LootQualityMod}, {isArmorModVsType}) - this shouldn't happen");
            return 0;
        }

        /// <summary>
        /// Performs a weighted RNG roll
        /// linearly interpolates between discrete values
        /// </summary>
        /// <returns>A quality level between 0.0f - 1.0f, higher values = better</returns>
        public static float RollInterval(TreasureDeath treasureDeath)
        {
            // roll for the initial chance for any quality modification -- based on tier
            if (!RollTierChance(treasureDeath))
                return 0.0f;

            // if the initial roll succeeds, roll for the actual quality level -- also based on tier
            var chances = GetQualityChancesForTier(treasureDeath.Tier);

            var rng = ThreadSafeRandom.Next(treasureDeath.LootQualityMod, 1.0f);

            for (var i = 0; i < chances.Count; i++)
            {
                var curChance = chances[i];

                if (rng < curChance)
                {
                    var prevChance = i > 0 ? chances[i - 1] : 0;

                    var dx = curChance - prevChance;
                    var dy = 1.0f / chances.Count;

                    var interval = (rng - prevChance) / dx;

                    return (float)(dy * (interval + i));
                }
            }
            log.Error($"QualityTables.RollInterval({treasureDeath.Tier}, {treasureDeath.LootQualityMod}) - this shouldn't happen");
            return 0.0f;
        }
    }
}
