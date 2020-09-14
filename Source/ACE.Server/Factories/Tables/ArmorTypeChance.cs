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
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T2_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.25f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.25f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.25f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.25f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T3_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.22f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.22f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.22f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.22f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.05f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.05f ),
            ( TreasureItemType_Orig.OverRobe,            0.02f ),   // added
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T4_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.16f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.16f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.17f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.17f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.16f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.16f ),
            ( TreasureItemType_Orig.OverRobe,            0.02f ),   // added
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T5_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.15f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.15f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.16f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.16f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.15f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.16f ),
            ( TreasureItemType_Orig.OverRobe,            0.02f ),   // added
            ( TreasureItemType_Orig.HeritageHighArmor,   0.05f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> T6_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.12f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.12f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.12f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.12f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.12f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.15f ),
            ( TreasureItemType_Orig.OverRobe,            0.02f ),   // added
            ( TreasureItemType_Orig.HeritageHighArmor,   0.15f ),
            ( TreasureItemType_Orig.KnorrAcademyArmor,        0.02f ),   // added below
            ( TreasureItemType_Orig.SedgemailLeatherArmor,    0.02f ),
            ( TreasureItemType_Orig.HaebreanArmor,            0.04f ),
        };

        // added, from mag-loot logs
        private static readonly ChanceTable<TreasureItemType_Orig> T7_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.11f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.11f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.12f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.12f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.12f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.12f ),
            ( TreasureItemType_Orig.OverRobe,            0.02f ),
            ( TreasureItemType_Orig.HeritageHighArmor,   0.12f ),
            ( TreasureItemType_Orig.KnorrAcademyArmor,        0.02f ),
            ( TreasureItemType_Orig.SedgemailLeatherArmor,    0.02f ),
            ( TreasureItemType_Orig.HaebreanArmor,            0.04f ),
            ( TreasureItemType_Orig.OlthoiArmor,              0.03f ),
            ( TreasureItemType_Orig.OlthoiHeritageArmor,      0.05f ),
        };


        private static readonly ChanceTable<TreasureItemType_Orig> T8_Chances = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.LeatherArmor,        0.10f ),
            ( TreasureItemType_Orig.StuddedLeatherArmor, 0.10f ),
            ( TreasureItemType_Orig.ChainMailArmor,      0.10f ),
            ( TreasureItemType_Orig.PlateMailArmor,      0.10f ),
            ( TreasureItemType_Orig.HeritageLowArmor,    0.11f ),
            ( TreasureItemType_Orig.CovenantArmor,       0.07f ),
            ( TreasureItemType_Orig.OverRobe,            0.02f ),
            ( TreasureItemType_Orig.HeritageHighArmor,   0.14f ),
            ( TreasureItemType_Orig.KnorrAcademyArmor,        0.02f ),
            ( TreasureItemType_Orig.SedgemailLeatherArmor,    0.02f ),
            ( TreasureItemType_Orig.HaebreanArmor,            0.04f ),
            ( TreasureItemType_Orig.OlthoiArmor,              0.06f ),
            ( TreasureItemType_Orig.OlthoiHeritageArmor,      0.12f ),
        };

        private static readonly List<ChanceTable<TreasureItemType_Orig>> armorTiers = new List<ChanceTable<TreasureItemType_Orig>>()
        {
            T1_Chances,
            T2_Chances,
            T3_Chances,
            T4_Chances,
            T5_Chances,
            T6_Chances,
            T7_Chances,
            T8_Chances
        };

        public static TreasureItemType_Orig Roll(int tier)
        {
            return armorTiers[tier - 1].Roll();
        }
    }
}
