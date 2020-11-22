using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class UnarmedWcids
    {
        private static ChanceTable<WeenieClassName> UnarmedWcids_Aluvian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.cestus,         0.40f ),
            ( WeenieClassName.cestusacid,     0.15f ),
            ( WeenieClassName.cestuselectric, 0.15f ),
            ( WeenieClassName.cestusfire,     0.15f ),
            ( WeenieClassName.cestusfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> UnarmedWcids_Gharundim = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.katar,         0.40f ),
            ( WeenieClassName.kataracid,     0.15f ),
            ( WeenieClassName.katarelectric, 0.15f ),
            ( WeenieClassName.katarfire,     0.15f ),
            ( WeenieClassName.katarfrost,    0.15f ),
        };

        private static ChanceTable<WeenieClassName> UnarmedWcids_Sho = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.nekode,         0.40f ),
            ( WeenieClassName.nekodeacid,     0.15f ),
            ( WeenieClassName.nekodeelectric, 0.15f ),
            ( WeenieClassName.nekodefire,     0.15f ),
            ( WeenieClassName.nekodefrost,    0.15f ),
        };

        public static WeenieClassName Roll(TreasureHeritageGroup heritage)
        {
            switch (heritage)
            {
                case TreasureHeritageGroup.Aluvian:
                    return UnarmedWcids_Aluvian.Roll();

                case TreasureHeritageGroup.Gharundim:
                    return UnarmedWcids_Gharundim.Roll();

                case TreasureHeritageGroup.Sho:
                    return UnarmedWcids_Sho.Roll();
            }
            return WeenieClassName.undef;
        }
    }
}
