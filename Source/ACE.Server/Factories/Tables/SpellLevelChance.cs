using System.Collections.Generic;

using log4net;

using ACE.Common;

namespace ACE.Server.Factories.Tables
{
    public static class SpellLevelChance
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<float> T1_SpellLevelChances = new List<float>()
        {
            0.25f,
            0.5f,
            0.25f,
            0,
            0,
            0,
            0,
            0,
        };

        private static readonly List<float> T2_SpellLevelChances = new List<float>()
        {
            0,
            0,
            0.25f,
            0.5f,
            0.25f,
            0,
            0,
            0
        };

        private static readonly List<float> T3_SpellLevelChances = new List<float>()
        {
            0,
            0,
            0,
            0.25f,
            0.5f,
            0.25f,
            0,
            0,
        };

        private static readonly List<float> T4_SpellLevelChances = new List<float>()
        {
            0,
            0,
            0,
            0,
            0.75f,
            0.25f,
            0,
            0,
        };

        private static readonly List<float> T5_SpellLevelChances = new List<float>()
        {
            0,
            0,
            0,
            0,
            0.5f,
            0.5f,
            0,
            0,
        };

        private static readonly List<float> T6_SpellLevelChances = new List<float>()
        {
            0,
            0,
            0,
            0,
            0.25f,
            0.75f,
            0,
            0,
        };

        // TODO: adjust
        private static readonly List<float> T7_SpellLevelChances = new List<float>()
        {
            0,
            0,
            0,
            0,
            0,
            0.75f,
            0.25f,
            0
        };

        private static readonly List<float> T8_SpellLevelChances = new List<float>()
        {
            0,
            0,
            0,
            0,
            0,
            0,
            0.75f,
            0.25f
        };

        /// <summary>
        /// Returns the spell level chance table for a tier
        /// </summary>
        public static List<float> GetSpellLevelChancesForTier(int tier)
        {
            switch (tier)
            {
                case 1:
                default:
                    return T1_SpellLevelChances;
                case 2:
                    return T2_SpellLevelChances;
                case 3:
                    return T3_SpellLevelChances;
                case 4:
                    return T4_SpellLevelChances;
                case 5:
                    return T5_SpellLevelChances;
                case 6:
                    return T6_SpellLevelChances;
                case 7:
                    return T7_SpellLevelChances;
                case 8:
                    return T8_SpellLevelChances;
            }
        }

        /// <summary>
        /// Rolls for a spell level for a tier
        /// </summary>
        public static int Roll(int tier)
        {
            var spellLevelChances = GetSpellLevelChancesForTier(tier);

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            var total = 0.0f;
            for (var i = 0; i < spellLevelChances.Count; i++)
            {
                total += spellLevelChances[i];
                if (rng < total)
                    return i + 1;
            }

            // this shouldn't normally happen, floating point addition imprecision?
            log.Error($"LootGenerationFactory - SpellLevel.Roll - This shouldn't happen, tier={tier}, total={total}, rng={rng}");
            for (var i = spellLevelChances.Count - 1; i >= 0; i--)
            {
                if (spellLevelChances[i] > 0)
                    return i + 1;
            }
            log.Error($"LootGenerationFactory - SpellLevel.Roll - This really shouldn't happen");
            return 1;
        }
    }
}
