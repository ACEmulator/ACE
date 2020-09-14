using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class LockpickWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.lockpickplain,    0.75f ),
            ( WeenieClassName.lockpickreliable, 0.25f ),
            ( WeenieClassName.lockpickgood,     0.00f ),
            ( WeenieClassName.lockpickexcell,   0.00f ),
            ( WeenieClassName.lockpicksuperb,   0.00f ),
            ( WeenieClassName.lockpickpeer,     0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.lockpickplain,    0.25f ),
            ( WeenieClassName.lockpickreliable, 0.50f ),
            ( WeenieClassName.lockpickgood,     0.25f ),
            ( WeenieClassName.lockpickexcell,   0.00f ),
            ( WeenieClassName.lockpicksuperb,   0.00f ),
            ( WeenieClassName.lockpickpeer,     0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.lockpickplain,    0.00f ),
            ( WeenieClassName.lockpickreliable, 0.25f ),
            ( WeenieClassName.lockpickgood,     0.50f ),
            ( WeenieClassName.lockpickexcell,   0.25f ),
            ( WeenieClassName.lockpicksuperb,   0.00f ),
            ( WeenieClassName.lockpickpeer,     0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.lockpickplain,    0.00f ),
            ( WeenieClassName.lockpickreliable, 0.00f ),
            ( WeenieClassName.lockpickgood,     0.25f ),
            ( WeenieClassName.lockpickexcell,   0.50f ),
            ( WeenieClassName.lockpicksuperb,   0.25f ),
            ( WeenieClassName.lockpickpeer,     0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.lockpickplain,    0.00f ),
            ( WeenieClassName.lockpickreliable, 0.00f ),
            ( WeenieClassName.lockpickgood,     0.00f ),
            ( WeenieClassName.lockpickexcell,   0.25f ),
            ( WeenieClassName.lockpicksuperb,   0.50f ),
            ( WeenieClassName.lockpickpeer,     0.25f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.lockpickplain,    0.00f ),
            ( WeenieClassName.lockpickreliable, 0.00f ),
            ( WeenieClassName.lockpickgood,     0.00f ),
            ( WeenieClassName.lockpickexcell,   0.00f ),
            ( WeenieClassName.lockpicksuperb,   0.25f ),
            ( WeenieClassName.lockpickpeer,     0.75f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> lockpickTiers = new List<ChanceTable<WeenieClassName>>()
        {
            T1_Chances,
            T2_Chances,
            T3_Chances,
            T4_Chances,
            T5_Chances,
            T6_Chances,
        };

        public static WeenieClassName Roll(int tier)
        {
            // todo: add unique profiles for t7 / t8?
            tier = Math.Clamp(tier, 1, 6);

            return lockpickTiers[tier - 1].Roll();
        }
    }
}
