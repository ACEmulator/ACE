using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class ArmorTypeChance
    {
        private static readonly ChanceTable<TreasureItemType_Orig> T1_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.34f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.33f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.33f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.00f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.00f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.00f ),
            ( TreasureItemType_Orig.HeritageHighArmor,   0.00f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T2_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.25f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.25f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.25f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.25f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.00f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.00f ),
            ( TreasureItemType_Orig.HeritageHighArmor,   0.00f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T3_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.22f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.22f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.23f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.23f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.05f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.05f ),
            ( TreasureItemType_Orig.HeritageHighArmor,   0.00f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T4_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.16f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.16f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.17f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.17f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.17f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.17f ),
            ( TreasureItemType_Orig.HeritageHighArmor,   0.00f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T5_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.15f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.16f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.16f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.16f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.16f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.16f ),
            ( TreasureItemType_Orig.HeritageHighArmor,   0.05f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T6_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.14f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.14f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.14f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.14f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.14f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.15f ),
            ( TreasureItemType_Orig.HeritageHighArmor,   0.15f ),
        };

        private static readonly List<ChanceTable<TreasureItemType_Orig>> armorTiers = new List<ChanceTable<TreasureItemType_Orig>>()
        {
            T1_Chances,
            T2_Chances,
            T3_Chances,
            T4_Chances,
            T5_Chances,
            T6_Chances,
        };

        public static TreasureItemType_Orig Roll(int tier)
        {
            // todo: add unique profiles for t7 / t8?
            tier = Math.Clamp(tier, 1, 6);

            return armorTiers[tier - 1].Roll();
        }
    }
}
