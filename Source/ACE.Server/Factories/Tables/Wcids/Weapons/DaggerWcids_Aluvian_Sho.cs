using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class DaggerWcids_Aluvian_Sho
    {
        private static readonly ChanceTable<WeenieClassName> T1_T3_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.knife,          0.16f ),
            ( WeenieClassName.knifeacid,      0.04f ),
            ( WeenieClassName.knifeelectric,  0.04f ),
            ( WeenieClassName.knifefire,      0.04f ),
            ( WeenieClassName.knifefrost,     0.04f ),
            ( WeenieClassName.dagger,         0.16f ),
            ( WeenieClassName.daggeracid,     0.04f ),
            ( WeenieClassName.daggerelectric, 0.04f ),
            ( WeenieClassName.daggerfire,     0.04f ),
            ( WeenieClassName.daggerfrost,    0.04f ),
            ( WeenieClassName.dirk,           0.16f ),
            ( WeenieClassName.dirkacid,       0.05f ),
            ( WeenieClassName.dirkelectric,   0.05f ),
            ( WeenieClassName.dirkfire,       0.05f ),
            ( WeenieClassName.dirkfrost,      0.05f ),
        };

        private static readonly ChanceTable<WeenieClassName> T4_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.knife,          0.06f ),
            ( WeenieClassName.knifeacid,      0.01f ),
            ( WeenieClassName.knifeelectric,  0.01f ),
            ( WeenieClassName.knifefire,      0.01f ),
            ( WeenieClassName.knifefrost,     0.01f ),
            ( WeenieClassName.dagger,         0.06f ),
            ( WeenieClassName.daggeracid,     0.01f ),
            ( WeenieClassName.daggerelectric, 0.01f ),
            ( WeenieClassName.daggerfire,     0.01f ),
            ( WeenieClassName.daggerfrost,    0.01f ),
            ( WeenieClassName.dirk,           0.40f ),
            ( WeenieClassName.dirkacid,       0.10f ),
            ( WeenieClassName.dirkelectric,   0.10f ),
            ( WeenieClassName.dirkfire,       0.10f ),
            ( WeenieClassName.dirkfrost,      0.10f ),
        };

        private static readonly ChanceTable<WeenieClassName> T5_T6_Chances = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.knife,          0.01f ),
            ( WeenieClassName.knifeacid,      0.01f ),
            ( WeenieClassName.knifeelectric,  0.01f ),
            ( WeenieClassName.knifefire,      0.01f ),
            ( WeenieClassName.knifefrost,     0.01f ),
            ( WeenieClassName.dagger,         0.01f ),
            ( WeenieClassName.daggeracid,     0.01f ),
            ( WeenieClassName.daggerelectric, 0.01f ),
            ( WeenieClassName.daggerfire,     0.01f ),
            ( WeenieClassName.daggerfrost,    0.01f ),
            ( WeenieClassName.dirk,           0.42f ),
            ( WeenieClassName.dirkacid,       0.12f ),
            ( WeenieClassName.dirkelectric,   0.12f ),
            ( WeenieClassName.dirkfire,       0.12f ),
            ( WeenieClassName.dirkfrost,      0.12f ),
        };
    }
}
