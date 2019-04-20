using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum AetheriaBitfield
    {
        None   = 0x0,
        Blue   = 0x1,
        Yellow = 0x2,
        Red    = 0x4,

        All    = Blue | Yellow | Red
    }
}
