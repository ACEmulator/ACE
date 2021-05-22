using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class GenericWcids
    {
        private static ChanceTable<WeenieClassName> T1_T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bowl,           0.09f ),
            ( WeenieClassName.chalice,        0.00f ),
            ( WeenieClassName.cup,            0.14f ),
            ( WeenieClassName.ewer,           0.03f ),
            ( WeenieClassName.flagon,         0.08f ),
            ( WeenieClassName.flasksimple,    0.13f ),
            ( WeenieClassName.goblet,         0.07f ),
            ( WeenieClassName.mug,            0.12f ),
            ( WeenieClassName.ornamentalbowl, 0.00f ),
            ( WeenieClassName.dinnerplate,    0.08f ),
            ( WeenieClassName.stoup,          0.13f ),
            ( WeenieClassName.tankard,        0.13f ),
        };

        private static ChanceTable<WeenieClassName> T3_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bowl,           0.08f ),
            ( WeenieClassName.chalice,        0.06f ),
            ( WeenieClassName.cup,            0.05f ),
            ( WeenieClassName.ewer,           0.11f ),
            ( WeenieClassName.flagon,         0.11f ),
            ( WeenieClassName.flasksimple,    0.05f ),
            ( WeenieClassName.goblet,         0.14f ),
            ( WeenieClassName.mug,            0.14f ),
            ( WeenieClassName.ornamentalbowl, 0.08f ),
            ( WeenieClassName.dinnerplate,    0.08f ),
            ( WeenieClassName.stoup,          0.05f ),
            ( WeenieClassName.tankard,        0.05f ),
        };

        private static ChanceTable<WeenieClassName> T5_T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bowl,           0.00f ),
            ( WeenieClassName.chalice,        0.23f ),
            ( WeenieClassName.cup,            0.00f ),
            ( WeenieClassName.ewer,           0.13f ),
            ( WeenieClassName.flagon,         0.09f ),
            ( WeenieClassName.flasksimple,    0.00f ),
            ( WeenieClassName.goblet,         0.23f ),
            ( WeenieClassName.mug,            0.00f ),
            ( WeenieClassName.ornamentalbowl, 0.19f ),
            ( WeenieClassName.dinnerplate,    0.13f ),
            ( WeenieClassName.stoup,          0.00f ),
            ( WeenieClassName.tankard,        0.00f ),
        };

        private static List<ChanceTable<WeenieClassName>> tierChances = new List<ChanceTable<WeenieClassName>>()
        {
            T1_T2_Chances,
            T1_T2_Chances,
            T3_T4_Chances,
            T3_T4_Chances,
            T5_T6_Chances,
            T5_T6_Chances,
        };

        public static WeenieClassName Roll(int tier)
        {
            // todo: add unique profiles for t7 / t8?
            tier = Math.Clamp(tier, 1, 6);

            return tierChances[tier - 1].Roll();
        }

        private static readonly HashSet<WeenieClassName> _combined = new HashSet<WeenieClassName>();

        static GenericWcids()
        {
            foreach (var tierChance in tierChances)
            {
                foreach (var entry in tierChance)
                    _combined.Add(entry.result);
            }
        }

        public static bool Contains(WeenieClassName wcid)
        {
            return _combined.Contains(wcid);
        }
    }
}
