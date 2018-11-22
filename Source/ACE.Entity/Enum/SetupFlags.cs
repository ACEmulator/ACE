using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum SetupFlags
    {
        HasParent        = 0x1,
        HasDefaultScale  = 0x2,
        AllowFreeHeading = 0x4,
        HasPhysicsBSP    = 0x8
    }
}
