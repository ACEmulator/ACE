using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class ClothingWcids
    {
        // shirt: 35%
        // pants: 23%
        // shoes: 20%
        // hat:   14%
        // gloves: 8%
        private static readonly ChanceTable<WeenieClassName> ClothingWcids_Aluvian = new ChanceTable<WeenieClassName>()
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

        // shirt: 33%
        // pants: 21%
        // hat: 20%
        // shoes: 17%
        // gloves: 9%
        private static readonly ChanceTable<WeenieClassName> ClothingWcids_Gharundim = new ChanceTable<WeenieClassName>()
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

        // shirt: 34%
        // pants: 24%
        // shoes: 17%
        // hat: 16%
        // gloves: 9%
        private static readonly ChanceTable<WeenieClassName> ClothingWcids_Sho = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shirtbaggy,    0.06f ),
            ( WeenieClassName.capcloth,      0.08f ),
            ( WeenieClassName.doublet,       0.06f ),
            ( WeenieClassName.pantsflared,   0.08f ),
            ( WeenieClassName.shirtflared,   0.06f ),
            ( WeenieClassName.tunicflared,   0.05f ),
            ( WeenieClassName.glovescloth,   0.09f ),
            ( WeenieClassName.capsho,        0.08f ),
            ( WeenieClassName.breechesloose, 0.08f ),
            ( WeenieClassName.pantsloose,    0.08f ),
            ( WeenieClassName.shirtloose,    0.06f ),
            ( WeenieClassName.tunicloose,    0.05f ),
            ( WeenieClassName.shoes,         0.10f ),
            ( WeenieClassName.slippers,      0.07f ),
        };

        // invented:
        // shirt: 33%
        // pants: 24%
        // hat: 18%
        // shoes: 15%
        // gloves: 10%
        private static readonly ChanceTable<WeenieClassName> ClothingWcids_Viamontian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shirtviamontfancy,   0.11f ),
            ( WeenieClassName.shirtviamontpoet,    0.11f ),
            ( WeenieClassName.shirtviamontvest,    0.11f ),
            ( WeenieClassName.leggingsviamont,     0.24f ),
            ( WeenieClassName.hatberet,            0.06f ),
            ( WeenieClassName.hatbandana,          0.06f ),
            ( WeenieClassName.ace44975_hood,       0.06f ),     // ?
            ( WeenieClassName.shoesviamontloafers, 0.08f ),
            ( WeenieClassName.shoes,               0.08f ),     // common to all?
            ( WeenieClassName.glovescloth,         0.10f ),
        };

        public static WeenieClassName Roll(TreasureDeath treasureDeath)
        {
            var heritage = HeritageChance.Roll(treasureDeath.UnknownChances);

            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                    return ClothingWcids_Aluvian.Roll();

                case HeritageGroup.Gharundim:
                    return ClothingWcids_Gharundim.Roll();

                case HeritageGroup.Sho:
                    return ClothingWcids_Sho.Roll();

                case HeritageGroup.Viamontian:
                    return ClothingWcids_Viamontian.Roll();
            }
            return WeenieClassName.undef;
        }
    }
}
