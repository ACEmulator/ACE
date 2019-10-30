using System;

namespace ACE.Server.Physics.Common
{
    [Flags]
    public enum Quadrant
    {
        None   = 0x0,
        Low    = 0x1,
        Medium = 0x2,
        High   = 0x4,
        Left   = 0x8,
        Right  = 0x10,
        Front  = 0x20,
        Back   = 0x40
    };
}
