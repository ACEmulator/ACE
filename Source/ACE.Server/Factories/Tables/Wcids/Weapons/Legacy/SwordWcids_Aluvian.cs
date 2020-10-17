using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class SwordWcids_Aluvian
    {
        private static ChanceTable<WeenieClassName> T1_T2_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.swordrapier,        0.10f ),
            ( WeenieClassName.swordshort,         0.12f ),
            ( WeenieClassName.swordshortacid,     0.03f ),
            ( WeenieClassName.swordshortelectric, 0.03f ),
            ( WeenieClassName.swordshortfire,     0.03f ),
            ( WeenieClassName.swordshortfrost,    0.03f ),
            ( WeenieClassName.scimitar,           0.10f ),
            ( WeenieClassName.scimitaracid,       0.03f ),
            ( WeenieClassName.scimitarelectric,   0.03f ),
            ( WeenieClassName.scimitarfire,       0.03f ),
            ( WeenieClassName.scimitarfrost,      0.03f ),
            ( WeenieClassName.swordlong,          0.10f ),
            ( WeenieClassName.swordlongacid,      0.03f ),
            ( WeenieClassName.swordlongelectric,  0.03f ),
            ( WeenieClassName.swordlongfire,      0.03f ),
            ( WeenieClassName.swordlongfrost,     0.03f ),
            ( WeenieClassName.swordbroad,         0.10f ),
            ( WeenieClassName.swordbroadacid,     0.03f ),
            ( WeenieClassName.swordbroadelectric, 0.03f ),
            ( WeenieClassName.swordbroadfire,     0.03f ),
            ( WeenieClassName.swordbroadfrost,    0.03f ),
        };

        private static ChanceTable<WeenieClassName> T3_T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.swordrapier,        0.06f ),
            ( WeenieClassName.swordshort,         0.06f ),
            ( WeenieClassName.swordshortacid,     0.01f ),
            ( WeenieClassName.swordshortelectric, 0.01f ),
            ( WeenieClassName.swordshortfire,     0.01f ),
            ( WeenieClassName.swordshortfrost,    0.01f ),
            ( WeenieClassName.scimitar,           0.12f ),
            ( WeenieClassName.scimitaracid,       0.04f ),
            ( WeenieClassName.scimitarelectric,   0.04f ),
            ( WeenieClassName.scimitarfire,       0.04f ),
            ( WeenieClassName.scimitarfrost,      0.04f ),
            ( WeenieClassName.swordlong,          0.12f ),
            ( WeenieClassName.swordlongacid,      0.04f ),
            ( WeenieClassName.swordlongelectric,  0.04f ),
            ( WeenieClassName.swordlongfire,      0.04f ),
            ( WeenieClassName.swordlongfrost,     0.04f ),
            ( WeenieClassName.swordbroad,         0.12f ),
            ( WeenieClassName.swordbroadacid,     0.04f ),
            ( WeenieClassName.swordbroadelectric, 0.04f ),
            ( WeenieClassName.swordbroadfire,     0.04f ),
            ( WeenieClassName.swordbroadfrost,    0.04f ),
        };

        private static ChanceTable<WeenieClassName> T5_T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.swordrapier,        0.01f ),
            ( WeenieClassName.swordshort,         0.01f ),
            ( WeenieClassName.swordshortacid,     0.01f ),
            ( WeenieClassName.swordshortelectric, 0.01f ),
            ( WeenieClassName.swordshortfire,     0.01f ),
            ( WeenieClassName.swordshortfrost,    0.01f ),
            ( WeenieClassName.scimitar,           0.14f ),
            ( WeenieClassName.scimitaracid,       0.04f ),
            ( WeenieClassName.scimitarelectric,   0.04f ),
            ( WeenieClassName.scimitarfire,       0.04f ),
            ( WeenieClassName.scimitarfrost,      0.04f ),
            ( WeenieClassName.swordlong,          0.14f ),
            ( WeenieClassName.swordlongacid,      0.04f ),
            ( WeenieClassName.swordlongelectric,  0.04f ),
            ( WeenieClassName.swordlongfire,      0.04f ),
            ( WeenieClassName.swordlongfrost,     0.04f ),
            ( WeenieClassName.swordbroad,         0.14f ),
            ( WeenieClassName.swordbroadacid,     0.05f ),
            ( WeenieClassName.swordbroadelectric, 0.05f ),
            ( WeenieClassName.swordbroadfire,     0.05f ),
            ( WeenieClassName.swordbroadfrost,    0.05f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> weaponTiers = new List<ChanceTable<WeenieClassName>>()
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

            return weaponTiers[tier - 1].Roll();
        }
    }
}
