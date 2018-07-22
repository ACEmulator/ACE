using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// The client calls this HookType, but, that name conflicts with HookType that resides in ACE.Server.Physics.Hooks.
    /// The one in Physics should probably be renamed to PhysicsHookType
    /// </summary>
    [Flags]
    public enum HookTypeEnum
    {
        Undef   = 0,
        Floor   = (1 << 0),
        Wall    = (1 << 1),
        Ceiling = (1 << 2),
        Yard    = (1 << 3),
        Roof    = (1 << 4)
    }
}
