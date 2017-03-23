
namespace ACE.Network.Enum
{
    using System;

    /// <summary>
    /// used for building a set of flags to send to be used in GameMessageMotion
    /// </summary>
    [Flags]
    public enum MotionStateFlag : uint
    {
        NoMotionState   = 0x00000000,
        CurrentStyle    = 0x00000001,
        ForwardCommand  = 0x00000002,
        ForwardSpeed    = 0x00000004,
        SideStepCommand = 0x00000008,
        SideStepSpeed   = 0x00000010,
        TurnCommand     = 0x00000020,
        TurnSpeed       = 0x00000040
    }
}
