using System;

namespace ACE.Network.Enum
{
    [Flags]
    public enum IdentifyResponseFlags
    {
        None = 0x0000,
        IntStatsTable = 0x1,
        Int64StatsTable = 0x2000,
        BoolStatsTable = 0x2,
        FloatStatsTable = 0x4,
        StringStatsTable = 0x8,
        DidStatsTable = 0x1000,
        SpellBook = 0x10,       
        ArmorProfile = 0x80,
        CreatureProfile = 0x100,
        WeaponProfile = 0x20,
        HookProfile = 0x40,
        ArmorEnchantmentBitfield = 0x200,
        WeaponEnchantmentBitfield = 0x800,
        ResistEnchantmentBitfield = 0x400,
        ArmorLevels = 0x4000,
    }
}
