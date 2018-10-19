using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum MovementOption: byte
    {
        None                = 0x0,
        StickToObject       = 0x1,
        StandingLongJump    = 0x2
    };
}
