using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum AmmoType : ushort
    {
        None                = 0x0,
        Arrow               = 0x1,
        Bolt                = 0x2,
        Atlatl              = 0x4,
        ArrowCrystal        = 0x8,
        BoltCrystal         = 0x10,
        AtlatlCrystal       = 0x20,
        ArrowChorizite      = 0x40,
        BoltChorizite       = 0x80,
        AtlatlChorizite     = 0x100
    }
}
