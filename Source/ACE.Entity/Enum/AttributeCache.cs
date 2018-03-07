using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum AttributeCache : uint
    {
        Undef           = 0x00000000,
        Strength        = 0x00000001,
        Endurance       = 0x00000002,
        Quickness       = 0x00000004,
        Coordination    = 0x00000008,
        Focus           = 0x00000010,
        Self            = 0x00000020,
        Health          = 0x00000040,
        Stamina         = 0x00000080,
        Mana            = 0x00000100,

        Full = Strength | Endurance | Quickness | Coordination | Focus | Self | Health | Stamina | Mana
    }
}
