using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum PortalType : uint
    {
        Purple      = 0x020001B3u,
        Blue        = 0x020005D2u,
        Green       = 0x020005D3u,
        Orange      = 0x020005D4u,
        Red         = 0x020005D5u,
        Yellow      = 0x020005D6u,
        White       = 0x020006F4u,
        Shadow      = 0x020008FDu,
        Broken      = 0x02000F2Eu,
        Destroyed   = 0x020019E4u
    }
}
