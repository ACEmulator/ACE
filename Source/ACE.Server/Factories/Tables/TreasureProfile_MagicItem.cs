using System.Collections.Generic;

using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureProfile_MagicItem
    {
        // indexed by TreasureDeath.MagicItemTreasureTypeSelectionChances

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile1 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,    1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile2 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Armor,     1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile3 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Scroll,    1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile4 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Clothing,  1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile5 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Jewelry,   1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile6 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Gem,       1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile7 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.ArtObject, 1.0f ),
        };

        /// <summary>
        /// A very common MagicItem profile
        /// </summary>
        private static ChanceTable<TreasureItemType_Orig> magicItemProfile8 = new ChanceTable<TreasureItemType_Orig>()
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
        private static ChanceTable<TreasureItemType_Orig> magicItemProfile9 = new ChanceTable<TreasureItemType_Orig>()
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

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile10 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,             0.30f ),
            ( TreasureItemType_Orig.Armor,              0.30f ),
            ( TreasureItemType_Orig.Scroll,             0.185f ),
            ( TreasureItemType_Orig.Clothing,           0.10f ),
            ( TreasureItemType_Orig.Jewelry,            0.10f ),
            ( TreasureItemType_Orig.Cloak,              0.01f  ),
            ( TreasureItemType_Orig.EncapsulatedSpirit, 0.005f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile11 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Jewelry,            0.33f ),
            ( TreasureItemType_Orig.Gem,                0.34f ),
            ( TreasureItemType_Orig.ArtObject,          0.315f ),
            ( TreasureItemType_Orig.Cloak,              0.01f  ),
            ( TreasureItemType_Orig.EncapsulatedSpirit, 0.005f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile12 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Scroll,             0.40f ),
            ( TreasureItemType_Orig.Jewelry,            0.20f ),
            ( TreasureItemType_Orig.Gem,                0.20f ),
            ( TreasureItemType_Orig.ArtObject,          0.185f ),
            ( TreasureItemType_Orig.Cloak,              0.01f  ),
            ( TreasureItemType_Orig.EncapsulatedSpirit, 0.005f ),
        };

        // added

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile13 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietyBreastplate, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile14 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietyGauntlets, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile15 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietyGirth, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile16 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietyGreaves, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile17 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietyHelm, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile18 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietyPauldrons, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile19 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietyTassets, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile20 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietyVambraces, 1.0f ),
        };

        private static ChanceTable<TreasureItemType_Orig> magicItemProfile21 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.SocietySollerets, 1.0f ),
        };

        // Legendary Chest
        private static ChanceTable<TreasureItemType_Orig> magicItemProfile22 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,             0.35f ),
            ( TreasureItemType_Orig.Armor,              0.35f ),
            ( TreasureItemType_Orig.Clothing,           0.10f ),
            ( TreasureItemType_Orig.Jewelry,            0.10f ),
            ( TreasureItemType_Orig.Cloak,              0.05f ),
            ( TreasureItemType_Orig.PetDevice,          0.05f ),
        };

        // Legendary Magic Chest
        private static ChanceTable<TreasureItemType_Orig> magicItemProfile23 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Clothing,           0.30f ),
            ( TreasureItemType_Orig.Jewelry,            0.20f ),
            ( TreasureItemType_Orig.Cloak,              0.20f ),
            ( TreasureItemType_Orig.PetDevice,          0.30f ),
        };

        // Mana Forge Chest
        private static ChanceTable<TreasureItemType_Orig> magicItemProfile24 = new ChanceTable<TreasureItemType_Orig>()
        {
            ( TreasureItemType_Orig.Weapon,           0.20f ),
            ( TreasureItemType_Orig.Armor,            0.20f ),
            ( TreasureItemType_Orig.Clothing,         0.30f ),
            ( TreasureItemType_Orig.Jewelry,          0.30f ),
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
            magicItemProfile13,
            magicItemProfile14,
            magicItemProfile15,
            magicItemProfile16,
            magicItemProfile17,
            magicItemProfile18,
            magicItemProfile19,
            magicItemProfile20,
            magicItemProfile21,
            magicItemProfile22,
            magicItemProfile23,
            magicItemProfile24,
        };

        /// <summary>
        /// Rolls for a TreasureItemType for a TreasureItemCategory.MagicItem
        /// </summary>
        /// <param name="magicItemProfile">From TreasureDeath.MagicItemTreasureTypeSelectionChances</param>
        public static TreasureItemType_Orig Roll(int magicItemProfile)
        {
            if (magicItemProfile < 1 || magicItemProfile > magicItemProfiles.Count)
                return TreasureItemType_Orig.Undef;

            return magicItemProfiles[magicItemProfile - 1].Roll();
        }
    }
}
