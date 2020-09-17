using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class JewelryWcids
    {
        private static readonly ChanceTable<WeenieClassName> T1_T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.amulet,        0.10f ),
            ( WeenieClassName.bracelet,      0.30f ),
            ( WeenieClassName.braceletheavy, 0.10f ),
            ( WeenieClassName.crown,         0.00f ),
            ( WeenieClassName.gorget,        0.00f ),
            ( WeenieClassName.necklace,      0.20f ),
            ( WeenieClassName.necklaceheavy, 0.00f ),
            ( WeenieClassName.ring,          0.27f ),
            ( WeenieClassName.ringjeweled,   0.03f ),
        };

        private static readonly ChanceTable<WeenieClassName> T3_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.amulet,        0.10f ),
            ( WeenieClassName.bracelet,      0.15f ),
            ( WeenieClassName.braceletheavy, 0.15f ),
            ( WeenieClassName.crown,         0.00f ),
            ( WeenieClassName.gorget,        0.10f ),
            ( WeenieClassName.necklace,      0.15f ),
            ( WeenieClassName.necklaceheavy, 0.05f ),
            ( WeenieClassName.ring,          0.15f ),
            ( WeenieClassName.ringjeweled,   0.15f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.amulet,        0.05f ),
            ( WeenieClassName.bracelet,      0.05f ),
            ( WeenieClassName.braceletheavy, 0.20f ),
            ( WeenieClassName.crown,         0.10f ),
            ( WeenieClassName.gorget,        0.10f ),
            ( WeenieClassName.necklace,      0.05f ),
            ( WeenieClassName.necklaceheavy, 0.15f ),
            ( WeenieClassName.ring,          0.06f ),
            ( WeenieClassName.ringjeweled,   0.24f ),
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
    }
}
