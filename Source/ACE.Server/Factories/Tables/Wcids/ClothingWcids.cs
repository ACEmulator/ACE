using ACE.Common;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Factories.Entity;

using WeenieClassName = ACE.Server.Factories.Enum.WeenieClassName;

namespace ACE.Server.Factories.Tables.Wcids
{
    public static class ClothingWcids
    {
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

        public static WeenieClassName Roll(TreasureDeath treasureDeath)
        {
            var heritage = RollHeritage(treasureDeath);

            switch (heritage)
            {
                case HeritageGroup.Aluvian:
                    return ClothingWcids_Aluvian.Roll();

                case HeritageGroup.Gharundim:
                    return ClothingWcids_Gharundim.Roll();
            }
            return WeenieClassName.undef;
        }

        public static HeritageGroup RollHeritage(TreasureDeath treasureDeath)
        {
            var heritage = HeritageChance.Roll(treasureDeath.UnknownChances);

            // FIXME: missing sho clothing table
            if (heritage >= HeritageGroup.Sho)
                heritage = (HeritageGroup)ThreadSafeRandom.Next(1, 2);

            return heritage;
        }
    }
}
