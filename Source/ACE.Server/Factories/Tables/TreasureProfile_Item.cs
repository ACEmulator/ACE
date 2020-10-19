using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureProfile_Item
    {
        // indexed by TreasureDeath.ItemTreasureTypeSelectionChances

        private static ChanceTable<TreasureItemType_Orig> itemProfile1 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> itemProfile2 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Armor,     1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> itemProfile3 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Scroll,    1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> itemProfile4 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Clothing,  1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> itemProfile5 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Jewelry,   1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> itemProfile6 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Gem,       1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> itemProfile7 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.ArtObject, 1.0f ),
        };

        /// <summary>
        /// The second most common ItemProfile
        /// </summary>
        private static ChanceTable<TreasureItemType_Orig> itemProfile8 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.125f ),
            ( TreasureItemType_Orig.Armor,     0.125f ),
            ( TreasureItemType_Orig.Scroll,    0.125f ),
            ( TreasureItemType_Orig.Clothing,  0.125f ),
            ( TreasureItemType_Orig.Jewelry,   0.125f ),
            ( TreasureItemType_Orig.Gem,       0.125f ),
            ( TreasureItemType_Orig.ArtObject, 0.125f ),
            ( TreasureItemType_Orig.PetDevice, 0.125f ),
        };

        /// <summary>
        /// The most common ItemProfile
        /// </summary>
        private static ChanceTable<TreasureItemType_Orig> itemProfile9 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.20f ),
            ( TreasureItemType_Orig.Armor,     0.20f ),
            ( TreasureItemType_Orig.Scroll,    0.20f ),
            ( TreasureItemType_Orig.Clothing,  0.05f ),
            ( TreasureItemType_Orig.Jewelry,   0.05f ),
            ( TreasureItemType_Orig.Gem,       0.05f ),
            ( TreasureItemType_Orig.ArtObject, 0.05f ),
            ( TreasureItemType_Orig.PetDevice, 0.20f ),
        };

        private static ChanceTable<TreasureItemType_Orig> itemProfile10 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    0.30f ),
            ( TreasureItemType_Orig.Armor,     0.30f ),
            ( TreasureItemType_Orig.Scroll,    0.20f ),
            ( TreasureItemType_Orig.PetDevice, 0.20f ),
        };

        private static ChanceTable<TreasureItemType_Orig> itemProfile11 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Clothing,  0.25f ),
            ( TreasureItemType_Orig.Jewelry,   0.25f ),
            ( TreasureItemType_Orig.Gem,       0.25f ),
            ( TreasureItemType_Orig.ArtObject, 0.25f ),
        };

        /// <summary>
        /// TreasureDeath.ItemTreasureTypeSelectionChances indexes into these profiles
        /// </summary>
        private static readonly List<ChanceTable<TreasureItemType_Orig>> itemProfiles = new List<ChanceTable<TreasureItemType_Orig>>()
        {
            itemProfile1,
            itemProfile2,
            itemProfile3,
            itemProfile4,
            itemProfile5,
            itemProfile6,
            itemProfile7,
            itemProfile8,
            itemProfile9,
            itemProfile10,
            itemProfile11,
        };

        /// <summary>
        /// Rolls for a TreasureItemType for a non-magical TreasureItemCategory.Item
        /// </summary>
        /// <param name="itemProfile">From TreasureDeath.ItemTreasureTypeSelectionChances</param>
        public static TreasureItemType_Orig Roll(int itemProfile)
        {
            if (itemProfile < 1 || itemProfile > itemProfiles.Count)
                return TreasureItemType_Orig.Undef;

            return itemProfiles[itemProfile - 1].Roll();
        }
    }
}
