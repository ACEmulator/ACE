using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureItemTypeChances_Orig
    {
        // indexed by TreasureDeath.ItemTreasureTypeSelectionChances

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
        private static readonly List<ChanceTable<TreasureItemType_Orig>> itemTypeChancesGroups = new List<ChanceTable<TreasureItemType_Orig>>()
        {
            itemTypeChancesGroup1,
            itemTypeChancesGroup2,
            itemTypeChancesGroup3,
            itemTypeChancesGroup4,
            itemTypeChancesGroup5,
            itemTypeChancesGroup6,
            itemTypeChancesGroup7,
            itemTypeChancesGroup8,
            itemTypeChancesGroup9,
            itemTypeChancesGroup10,
            itemTypeChancesGroup11,
        };

        /// <summary>
        /// Rolls for a TreasureItemType for a non-magical TreasureItemCategory.Item
        /// </summary>
        /// <param name="itemTypeChancesIdx">From TreasureDeath.ItemTreasureTypeSelectionChances</param>
        public static TreasureItemType_Orig Roll(int itemTypeChancesIdx)
        {
            if (itemTypeChancesIdx < 1 || itemTypeChancesIdx > itemTypeChancesGroups.Count)
                return TreasureItemType_Orig.Undef;

            return itemTypeChancesGroups[itemTypeChancesIdx - 1].Roll();
        }
    }
}
