using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureItemTypeChances_Orig
    {
        private static readonly ChanceTable<TreasureItemType_Orig> profile1 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    1.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile2 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     1.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile3 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    1.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile4 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  1.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile5 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   1.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile6 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       1.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile7 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.0f ),
            ( TreasureItemType_Orig.Armor,     0.0f ),
            ( TreasureItemType_Orig.Scroll,    0.0f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile8 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.15f ),
            ( TreasureItemType_Orig.Armor,     0.15f ),
            ( TreasureItemType_Orig.Scroll,    0.14f ),
            ( TreasureItemType_Orig.Clothing,  0.14f ),
            ( TreasureItemType_Orig.Jewelry,   0.14f ),
            ( TreasureItemType_Orig.Gem,       0.14f ),
            ( TreasureItemType_Orig.ArtObject, 0.14f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile9 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.30f ),
            ( TreasureItemType_Orig.Armor,     0.30f ),
            ( TreasureItemType_Orig.Scroll,    0.20f ),
            ( TreasureItemType_Orig.Clothing,  0.05f ),
            ( TreasureItemType_Orig.Jewelry,   0.05f ),
            ( TreasureItemType_Orig.Gem,       0.05f ),
            ( TreasureItemType_Orig.ArtObject, 0.05f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile10 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.40f ),
            ( TreasureItemType_Orig.Armor,     0.40f ),
            ( TreasureItemType_Orig.Scroll,    0.20f ),
            ( TreasureItemType_Orig.Clothing,  0.0f ),
            ( TreasureItemType_Orig.Jewelry,   0.0f ),
            ( TreasureItemType_Orig.Gem,       0.0f ),
            ( TreasureItemType_Orig.ArtObject, 0.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> profile11 = new ChanceTable<TreasureItemType_Orig>()
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
        /// Index lookup would be treasure_death table, *_Selection_Chances columns
        /// </summary>
        public static Dictionary<int, ChanceTable<TreasureItemType_Orig>> Profiles = new Dictionary<int, ChanceTable<TreasureItemType_Orig>>()
        {
            { 1, profile1 },
            { 2, profile2 },
            { 3, profile3 },
            { 4, profile4 },
            { 5, profile5 },
            { 6, profile6 },
            { 7, profile7 },
            { 8, profile8 },
            { 9, profile9 },
            { 10, profile10 },
            { 11, profile11 },
        };
    }
}
