namespace ACE.Server.Factories.Enum
{
    public enum TreasureItemType_Orig
    {
        // retail
        Undef,
        Pyreal,
        Gem,
        Jewelry,
        ArtObject,
        Weapon,
        Armor,
        Clothing,
        Scroll,
        Caster,
        ManaStone,
        Consumable,
        HealKit,
        Lockpick,
        SpellComponent,

        /*LeatherArmor,
        StuddedLeatherArmor,
        ChainMailArmor,
        CovenantArmor,
        PlateMailArmor,
        HeritageLowArmor,
        HeritageHighArmor,

        SwordWeapon,
        MaceWeapon,
        AxeWeapon,
        SpearWeapon,
        UnarmedWeapon,
        StaffWeapon,
        DaggerWeapon,
        BowWeapon,
        CrossbowWeapon,
        AtlatlWeapon,*/

        // introduced 08-2008 - balance of power
        SocietyArmor,
        SocietyBreastplate,
        SocietyGauntlets,
        SocietyGirth,
        SocietyGreaves,
        SocietyHelm,
        SocietyPauldrons,
        SocietyTassets,
        SocietyVambraces,
        SocietySollerets,

        // introduced 10-2011 - cloak of darkness
        Cloak,

        // from analysis of magloot corpse logs, these appeared to be top-level items
        // PetDevices appeared to only be in the non-magical item tables
        // if a creature didn't drop non-magical items, there were no traces of it dropping a PetDevice
        // similarly, if a creature didn't drop non-magical or mundane items,
        // it was still found to drop EncapsulatedSpirit

        // introduced 02-2013 - balance of power
        PetDevice,
        EncapsulatedSpirit,
    }
}
