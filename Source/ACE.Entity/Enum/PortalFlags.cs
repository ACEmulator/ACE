using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum PortalFlags
    {
        ExactMatch = 0x1,
        PortalSide = 0x2
    }
}
