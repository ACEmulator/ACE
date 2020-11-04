using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class DaggerWcids_Gharundim
    {
        private static ChanceTable<WeenieClassName> T1_T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.jambiya,         0.16f ),
            ( WeenieClassName.jambiyaacid,     0.04f ),
            ( WeenieClassName.jambiyaelectric, 0.04f ),
            ( WeenieClassName.jambiyafire,     0.04f ),
            ( WeenieClassName.jambiyafrost,    0.04f ),
            ( WeenieClassName.khanjar,         0.16f ),
            ( WeenieClassName.khanjaracid,     0.04f ),
            ( WeenieClassName.khanjarelectric, 0.04f ),
            ( WeenieClassName.khanjarfire,     0.04f ),
            ( WeenieClassName.khanjarfrost,    0.04f ),
            ( WeenieClassName.dirk,            0.16f ),
            ( WeenieClassName.dirkacid,        0.05f ),
            ( WeenieClassName.dirkelectric,    0.05f ),
            ( WeenieClassName.dirkfire,        0.05f ),
            ( WeenieClassName.dirkfrost,       0.05f ),
        };

        private static ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.jambiya,         0.06f ),
            ( WeenieClassName.jambiyaacid,     0.01f ),
            ( WeenieClassName.jambiyaelectric, 0.01f ),
            ( WeenieClassName.jambiyafire,     0.01f ),
            ( WeenieClassName.jambiyafrost,    0.01f ),
            ( WeenieClassName.khanjar,         0.06f ),
            ( WeenieClassName.khanjaracid,     0.01f ),
            ( WeenieClassName.khanjarelectric, 0.01f ),
            ( WeenieClassName.khanjarfire,     0.01f ),
            ( WeenieClassName.khanjarfrost,    0.01f ),
            ( WeenieClassName.dirk,            0.40f ),
            ( WeenieClassName.dirkacid,        0.10f ),
            ( WeenieClassName.dirkelectric,    0.10f ),
            ( WeenieClassName.dirkfire,        0.10f ),
            ( WeenieClassName.dirkfrost,       0.10f ),
        };

        private static ChanceTable<WeenieClassName> T5_T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.jambiya,         0.01f ),
            ( WeenieClassName.jambiyaacid,     0.01f ),
            ( WeenieClassName.jambiyaelectric, 0.01f ),
            ( WeenieClassName.jambiyafire,     0.01f ),
            ( WeenieClassName.jambiyafrost,    0.01f ),
            ( WeenieClassName.khanjar,         0.01f ),
            ( WeenieClassName.khanjaracid,     0.01f ),
            ( WeenieClassName.khanjarelectric, 0.01f ),
            ( WeenieClassName.khanjarfire,     0.01f ),
            ( WeenieClassName.khanjarfrost,    0.01f ),
            ( WeenieClassName.dirk,            0.42f ),
            ( WeenieClassName.dirkacid,        0.12f ),
            ( WeenieClassName.dirkelectric,    0.12f ),
            ( WeenieClassName.dirkfire,        0.12f ),
            ( WeenieClassName.dirkfrost,       0.12f ),
        };

        private static readonly List<ChanceTable<WeenieClassName>> weaponTiers = new List<ChanceTable<WeenieClassName>>()
        {
            T1_T3_Chances,
            T1_T3_Chances,
            T1_T3_Chances,
            T4_Chances,
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
