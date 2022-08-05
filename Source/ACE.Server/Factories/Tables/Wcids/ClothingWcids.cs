using System.Collections.Generic;

using ACE.Database.Models.World;
using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

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
        private static ChanceTable<WeenieClassName> ClothingWcids_Aluvian = new ChanceTable<WeenieClassName>()
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
        private static ChanceTable<WeenieClassName> ClothingWcids_Gharundim = new ChanceTable<WeenieClassName>()
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
        private static ChanceTable<WeenieClassName> ClothingWcids_Sho = new ChanceTable<WeenieClassName>()
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
        private static ChanceTable<WeenieClassName> ClothingWcids_Viamontian = new ChanceTable<WeenieClassName>()
        {
            ( WeenieClassName.shirtviamontfancy,   0.11f ),
            ( WeenieClassName.shirtviamontpoet,    0.11f ),
            ( WeenieClassName.shirtviamontvest,    0.11f ),
            ( WeenieClassName.leggingsviamont,     0.24f ),
            ( WeenieClassName.hatberet,            0.06f ),
            ( WeenieClassName.hatbandana,          0.06f ),
            ( WeenieClassName.ace44975_hood,       0.06f ),     // introduced in 11-2011 - master of design, and has base al 100,
                                                                // so maybe not in viamontian clothing table?
            ( WeenieClassName.shoesviamontloafers, 0.075f ),
            ( WeenieClassName.bootsviamont,        0.075f ),    // in clothing table, instead of leather armor table?
                                                                // other boots are in leather armor table, but they have much higher al (90 vs. 20)
                                                                // this would follow the trend of al 20 head/hand/foot wearables being in the clothing tables
            ( WeenieClassName.glovescloth,         0.10f ),
        };

        public static WeenieClassName Roll(TreasureDeath treasureDeath)
        {
            var heritage = HeritageChance.Roll(treasureDeath.UnknownChances, true);

            switch (heritage)
            {
                case TreasureHeritageGroup.Aluvian:
                    return ClothingWcids_Aluvian.Roll();

                case TreasureHeritageGroup.Gharundim:
                    return ClothingWcids_Gharundim.Roll();

                case TreasureHeritageGroup.Sho:
                    return ClothingWcids_Sho.Roll();

                case TreasureHeritageGroup.Viamontian:
                    return ClothingWcids_Viamontian.Roll();
            }
            return WeenieClassName.undef;
        }

        private static readonly HashSet<WeenieClassName> _combined = new HashSet<WeenieClassName>();

        static ClothingWcids()
        {
            BuildCombined(ClothingWcids_Aluvian);
            BuildCombined(ClothingWcids_Gharundim);
            BuildCombined(ClothingWcids_Sho);
            BuildCombined(ClothingWcids_Viamontian);
        }

        private static void BuildCombined(ChanceTable<WeenieClassName> wcids)
        {
            foreach (var entry in wcids)
                _combined.Add(entry.result);
        }

        public static bool Contains(WeenieClassName wcid)
        {
            return _combined.Contains(wcid);
        }
    }
}
