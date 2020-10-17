using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class AtlatlWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.atlatl,      0.50f ),
            ( WeenieClassName.atlatlroyal, 0.50f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.atlatl,                     0.25f ),
            ( WeenieClassName.atlatlroyal,                0.26f ),
            ( WeenieClassName.atlatlslashing,             0.035f ),
            ( WeenieClassName.atlatlpiercing,             0.035f ),
            ( WeenieClassName.atlatlblunt,                0.035f ),
            ( WeenieClassName.atlatlacid,                 0.035f ),
            ( WeenieClassName.atlatlfire,                 0.035f ),
            ( WeenieClassName.atlatlfrost,                0.035f ),
            ( WeenieClassName.atlatlelectric,             0.035f ),
            ( WeenieClassName.ace31812_slashingslingshot, 0.035f ),
            ( WeenieClassName.ace31818_piercingslingshot, 0.035f ),
            ( WeenieClassName.ace31814_bluntslingshot,    0.035f ),
            ( WeenieClassName.ace31813_acidslingshot,     0.035f ),
            ( WeenieClassName.ace31816_fireslingshot,     0.035f ),
            ( WeenieClassName.ace31817_frostslingshot,    0.035f ),
            ( WeenieClassName.ace31815_electricslingshot, 0.035f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_T8_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.atlatlslashing,             0.075f ),
            ( WeenieClassName.atlatlpiercing,             0.075f ),
            ( WeenieClassName.atlatlblunt,                0.07f ),
            ( WeenieClassName.atlatlacid,                 0.07f ),
            ( WeenieClassName.atlatlfire,                 0.07f ),
            ( WeenieClassName.atlatlfrost,                0.07f ),
            ( WeenieClassName.atlatlelectric,             0.07f ),
            ( WeenieClassName.ace31812_slashingslingshot, 0.075f ),
            ( WeenieClassName.ace31818_piercingslingshot, 0.075f ),
            ( WeenieClassName.ace31814_bluntslingshot,    0.07f ),
            ( WeenieClassName.ace31813_acidslingshot,     0.07f ),
            ( WeenieClassName.ace31816_fireslingshot,     0.07f ),
            ( WeenieClassName.ace31817_frostslingshot,    0.07f ),
            ( WeenieClassName.ace31815_electricslingshot, 0.07f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> atlatlTiers = new List<ChanceTable<WeenieClassName>>()
        {
            T1_T4_Chances,
            T1_T4_Chances,
            T1_T4_Chances,
            T1_T4_Chances,
            T5_Chances,
            T6_T8_Chances,
            T6_T8_Chances,
            T6_T8_Chances,
        };

        public static WeenieClassName Roll(int tier)
        {
            return atlatlTiers[tier - 1].Roll();
        }
    }
}
