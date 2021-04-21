using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class SpearWcids
    {
        private static ChanceTable<WeenieClassName> SpearWcids_Aluvian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.spear,              0.16f ),
            ( WeenieClassName.spearacid,          0.04f ),
            ( WeenieClassName.spearelectric,      0.04f ),
            ( WeenieClassName.spearflame,         0.04f ),
            ( WeenieClassName.spearfrost,         0.04f ),
            ( WeenieClassName.trident,            0.16f ),
            ( WeenieClassName.tridentacid,        0.04f ),
            ( WeenieClassName.tridentelectric,    0.04f ),
            ( WeenieClassName.tridentfire,        0.04f ),
            ( WeenieClassName.tridentfrost,       0.04f ),
            ( WeenieClassName.swordstaff,         0.16f ),
            ( WeenieClassName.swordstaffacid,     0.05f ),
            ( WeenieClassName.swordstaffelectric, 0.05f ),
            ( WeenieClassName.swordstafffire,     0.05f ),
            ( WeenieClassName.swordstafffrost,    0.05f ),
        };

        private static ChanceTable<WeenieClassName> SpearWcids_Gharundim = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.budiaq,             0.16f ),
            ( WeenieClassName.budiaqacid,         0.04f ),
            ( WeenieClassName.budiaqelectric,     0.04f ),
            ( WeenieClassName.budiaqfire,         0.04f ),
            ( WeenieClassName.budiaqfrost,        0.04f ),
            ( WeenieClassName.trident,            0.16f ),
            ( WeenieClassName.tridentacid,        0.04f ),
            ( WeenieClassName.tridentelectric,    0.04f ),
            ( WeenieClassName.tridentfire,        0.04f ),
            ( WeenieClassName.tridentfrost,       0.04f ),
            ( WeenieClassName.swordstaff,         0.16f ),
            ( WeenieClassName.swordstaffacid,     0.05f ),
            ( WeenieClassName.swordstaffelectric, 0.05f ),
            ( WeenieClassName.swordstafffire,     0.05f ),
            ( WeenieClassName.swordstafffrost,    0.05f ),
        };

        private static ChanceTable<WeenieClassName> SpearWcids_Sho = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.yari,               0.16f ),
            ( WeenieClassName.yariacid,           0.04f ),
            ( WeenieClassName.yarielectric,       0.04f ),
            ( WeenieClassName.yarifire,           0.04f ),
            ( WeenieClassName.yarifrost,          0.04f ),
            ( WeenieClassName.trident,            0.16f ),
            ( WeenieClassName.tridentacid,        0.04f ),
            ( WeenieClassName.tridentelectric,    0.04f ),
            ( WeenieClassName.tridentfire,        0.04f ),
            ( WeenieClassName.tridentfrost,       0.04f ),
            ( WeenieClassName.swordstaff,         0.16f ),
            ( WeenieClassName.swordstaffacid,     0.05f ),
            ( WeenieClassName.swordstaffelectric, 0.05f ),
            ( WeenieClassName.swordstafffire,     0.05f ),
            ( WeenieClassName.swordstafffrost,    0.05f ),
        };

        public static WeenieClassName Roll(TreasureHeritageGroup heritage)
        {
            switch (heritage)
            {
                case TreasureHeritageGroup.Aluvian:
                    return SpearWcids_Aluvian.Roll();

                case TreasureHeritageGroup.Gharundim:
                    return SpearWcids_Gharundim.Roll();

                case TreasureHeritageGroup.Sho:
                    return SpearWcids_Sho.Roll();
            }
            return WeenieClassName.undef;
        }
    }
}
