using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum IdentifyResponseFlags
    {
        None                        = 0x0000,
        IntStatsTable               = 0x0001,
        BoolStatsTable              = 0x0002,
        FloatStatsTable             = 0x0004,
        StringStatsTable            = 0x0008,
        SpellBook                   = 0x0010,
        WeaponProfile               = 0x0020,
        HookProfile                 = 0x0040,
        ArmorProfile                = 0x0080,
        CreatureProfile             = 0x0100,
        ArmorEnchantmentBitfield    = 0x0200,
        ResistEnchantmentBitfield   = 0x0400,
        WeaponEnchantmentBitfield   = 0x0800,
        DidStatsTable               = 0x1000,
        Int64StatsTable             = 0x2000,
        ArmorLevels                 = 0x4000,
    }
}
