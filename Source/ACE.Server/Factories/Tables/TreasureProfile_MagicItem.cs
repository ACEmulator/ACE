using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureProfile_MagicItem
    {
        // indexed by TreasureDeath.MagicItemTreasureTypeSelectionChances

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile1 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile2 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Armor,     1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile3 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Scroll,    1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile4 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Clothing,  1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile5 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Jewelry,   1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile6 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Gem,       1.0f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile7 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.ArtObject, 1.0f ),
        };

        /// <summary>
        /// A very common MagicItem profile
        /// </summary>
        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile8 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,             0.15f ),
            ( TreasureItemType_Orig.Armor,              0.15f ),
            ( TreasureItemType_Orig.Scroll,             0.14f ),
            ( TreasureItemType_Orig.Clothing,           0.14f ),
            ( TreasureItemType_Orig.Jewelry,            0.14f ),
            ( TreasureItemType_Orig.Gem,                0.14f ),
            ( TreasureItemType_Orig.ArtObject,          0.125f ),
            ( TreasureItemType_Orig.Cloak,              0.01f  ),
            ( TreasureItemType_Orig.EncapsulatedSpirit, 0.005f ),
        };

        /// <summary>
        /// A very common MagicItem profile
        /// </summary>
        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile9 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,             0.30f ),
            ( TreasureItemType_Orig.Armor,              0.30f ),
            ( TreasureItemType_Orig.Scroll,             0.10f ),
            ( TreasureItemType_Orig.Clothing,           0.10f ),
            ( TreasureItemType_Orig.Jewelry,            0.10f ),
            ( TreasureItemType_Orig.Gem,                0.05f ),
            ( TreasureItemType_Orig.ArtObject,          0.035f ),
            ( TreasureItemType_Orig.Cloak,              0.01f  ),
            ( TreasureItemType_Orig.EncapsulatedSpirit, 0.005f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile10 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,             0.30f ),
            ( TreasureItemType_Orig.Armor,              0.30f ),
            ( TreasureItemType_Orig.Scroll,             0.185f ),
            ( TreasureItemType_Orig.Clothing,           0.10f ),
            ( TreasureItemType_Orig.Jewelry,            0.10f ),
            ( TreasureItemType_Orig.Cloak,              0.01f  ),
            ( TreasureItemType_Orig.EncapsulatedSpirit, 0.005f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile11 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Jewelry,            0.33f ),
            ( TreasureItemType_Orig.Gem,                0.34f ),
            ( TreasureItemType_Orig.ArtObject,          0.315f ),
            ( TreasureItemType_Orig.Cloak,              0.01f  ),
            ( TreasureItemType_Orig.EncapsulatedSpirit, 0.005f ),
        };

        private static readonly ChanceTable<TreasureItemType_Orig> magicItemProfile12 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Scroll,             0.40f ),
            ( TreasureItemType_Orig.Jewelry,            0.20f ),
            ( TreasureItemType_Orig.Gem,                0.20f ),
            ( TreasureItemType_Orig.ArtObject,          0.185f ),
            ( TreasureItemType_Orig.Cloak,              0.01f  ),
            ( TreasureItemType_Orig.EncapsulatedSpirit, 0.005f ),
        };

        /// <summary>
        /// TreasureDeath.MagicItemTreasureTypeSelectionChances indexes into these profiles
        /// </summary>
        public static List<ChanceTable<TreasureItemType_Orig>> magicItemProfiles = new List<ChanceTable<TreasureItemType_Orig>>()
        {
            magicItemProfile1,
            magicItemProfile2,
            magicItemProfile3,
            magicItemProfile4,
            magicItemProfile5,
            magicItemProfile6,
            magicItemProfile7,
            magicItemProfile8,
            magicItemProfile9,
            magicItemProfile10,
            magicItemProfile11,
            magicItemProfile12,
        };

        /// <summary>
        /// Rolls for a TreasureItemType for a TreasureItemCategory.MagicItem
        /// </summary>
        /// <param name="magicItemProfileIdx">From TreasureDeath.MagicItemTreasureTypeSelectionChances</param>
        public static TreasureItemType_Orig Roll(int magicItemProfileIdx)
        {
            if (magicItemProfileIdx < 1 || magicItemProfileIdx > magicItemProfiles.Count)
                return TreasureItemType_Orig.Undef;

            return magicItemProfiles[magicItemProfileIdx - 1].Roll();
        }
    }
}
