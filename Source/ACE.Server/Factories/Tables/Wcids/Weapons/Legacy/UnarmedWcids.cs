using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class UnarmedWcids
    {
        private static readonly ChanceTable<WeenieClassName> UnarmedWcids_Aluvian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.cestus,         0.40f ),
            ( WeenieClassName.cestusacid,     0.15f ),
            ( WeenieClassName.cestuselectric, 0.15f ),
            ( WeenieClassName.cestusfire,     0.15f ),
            ( WeenieClassName.cestusfrost,    0.15f ),
        };

        private static readonly ChanceTable<WeenieClassName> UnarmedWcids_Gharundim = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.katar,         0.40f ),
            ( WeenieClassName.kataracid,     0.15f ),
            ( WeenieClassName.katarelectric, 0.15f ),
            ( WeenieClassName.katarfire,     0.15f ),
            ( WeenieClassName.katarfrost,    0.15f ),
        };

        private static readonly ChanceTable<WeenieClassName> UnarmedWcids_Sho = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.nekode,         0.40f ),
            ( WeenieClassName.nekodeacid,     0.15f ),
            ( WeenieClassName.nekodeelectric, 0.15f ),
            ( WeenieClassName.nekodefire,     0.15f ),
            ( WeenieClassName.nekodefrost,    0.15f ),
        };

        public static WeenieClassName Roll(HeritageGroup heritage)
        {
            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                    return UnarmedWcids_Aluvian.Roll();

                case HeritageGroup.Gharundim:
                    return UnarmedWcids_Gharundim.Roll();

                case HeritageGroup.Sho:
                    return UnarmedWcids_Sho.Roll();
            }
            return WeenieClassName.undef;
        }
    }
}
