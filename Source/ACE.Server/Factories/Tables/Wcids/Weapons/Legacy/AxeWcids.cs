using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class AxeWcids
    {
        private static ChanceTable<WeenieClassName> AxeWcids_Aluvian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.axehand,           0.16f ),
            ( WeenieClassName.axehandacid,       0.04f ),
            ( WeenieClassName.axehandelectric,   0.04f ),
            ( WeenieClassName.axehandfire,       0.04f ),
            ( WeenieClassName.axehandfrost,      0.04f ),
            ( WeenieClassName.axebattle,         0.16f ),
            ( WeenieClassName.axebattleacid,     0.04f ),
            ( WeenieClassName.axebattleelectric, 0.04f ),
            ( WeenieClassName.axebattlefire,     0.04f ),
            ( WeenieClassName.axebattlefrost,    0.04f ),
            ( WeenieClassName.warhammer,         0.16f ),
            ( WeenieClassName.warhammeracid,     0.05f ),
            ( WeenieClassName.warhammerelectric, 0.05f ),
            ( WeenieClassName.warhammerfire,     0.05f ),
            ( WeenieClassName.warhammerfrost,    0.05f ),
        };

        private static ChanceTable<WeenieClassName> AxeWcids_Gharundim = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.tungi,             0.16f ),
            ( WeenieClassName.tungiacid,         0.04f ),
            ( WeenieClassName.tungielectric,     0.04f ),
            ( WeenieClassName.tungifire,         0.04f ),
            ( WeenieClassName.tungifrost,        0.04f ),
            ( WeenieClassName.silifi,            0.16f ),
            ( WeenieClassName.silifiacid,        0.04f ),
            ( WeenieClassName.silifielectric,    0.04f ),
            ( WeenieClassName.silififire,        0.04f ),
            ( WeenieClassName.silififrost,       0.04f ),
            ( WeenieClassName.warhammer,         0.16f ),
            ( WeenieClassName.warhammeracid,     0.05f ),
            ( WeenieClassName.warhammerelectric, 0.05f ),
            ( WeenieClassName.warhammerfire,     0.05f ),
            ( WeenieClassName.warhammerfrost,    0.05f ),
        };

        private static ChanceTable<WeenieClassName> AxeWcids_Sho = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shouono,           0.16f ),
            ( WeenieClassName.shouonoacid,       0.04f ),
            ( WeenieClassName.shouonoelectric,   0.04f ),
            ( WeenieClassName.shouonofire,       0.04f ),
            ( WeenieClassName.shouonofrost,      0.04f ),
            ( WeenieClassName.ono,               0.16f ),
            ( WeenieClassName.onoacid,           0.04f ),
            ( WeenieClassName.onoelectric,       0.04f ),
            ( WeenieClassName.onofire,           0.04f ),
            ( WeenieClassName.onofrost,          0.04f ),
            ( WeenieClassName.warhammer,         0.16f ),
            ( WeenieClassName.warhammeracid,     0.05f ),
            ( WeenieClassName.warhammerelectric, 0.05f ),
            ( WeenieClassName.warhammerfire,     0.05f ),
            ( WeenieClassName.warhammerfrost,    0.05f ),
        };

        public static WeenieClassName Roll(TreasureHeritageGroup heritage)
        {
            switch (heritage)
            {
                case TreasureHeritageGroup.Aluvian:
                    return AxeWcids_Aluvian.Roll();

                case TreasureHeritageGroup.Gharundim:
                    return AxeWcids_Gharundim.Roll();

                case TreasureHeritageGroup.Sho:
                    return AxeWcids_Sho.Roll();
            }
            return WeenieClassName.undef;
        }
    }
}
