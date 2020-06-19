using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum PortalBitmask
    {
        Undef           = 0x00,
        //NotPassable     = 0x00,
        Unrestricted    = 0x01,
        NoPk            = 0x02,
        NoPKLite        = 0x04,
        NoNPK           = 0x08,
        NoSummon        = 0x10,
        NoRecall        = 0x20,

        // These were added when playable Olthoi were introduced
        OnlyOlthoiPCs   = 0x40,
        NoOlthoiPCs     = 0x80,
        NoVitae         = 0x100,
        NoNewAccounts   = 0x200
    }
}
