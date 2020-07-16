using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum FactionBits
    {
        None          = 0x0,
        CelestialHand = 0x1,
        EldrytchWeb   = 0x2,
        RadiantBlood  = 0x4
    }
}
