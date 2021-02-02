namespace ACE.Server.Factories.Enum
{
    // Enum used for adjusting the loot bias for the various types of Mana forge chests
    // TODO: port this over to TreasureItemTypeChances
    /*[Flags]
    public enum LootBias
    {
        UnBiased   = 0x0,
        Armor      = 0x1,
        Weapons    = 0x2,
        SpellComps = 0x4,
        Clothing   = 0x8,
        Jewelry    = 0x10,

        MagicEquipment = Armor | Weapons | SpellComps | Clothing | Jewelry,
        MixedEquipment = Armor | Weapons | SpellComps | Clothing | Jewelry
    }*/

    public enum LootBias
    {
        UnBiased,

        Armor,
        Weapons,
        SpellComps,
        Clothing,
        Jewelry,

        MagicEquipment,
        MixedEquipment
    };
}
