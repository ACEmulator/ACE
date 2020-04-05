using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum Quadrant
    {
        None   = 0x0,
        High   = 0x1,
        Medium = 0x2,
        Low    = 0x4,
        Left   = 0x8,
        Right  = 0x10,
        Front  = 0x20,
        Back   = 0x40
    };
}
