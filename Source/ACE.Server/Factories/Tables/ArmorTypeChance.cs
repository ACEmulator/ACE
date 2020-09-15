using System;
using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class ArmorTypeChance
    {
        private static readonly ChanceTable<TreasureArmorType> T1_Chances = new ChanceTable<TreasureArmorType>()
        {
            ( TreasureArmorType.Leather,        0.34f ),
            ( TreasureArmorType.StuddedLeather, 0.33f ),
            ( TreasureArmorType.Chainmail,      0.33f ),
        };

        private static readonly ChanceTable<TreasureArmorType> T2_Chances = new ChanceTable<TreasureArmorType>()
        {
            ( TreasureArmorType.Leather,        0.25f ),
            ( TreasureArmorType.StuddedLeather, 0.25f ),
            ( TreasureArmorType.Chainmail,      0.25f ),
            ( TreasureArmorType.Platemail,      0.25f ),
        };

        private static readonly ChanceTable<TreasureArmorType> T3_Chances = new ChanceTable<TreasureArmorType>()
        {
            ( TreasureArmorType.Leather,        0.22f ),
            ( TreasureArmorType.StuddedLeather, 0.22f ),
            ( TreasureArmorType.Chainmail,      0.22f ),
            ( TreasureArmorType.Platemail,      0.22f ),
            ( TreasureArmorType.HeritageLow,    0.05f ),
            ( TreasureArmorType.Covenant,       0.05f ),
            ( TreasureArmorType.Overrobe,       0.02f ),    // added
        };

        private static readonly ChanceTable<TreasureArmorType> T4_Chances = new ChanceTable<TreasureArmorType>()
        {
            ( TreasureArmorType.Leather,        0.16f ),
            ( TreasureArmorType.StuddedLeather, 0.16f ),
            ( TreasureArmorType.Chainmail,      0.17f ),
            ( TreasureArmorType.Platemail,      0.17f ),
            ( TreasureArmorType.HeritageLow,    0.16f ),
            ( TreasureArmorType.Covenant,       0.16f ),
            ( TreasureArmorType.Overrobe,       0.02f ),    // added
        };

        private static readonly ChanceTable<TreasureArmorType> T5_Chances = new ChanceTable<TreasureArmorType>()
        {
            ( TreasureArmorType.Leather,        0.15f ),
            ( TreasureArmorType.StuddedLeather, 0.15f ),
            ( TreasureArmorType.Chainmail,      0.16f ),
            ( TreasureArmorType.Platemail,      0.16f ),
            ( TreasureArmorType.HeritageLow,    0.15f ),
            ( TreasureArmorType.Covenant,       0.16f ),
            ( TreasureArmorType.HeritageHigh,   0.05f ),
            ( TreasureArmorType.Overrobe,       0.02f ),    // added
        };

        private static readonly ChanceTable<TreasureArmorType> T6_Chances = new ChanceTable<TreasureArmorType>()
        {
            ( TreasureArmorType.Leather,        0.12f ),
            ( TreasureArmorType.StuddedLeather, 0.12f ),
            ( TreasureArmorType.Chainmail,      0.12f ),
            ( TreasureArmorType.Platemail,      0.12f ),
            ( TreasureArmorType.HeritageLow,    0.12f ),
            ( TreasureArmorType.Covenant,       0.15f ),
            ( TreasureArmorType.HeritageHigh,   0.15f ),
            ( TreasureArmorType.Haebrean,       0.04f ),    // added
            ( TreasureArmorType.KnorrAcademy,   0.02f ),
            ( TreasureArmorType.Sedgemail,      0.02f ),
            ( TreasureArmorType.Overrobe,       0.02f ),
        };

        // added, from mag-loot logs
        private static readonly ChanceTable<TreasureArmorType> T7_Chances = new ChanceTable<TreasureArmorType>()
        {
            ( TreasureArmorType.Leather,        0.11f ),
            ( TreasureArmorType.StuddedLeather, 0.11f ),
            ( TreasureArmorType.Chainmail,      0.12f ),
            ( TreasureArmorType.Platemail,      0.12f ),
            ( TreasureArmorType.HeritageLow,    0.12f ),
            ( TreasureArmorType.Covenant,       0.12f ),
            ( TreasureArmorType.HeritageHigh,   0.12f ),
            ( TreasureArmorType.Olthoi,         0.03f ),
            ( TreasureArmorType.OlthoiHeritage, 0.05f ),
            ( TreasureArmorType.Haebrean,       0.04f ),
            ( TreasureArmorType.KnorrAcademy,   0.02f ),
            ( TreasureArmorType.Sedgemail,      0.02f ),
            ( TreasureArmorType.Overrobe,       0.02f ),
        };


        private static readonly ChanceTable<TreasureArmorType> T8_Chances = new ChanceTable<TreasureArmorType>()
        {
            ( TreasureArmorType.Leather,        0.10f ),
            ( TreasureArmorType.StuddedLeather, 0.10f ),
            ( TreasureArmorType.Chainmail,      0.10f ),
            ( TreasureArmorType.Platemail,      0.10f ),
            ( TreasureArmorType.HeritageLow,    0.11f ),
            ( TreasureArmorType.Covenant,       0.07f ),
            ( TreasureArmorType.HeritageHigh,   0.14f ),
            ( TreasureArmorType.Olthoi,         0.06f ),
            ( TreasureArmorType.OlthoiHeritage, 0.12f ),
            ( TreasureArmorType.Haebrean,       0.04f ),
            ( TreasureArmorType.KnorrAcademy,   0.02f ),
            ( TreasureArmorType.Sedgemail,      0.02f ),
            ( TreasureArmorType.Overrobe,       0.02f ),
        };

        private static readonly List<ChanceTable<TreasureArmorType>> armorTiers = new List<ChanceTable<TreasureArmorType>>()
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

        public static TreasureArmorType Roll(int tier)
        {
            return armorTiers[tier - 1].Roll();
        }
    }
}
