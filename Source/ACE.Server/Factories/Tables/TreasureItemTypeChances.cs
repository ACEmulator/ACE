using ACE.Server.Factories.Entity;
using ACE.Server.Factories.Enum;

namespace ACE.Server.Factories.Tables
{
    public static class TreasureItemTypeChances
    {
        // converting to a percentage base roll for items (to better align with retail drop rate data from Magnus) - HarliQ 11/11/19
        // Gems 14%
        // Armor 24%
        // Weapons 30%
        // Clothing 13%
        // Cloaks 1%
        // Jewelry 18%
        // - jewelry - 10%
        // - last 8% - is magical, more jewelry, else dinnerware

        public static readonly ChanceTable<TreasureItemType> DefaultMagical = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Gem,        0.14f ),
            ( TreasureItemType.Armor,      0.24f ),
            ( TreasureItemType.Weapon,     0.30f ),
            ( TreasureItemType.Clothing,   0.13f ),
            ( TreasureItemType.Cloak,      0.01f ),
            ( TreasureItemType.Jewelry,    0.18f )
        };

        public static readonly ChanceTable<TreasureItemType> DefaultNonMagical = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Gem,        0.14f ),
            ( TreasureItemType.Armor,      0.24f ),
            ( TreasureItemType.Weapon,     0.30f ),
            ( TreasureItemType.Clothing,   0.13f ),
            ( TreasureItemType.Cloak,      0.01f ),
            ( TreasureItemType.Jewelry,    0.10f ),
            ( TreasureItemType.Dinnerware, 0.08f )
        };

        // LootBias.Armor
        public static readonly ChanceTable<TreasureItemType> Armor = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Armor,   1.0f )
        };

        // LootBias.Weapons
        public static readonly ChanceTable<TreasureItemType> Weapons = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Weapon,  1.0f)
        };

        // LootBias.Jewelry
        public static readonly ChanceTable<TreasureItemType> Jewelry = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Jewelry, 1.0f)
        };

        // from Magnus loot logs - Container_Legendary_Chest_T8 (5,826 items)
        // ~32.7% weapons, 35.6% armor, 14.1% clothing, 7.8% jewelry, 4.37% gems, 5.3% misc

        // LootBias.MixedEquipment
        // LootBias.MagicEquipment
        /*public static readonly ChanceTable<TreasureItemType> MixedMagicEquipment = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Weapon,     0.36f ),
            ( TreasureItemType.Armor,      0.40f ),
            ( TreasureItemType.Clothing,   0.15f ),
            ( TreasureItemType.Jewelry,    0.08f ),
            //( TreasureItemType.Gem,        0.05f ),    // 100% aetheria
            //( TreasureItemType.Dinnerware, 0.05f ),
            ( TreasureItemType.Cloak,      0.01f ),
        };*/

        public static readonly ChanceTable<TreasureItemType> MixedMagicEquipment = new ChanceTable<TreasureItemType>()
        {
            ( TreasureItemType.Armor,      0.30f ),
            ( TreasureItemType.Weapon,     0.35f ),
            ( TreasureItemType.Jewelry,    0.20f ),
            ( TreasureItemType.Clothing,   0.14f ),
            //( TreasureItemType.Gem,        0.05f ),    // 100% aetheria
            //( TreasureItemType.Dinnerware, 0.05f ),
            ( TreasureItemType.Cloak,      0.01f ),
        };
    }
}
