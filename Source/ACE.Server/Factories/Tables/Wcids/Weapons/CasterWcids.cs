using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class CasterWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_T3_Chances = new ChanceTable<WeenieClassName>()
        {
            (WeenieClassName.orb,          0.25f ),
            (WeenieClassName.sceptre,      0.25f ),
            (WeenieClassName.staff,        0.25f ),
            (WeenieClassName.wand,         0.25f ),
            (WeenieClassName.wandslashing, 0.00f ),
            (WeenieClassName.wandpiercing, 0.00f ),
            (WeenieClassName.wandblunt,    0.00f ),
            (WeenieClassName.wandacid,     0.00f ),
            (WeenieClassName.wandfire,     0.00f ),
            (WeenieClassName.wandfrost,    0.00f ),
            (WeenieClassName.wandelectric, 0.00f ),
        };

        private static readonly ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.orb,          0.12f ),
            ( WeenieClassName.sceptre,      0.13f ),
            ( WeenieClassName.staff,        0.13f ),
            ( WeenieClassName.wand,         0.13f ),
            ( WeenieClassName.wandslashing, 0.07f ),
            ( WeenieClassName.wandpiercing, 0.07f ),
            ( WeenieClassName.wandblunt,    0.07f ),
            ( WeenieClassName.wandacid,     0.07f ),
            ( WeenieClassName.wandfire,     0.07f ),
            ( WeenieClassName.wandfrost,    0.07f ),
            ( WeenieClassName.wandelectric, 0.07f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.orb,          0.05f ),
            ( WeenieClassName.sceptre,      0.06f ),
            ( WeenieClassName.staff,        0.06f ),
            ( WeenieClassName.wand,         0.06f ),
            ( WeenieClassName.wandslashing, 0.11f ),
            ( WeenieClassName.wandpiercing, 0.11f ),
            ( WeenieClassName.wandblunt,    0.11f ),
            ( WeenieClassName.wandacid,     0.11f ),
            ( WeenieClassName.wandfire,     0.11f ),
            ( WeenieClassName.wandfrost,    0.11f ),
            ( WeenieClassName.wandelectric, 0.11f ),
        };

        private static readonly ChanceTable<WeenieClassName> T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.orb,          0.05f ),
            ( WeenieClassName.sceptre,      0.05f ),
            ( WeenieClassName.staff,        0.05f ),
            ( WeenieClassName.wand,         0.05f ),
            ( WeenieClassName.wandslashing, 0.12f ),
            ( WeenieClassName.wandpiercing, 0.12f ),
            ( WeenieClassName.wandblunt,    0.12f ),
            ( WeenieClassName.wandacid,     0.11f ),
            ( WeenieClassName.wandfire,     0.11f ),
            ( WeenieClassName.wandfrost,    0.11f ),
            ( WeenieClassName.wandelectric, 0.11f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> casterTiers = new List<ChanceTable<WeenieClassName>>()
        {
            T1_T3_Chances,
            T1_T3_Chances,
            T1_T3_Chances,
            T4_Chances,
            T5_Chances,
            T6_Chances,
        };

        public static WeenieClassName Roll(int tier)
        {
            // todo: add unique profiles for t7 / t8?
            tier = Math.Clamp(tier, 1, 6);

            return casterTiers[tier - 1].Roll();
        }
    }
}
