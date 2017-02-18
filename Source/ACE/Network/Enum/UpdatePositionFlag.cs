using System;

namespace ACE.Network.Enum
{
    [Flags]
    public enum UpdatePositionFlag
    {
        None          = 0x00,
        Velocity      = 0x01,
        Placement     = 0x02,
        Contact       = 0x04,
        NoQuaternionW = 0x08,
        NoQuaternionX = 0x10,
        NoQuaternionY = 0x20,
        NoQuaternionZ = 0x40
    }
}
