using System;

namespace ACE.Server.Network.Enum
{
    [Flags]
    public enum RawMotionFlags: uint
    {
        Invalid         = 0x0,
        CurrentHoldKey  = 0x1,
        CurrentStyle    = 0x2,
        ForwardCommand  = 0x4,
        ForwardHoldKey  = 0x8,
        ForwardSpeed    = 0x10,
        SideStepCommand = 0x20,
        SideStepHoldKey = 0x40,
        SideStepSpeed   = 0x80,
        TurnCommand     = 0x100,
        TurnHoldKey     = 0x200,
        TurnSpeed       = 0x400
    }
}
