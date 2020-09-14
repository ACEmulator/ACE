using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class ManaStoneWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.manastoneminor,   0.75f ),
            ( WeenieClassName.manastonelesser,  0.25f ),
            ( WeenieClassName.manastone,        0.00f ),
            ( WeenieClassName.manastonemedium,  0.00f ),
            ( WeenieClassName.manastonegreater, 0.00f ),
            ( WeenieClassName.manastonemajor,   0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.manastoneminor,   0.25f ),
            ( WeenieClassName.manastonelesser,  0.50f ),
            ( WeenieClassName.manastone,        0.25f ),
            ( WeenieClassName.manastonemedium,  0.00f ),
            ( WeenieClassName.manastonegreater, 0.00f ),
            ( WeenieClassName.manastonemajor,   0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.manastoneminor,   0.00f ),
            ( WeenieClassName.manastonelesser,  0.25f ),
            ( WeenieClassName.manastone,        0.50f ),
            ( WeenieClassName.manastonemedium,  0.25f ),
            ( WeenieClassName.manastonegreater, 0.00f ),
            ( WeenieClassName.manastonemajor,   0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.manastoneminor,   0.00f ),
            ( WeenieClassName.manastonelesser,  0.00f ),
            ( WeenieClassName.manastone,        0.25f ),
            ( WeenieClassName.manastonemedium,  0.50f ),
            ( WeenieClassName.manastonegreater, 0.25f ),
            ( WeenieClassName.manastonemajor,   0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.manastoneminor,   0.00f ),
            ( WeenieClassName.manastonelesser,  0.00f ),
            ( WeenieClassName.manastone,        0.00f ),
            ( WeenieClassName.manastonemedium,  0.25f ),
            ( WeenieClassName.manastonegreater, 0.50f ),
            ( WeenieClassName.manastonemajor,   0.25f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.manastoneminor,   0.00f ),
            ( WeenieClassName.manastonelesser,  0.00f ),
            ( WeenieClassName.manastone,        0.00f ),
            ( WeenieClassName.manastonemedium,  0.00f ),
            ( WeenieClassName.manastonegreater, 0.25f ),
            ( WeenieClassName.manastonemajor,   0.75f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> manaStoneTiers = new List<ChanceTable<WeenieClassName>>()
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

            return manaStoneTiers[tier - 1].Roll();
        }
    }
}
