using System.Collections.Generic;

using log4net;

using ACE.Common;

namespace ACE.Server.Factories.Tables
{
    public static class QualityChance
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly List<float> QualityChancePerTier = new List<float>()
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

        public static readonly List<float> T1_QualityChances = new List<float>()
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

        public static readonly List<float> T2_QualityChances = new List<float>()
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

        public static readonly List<float> T3_QualityChances = new List<float>()
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

        public static readonly List<float> T4_QualityChances = new List<float>()
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

        public static readonly List<float> T5_QualityChances = new List<float>()
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

        public static readonly List<float> T6_QualityChances = new List<float>()
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

        public static readonly List<float> T7_QualityChances = new List<float>()
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

        public static readonly List<float> T8_QualityChances = new List<float>()
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
        /// Performs a weighted RNG roll
        /// linearly interpolates between discrete values
        /// </summary>
        /// <returns>A value between 0.0f - 1.0f, higher values = better</returns>
        public static float Roll(int tier)
        {
            var chances = GetQualityChancesForTier(tier);

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

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
            log.Error($"QualityTables.Roll - This shouldn't happen");
            return 0.0f;
        }
    }
}
