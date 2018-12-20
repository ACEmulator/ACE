using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum HookType
    {
        Undef   = 0x00,
        Floor   = 0x01,
        Wall    = 0x02,
        Ceiling = 0x04,
        Yard    = 0x08,
        Roof    = 0x10
    }
}
