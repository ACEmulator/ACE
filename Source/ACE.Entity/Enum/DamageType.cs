using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum DamageType
    {
        // TODO: Check for more damage types
        Nether      = 0x00000000,
        Slashing    = 0x00000001,
        Piercing    = 0x00000002,
        Bludgeoning = 0x00000004,
        Cold        = 0x00000008,
        Fire        = 0x00000010,
        Acid        = 0x00000020,
        Lightning   = 0x00000040,
        Drain       = 0x00000080,
    }
}
