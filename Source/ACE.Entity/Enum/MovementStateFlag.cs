using System;

namespace ACE.Entity.Enum
{
    /// <summary>
    /// The current movement state for an object
    /// This is sent as part of the InterpretedMotionState network structure
    /// </summary>
    [Flags]
    public enum MovementStateFlag : uint
    {
        Invalid         = 0x0,
        CurrentStyle    = 0x1,
        ForwardCommand  = 0x2,
        ForwardSpeed    = 0x4,
        SideStepCommand = 0x8,
        SideStepSpeed   = 0x10,
        TurnCommand     = 0x20,
        TurnSpeed       = 0x40
    }
}
