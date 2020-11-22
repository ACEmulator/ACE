using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class MaceWcids
    {
        private static ChanceTable<WeenieClassName> MaceWcids_Aluvian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.club,                0.13f ),
            ( WeenieClassName.clubacid,            0.03f ),
            ( WeenieClassName.clubelectric,        0.03f ),
            ( WeenieClassName.clubfire,            0.03f ),
            ( WeenieClassName.clubfrost,           0.03f ),
            ( WeenieClassName.mace,                0.13f ),
            ( WeenieClassName.maceacid,            0.03f ),
            ( WeenieClassName.maceelectric,        0.03f ),
            ( WeenieClassName.macefire,            0.03f ),
            ( WeenieClassName.macefrost,           0.03f ),
            ( WeenieClassName.morningstar,         0.13f ),
            ( WeenieClassName.morningstaracid,     0.03f ),
            ( WeenieClassName.morningstarelectric, 0.03f ),
            ( WeenieClassName.morningstarfire,     0.03f ),
            ( WeenieClassName.morningstarfrost,    0.03f ),
            ( WeenieClassName.clubspiked,          0.13f ),
            ( WeenieClassName.clubspikedacid,      0.03f ),
            ( WeenieClassName.clubspikedelectric,  0.03f ),
            ( WeenieClassName.clubspikedfire,      0.03f ),
            ( WeenieClassName.clubspikedfrost,     0.03f ),
        };

        private static ChanceTable<WeenieClassName> MaceWcids_Gharundim = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.kasrullah,           0.13f ),
            ( WeenieClassName.kasrullahacid,       0.03f ),
            ( WeenieClassName.kasrullahelectric,   0.03f ),
            ( WeenieClassName.kasrullahfire,       0.03f ),
            ( WeenieClassName.kasrullahfrost,      0.03f ),
            ( WeenieClassName.dabus,               0.13f ),
            ( WeenieClassName.dabusacid,           0.03f ),
            ( WeenieClassName.dabuselectric,       0.03f ),
            ( WeenieClassName.dabusfire,           0.03f ),
            ( WeenieClassName.dabusfrost,          0.03f ),
            ( WeenieClassName.morningstar,         0.13f ),
            ( WeenieClassName.morningstaracid,     0.03f ),
            ( WeenieClassName.morningstarelectric, 0.03f ),
            ( WeenieClassName.morningstarfire,     0.03f ),
            ( WeenieClassName.morningstarfrost,    0.03f ),
            ( WeenieClassName.clubspiked,          0.13f ),
            ( WeenieClassName.clubspikedacid,      0.03f ),
            ( WeenieClassName.clubspikedelectric,  0.03f ),
            ( WeenieClassName.clubspikedfire,      0.03f ),
            ( WeenieClassName.clubspikedfrost,     0.03f ),
        };

        private static ChanceTable<WeenieClassName> MaceWcids_Sho = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.jitte,               0.13f ),
            ( WeenieClassName.jitteacid,           0.03f ),
            ( WeenieClassName.jitteelectric,       0.03f ),
            ( WeenieClassName.jittefire,           0.03f ),
            ( WeenieClassName.jittefrost,          0.03f ),
            ( WeenieClassName.tofun,               0.13f ),
            ( WeenieClassName.tofunacid,           0.03f ),
            ( WeenieClassName.tofunelectric,       0.03f ),
            ( WeenieClassName.tofunfire,           0.03f ),
            ( WeenieClassName.tofunfrost,          0.03f ),
            ( WeenieClassName.morningstar,         0.13f ),
            ( WeenieClassName.morningstaracid,     0.03f ),
            ( WeenieClassName.morningstarelectric, 0.03f ),
            ( WeenieClassName.morningstarfire,     0.03f ),
            ( WeenieClassName.morningstarfrost,    0.03f ),
            ( WeenieClassName.clubspiked,          0.13f ),
            ( WeenieClassName.clubspikedacid,      0.03f ),
            ( WeenieClassName.clubspikedelectric,  0.03f ),
            ( WeenieClassName.clubspikedfire,      0.03f ),
            ( WeenieClassName.clubspikedfrost,     0.03f ),
        };

        public static WeenieClassName Roll(TreasureHeritageGroup heritage)
        {
            switch (heritage)
            {
                case TreasureHeritageGroup.Aluvian:
                    return MaceWcids_Aluvian.Roll();

                case TreasureHeritageGroup.Gharundim:
                    return MaceWcids_Gharundim.Roll();

                case TreasureHeritageGroup.Sho:
                    return MaceWcids_Sho.Roll();
            }
            return WeenieClassName.undef;
        }
    }
}
