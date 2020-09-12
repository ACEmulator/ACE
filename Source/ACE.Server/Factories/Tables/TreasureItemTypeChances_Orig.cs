using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureItemTypeChances_Orig
    {
        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup1 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    1.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup2 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     1.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup3 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    1.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup4 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  1.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup5 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   1.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup6 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       1.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup7 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup8 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.15f ),
            ( TreasureItemType_Orig.Armor,     0.15f ),
            ( TreasureItemType_Orig.Scroll,    0.14f ),
            ( TreasureItemType_Orig.Clothing,  0.14f ),
            ( TreasureItemType_Orig.Jewelry,   0.14f ),
            ( TreasureItemType_Orig.Gem,       0.14f ),
            ( TreasureItemType_Orig.ArtObject, 0.14f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup9 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.30f ),
            ( TreasureItemType_Orig.Armor,     0.30f ),
            ( TreasureItemType_Orig.Scroll,    0.20f ),
            ( TreasureItemType_Orig.Clothing,  0.05f ),
            ( TreasureItemType_Orig.Jewelry,   0.05f ),
            ( TreasureItemType_Orig.Gem,       0.05f ),
            ( TreasureItemType_Orig.ArtObject, 0.05f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup10 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.40f ),
            ( TreasureItemType_Orig.Armor,     0.40f ),
            ( TreasureItemType_Orig.Scroll,    0.20f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> itemTypeChancesGroup11 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.25f ),
            ( TreasureItemType_Orig.Jewelry,   0.25f ),
            ( TreasureItemType_Orig.Gem,       0.25f ),
            ( TreasureItemType_Orig.ArtObject, 0.25f ),
        };

        /// <summary>
        /// TreasureDeath.ItemTreasureTypeSelectionChances indexes into these profiles
        /// </summary>
        public static Dictionary<int, ChanceTable<TreasureItemType_Orig>> itemTypeChancesGroups = new Dictionary<int, ChanceTable<TreasureItemType_Orig>>()
        {
            { 1, itemTypeChancesGroup1 },
            { 2, itemTypeChancesGroup2 },
            { 3, itemTypeChancesGroup3 },
            { 4, itemTypeChancesGroup4 },
            { 5, itemTypeChancesGroup5 },
            { 6, itemTypeChancesGroup6 },
            { 7, itemTypeChancesGroup7 },
            { 8, itemTypeChancesGroup8 },
            { 9, itemTypeChancesGroup9 },
            { 10, itemTypeChancesGroup10 },
            { 11, itemTypeChancesGroup11 },
        };
    }
}
