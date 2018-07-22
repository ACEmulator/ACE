using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum HookType
    {
        Undef   = 0,
        Floor   = (1 << 0),
        Wall    = (1 << 1),
        Ceiling = (1 << 2),
        Yard    = (1 << 3),
        Roof    = (1 << 4)
    }
}
