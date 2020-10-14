using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class BowWcids_Sho
    {
        private static readonly ChanceTable<WeenieClassName> T1_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bowshort, 0.50f ),
            ( WeenieClassName.shouyumi, 0.25f ),
            ( WeenieClassName.yumi,     0.25f )
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bowshort,                     0.25f ),
            ( WeenieClassName.shouyumi,                     0.13f ),
            ( WeenieClassName.yumi,                         0.13f ),
            ( WeenieClassName.bowslashing,                  0.035f ),
            ( WeenieClassName.bowpiercing,                  0.035f ),
            ( WeenieClassName.bowblunt,                     0.035f ),
            ( WeenieClassName.bowacid,                      0.035f ),
            ( WeenieClassName.bowfire,                      0.035f ),
            ( WeenieClassName.bowfrost,                     0.035f ),
            ( WeenieClassName.bowelectric,                  0.035f ),
            ( WeenieClassName.ace31798_slashingcompoundbow, 0.035f ),
            ( WeenieClassName.ace31804_piercingcompoundbow, 0.035f ),
            ( WeenieClassName.ace31800_bluntcompoundbow,    0.035f ),
            ( WeenieClassName.ace31799_acidcompoundbow,     0.035f ),
            ( WeenieClassName.ace31802_firecompoundbow,     0.035f ),
            ( WeenieClassName.ace31803_frostcompoundbow,    0.035f ),
            ( WeenieClassName.ace31801_electriccompoundbow, 0.035f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_T8_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.bowslashing,                  0.075f ),
            ( WeenieClassName.bowpiercing,                  0.075f ),
            ( WeenieClassName.bowblunt,                     0.07f ),
            ( WeenieClassName.bowacid,                      0.07f ),
            ( WeenieClassName.bowfire,                      0.07f ),
            ( WeenieClassName.bowfrost,                     0.07f ),
            ( WeenieClassName.bowelectric,                  0.07f ),
            ( WeenieClassName.ace31798_slashingcompoundbow, 0.075f ),
            ( WeenieClassName.ace31804_piercingcompoundbow, 0.075f ),
            ( WeenieClassName.ace31800_bluntcompoundbow,    0.07f ),
            ( WeenieClassName.ace31799_acidcompoundbow,     0.07f ),
            ( WeenieClassName.ace31802_firecompoundbow,     0.07f ),
            ( WeenieClassName.ace31803_frostcompoundbow,    0.07f ),
            ( WeenieClassName.ace31801_electriccompoundbow, 0.07f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> bowTiers = new List<ChanceTable<WeenieClassName>>()
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
            return bowTiers[tier - 1].Roll();
        }
    }
}
