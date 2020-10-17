using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class CrossbowWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.crossbowlight,    0.50f ),
            ( WeenieClassName.crossbowheavy,    0.25f ),
            ( WeenieClassName.crossbowarbalest, 0.25f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.crossbowlight,                     0.25f ),
            ( WeenieClassName.crossbowheavy,                     0.13f ),
            ( WeenieClassName.crossbowarbalest,                  0.13f ),
            ( WeenieClassName.crossbowslashing,                  0.035f ),
            ( WeenieClassName.crossbowpiercing,                  0.035f ),
            ( WeenieClassName.crossbowblunt,                     0.035f ),
            ( WeenieClassName.crossbowacid,                      0.035f ),
            ( WeenieClassName.crossbowfire,                      0.035f ),
            ( WeenieClassName.crossbowfrost,                     0.035f ),
            ( WeenieClassName.crossbowelectric,                  0.035f ),
            ( WeenieClassName.ace31805_slashingcompoundcrossbow, 0.035f ),
            ( WeenieClassName.ace31811_piercingcompoundcrossbow, 0.035f ),
            ( WeenieClassName.ace31807_bluntcompoundcrossbow,    0.035f ),
            ( WeenieClassName.ace31806_acidcompoundcrossbow,     0.035f ),
            ( WeenieClassName.ace31809_firecompoundcrossbow,     0.035f ),
            ( WeenieClassName.ace31810_frostcompoundcrossbow,    0.035f ),
            ( WeenieClassName.ace31808_electriccompoundcrossbow, 0.035f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_T8_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.crossbowslashing,                  0.075f ),
            ( WeenieClassName.crossbowpiercing,                  0.075f ),
            ( WeenieClassName.crossbowblunt,                     0.07f ),
            ( WeenieClassName.crossbowacid,                      0.07f ),
            ( WeenieClassName.crossbowfire,                      0.07f ),
            ( WeenieClassName.crossbowfrost,                     0.07f ),
            ( WeenieClassName.crossbowelectric,                  0.07f ),
            ( WeenieClassName.ace31805_slashingcompoundcrossbow, 0.075f ),
            ( WeenieClassName.ace31811_piercingcompoundcrossbow, 0.075f ),
            ( WeenieClassName.ace31807_bluntcompoundcrossbow,    0.07f ),
            ( WeenieClassName.ace31806_acidcompoundcrossbow,     0.07f ),
            ( WeenieClassName.ace31809_firecompoundcrossbow,     0.07f ),
            ( WeenieClassName.ace31810_frostcompoundcrossbow,    0.07f ),
            ( WeenieClassName.ace31808_electriccompoundcrossbow, 0.07f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> crossbowTiers = new List<ChanceTable<WeenieClassName>>()
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
            return crossbowTiers[tier - 1].Roll();
        }
    }
}
