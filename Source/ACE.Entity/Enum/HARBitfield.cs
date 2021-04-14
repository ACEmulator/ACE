using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum HARBitfield
    {
        Undef             = 0x0,
        OpenHouse         = 0x1,
        AllegianceGuests  = 0x2,
        AllegianceStorage = 0x4
    }
}
