using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureMagicItemTypeChances_Orig
    {
        // indexed by TreasureDeath.MagicItemTreasureTypeSelectionChances

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
        public static List<ChanceTable<TreasureItemType_Orig>> magicItemTypeChancesGroups = new List<ChanceTable<TreasureItemType_Orig>>()
        {
            magicItemTypeChancesGroup1,
            magicItemTypeChancesGroup2,
            magicItemTypeChancesGroup3,
            magicItemTypeChancesGroup4,
            magicItemTypeChancesGroup5,
            magicItemTypeChancesGroup6,
            magicItemTypeChancesGroup7,
            magicItemTypeChancesGroup8,
            magicItemTypeChancesGroup9,
            magicItemTypeChancesGroup10,
            magicItemTypeChancesGroup11,
            magicItemTypeChancesGroup12,
        };

        /// <summary>
        /// Rolls for a TreasureItemType for a TreasureItemCategory.MagicItem
        /// </summary>
        /// <param name="magicItemTypeChancesIdx">From TreasureDeath.MagicItemTreasureTypeSelectionChances</param>
        public static TreasureItemType_Orig Roll(int magicItemTypeChancesIdx)
        {
            if (magicItemTypeChancesIdx < 1 || magicItemTypeChancesIdx > magicItemTypeChancesGroups.Count)
                return TreasureItemType_Orig.Undef;

            return magicItemTypeChancesGroups[magicItemTypeChancesIdx - 1].Roll();
        }
    }
}
