using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class ClothingWcids
    {
        private static readonly ChanceTable<WeenieClassName> Clothing1 = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shirtbaggy,   0.06f ),
            ( WeenieClassName.tunicbaggy,   0.06f ),
            ( WeenieClassName.capcloth,     0.07f ),
            ( WeenieClassName.cowlcloth,    0.07f ),
            ( WeenieClassName.doublet,      0.05f ),
            ( WeenieClassName.glovescloth,  0.08f ),
            ( WeenieClassName.pants,        0.08f ),
            ( WeenieClassName.shirt,        0.06f ),
            ( WeenieClassName.shoes,        0.20f ),
            ( WeenieClassName.smock,        0.06f ),
            ( WeenieClassName.trousers,     0.08f ),
            ( WeenieClassName.tunic,        0.06f ),
            ( WeenieClassName.breecheswide, 0.07f ),
        };

        private static readonly ChanceTable<WeenieClassName> Clothing2 = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.breechesbaggy, 0.07f ),
            ( WeenieClassName.pantsbaggy,    0.07f ),
            ( WeenieClassName.tunicbaggy,    0.06f ),
            ( WeenieClassName.capfez,        0.07f ),
            ( WeenieClassName.glovescloth,   0.09f ),
            ( WeenieClassName.jerkin,        0.06f ),
            ( WeenieClassName.shirtloose,    0.05f ),
            ( WeenieClassName.pantaloons,    0.07f ),
            ( WeenieClassName.shirtpuffy,    0.05f ),
            ( WeenieClassName.tunicpuffy,    0.06f ),
            ( WeenieClassName.qafiya,        0.06f ),
            ( WeenieClassName.sandals,       0.05f ),
            ( WeenieClassName.shoes,         0.06f ),
            ( WeenieClassName.slippers,      0.06f ),
            ( WeenieClassName.smock,         0.05f ),
            ( WeenieClassName.turban,        0.07f ),
        };
    }
}
