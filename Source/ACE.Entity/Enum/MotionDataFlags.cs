using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum MotionDataFlags: byte
    {
        HasVelocity = 0x1,
        HasOmega    = 0x2
    }
}
