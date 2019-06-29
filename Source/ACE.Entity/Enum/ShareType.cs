using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum ShareType
    {
        None       = 0x0,
        Fellowship = 0x1,
        Allegiance = 0x2,
        All        = 0x3
    }
}
