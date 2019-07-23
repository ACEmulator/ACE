using System;

namespace ACE.Server.Network.Enum
{
    [Flags]
    public enum AllegianceIndex
    {
        Undefined           = 0x0,
        LoggedIn            = 0x1,
        Update              = 0x2,
        HasAllegianceAge    = 0x4,
        HasPackedLevel      = 0x8,
        MayPassupExperience = 0x10
    }
}
