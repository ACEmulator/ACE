using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureMagicItemTypeChances_Orig
    {
        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup1 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    1.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup2 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     1.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup3 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    1.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup4 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  1.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup5 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   1.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup6 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       1.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup7 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup8 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.15f ),
            ( TreasureItemType_Orig.Armor,     0.15f ),
            ( TreasureItemType_Orig.Scroll,    0.14f ),
            ( TreasureItemType_Orig.Clothing,  0.14f ),
            ( TreasureItemType_Orig.Jewelry,   0.14f ),
            ( TreasureItemType_Orig.Gem,       0.14f ),
            ( TreasureItemType_Orig.ArtObject, 0.14f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup9 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.30f ),
            ( TreasureItemType_Orig.Armor,     0.30f ),
            ( TreasureItemType_Orig.Scroll,    0.10f ),
            ( TreasureItemType_Orig.Clothing,  0.10f ),
            ( TreasureItemType_Orig.Jewelry,   0.10f ),
            ( TreasureItemType_Orig.Gem,       0.05f ),
            ( TreasureItemType_Orig.ArtObject, 0.05f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup10 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.30f ),
            ( TreasureItemType_Orig.Armor,     0.30f ),
            ( TreasureItemType_Orig.Scroll,    0.20f ),
            ( TreasureItemType_Orig.Clothing,  0.10f ),
            ( TreasureItemType_Orig.Jewelry,   0.10f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup11 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.33f ),
            ( TreasureItemType_Orig.Gem,       0.34f ),
            ( TreasureItemType_Orig.ArtObject, 0.33f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemTypeChancesGroup12 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.40f ),
            ( TreasureItemType_Orig.Clothing,  0.00f ),
            ( TreasureItemType_Orig.Jewelry,   0.20f ),
            ( TreasureItemType_Orig.Gem,       0.20f ),
            ( TreasureItemType_Orig.ArtObject, 0.20f ),
        };

        /// <summary>
        /// TreasureDeath.MagicItemTreasureTypeSelectionChances indexes into these profiles
        /// </summary>
        public static Dictionary<int, ChanceTable<TreasureItemType_Orig>> magicItemTypeChancesGroups = new Dictionary<int, ChanceTable<TreasureItemType_Orig>>()
        {
            { 1, magicItemTypeChancesGroup1 },
            { 2, magicItemTypeChancesGroup2 },
            { 3, magicItemTypeChancesGroup3 },
            { 4, magicItemTypeChancesGroup4 },
            { 5, magicItemTypeChancesGroup5 },
            { 6, magicItemTypeChancesGroup6 },
            { 7, magicItemTypeChancesGroup7 },
            { 8, magicItemTypeChancesGroup8 },
            { 9, magicItemTypeChancesGroup9 },
            { 10, magicItemTypeChancesGroup10 },
            { 11, magicItemTypeChancesGroup11 },
            { 12, magicItemTypeChancesGroup12 },
        };
    }
}
