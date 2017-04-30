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
}