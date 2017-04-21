namespace ACE.Network.Enum
{
    using System;

    /// <summary>
    /// used for building a set of flags to send to be used in GameMessageMotion
    /// </summary>
    [Flags]
    public enum MovementStateFlag : uint
    {
        NoMotionState = 0x0,
        CurrentStyle = 0x1,
        ForwardCommand = 0x2,
        ForwardSpeed = 0x4,
        SideStepCommand = 0x8,
        SideStepSpeed = 0x10,
        TurnCommand = 0x20,
        TurnSpeed = 0x40
    }

    [Flags]
    public enum MotionStateFlag
    {
        None = 0x0000,
        CurrentHoldKey = 0x0001,
        CurrentStyle = 0x0002,
        ForwardCommand = 0x0004,
        ForwardHoldKey = 0x0008,
        ForwardSpeed = 0x0010,
        SideStepCommand = 0x0020,
        SideStepHoldKey = 0x0040,
        SideStepSpeed = 0x0080,
        TurnCommand = 0x0100,
        TurnHoldKey = 0x0200,
        TurnSpeed = 0x0400
    }
}