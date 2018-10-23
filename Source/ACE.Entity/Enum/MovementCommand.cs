namespace ACE.Entity.Enum
{
    /// <summary>
    /// A subset of MotionCommand & 0xFFFF
    /// </summary>
    public enum MovementCommand: ushort
    {
        Invalid         = 0x0,
        HoldRun         = 0x1,
        HoldSideStep    = 0x2,
        Ready           = 0x3,
        WalkForward     = 0x5,
        WalkBackwards   = 0x6,
        RunForward      = 0x7,
        TurnRight       = 0x0D,
        TurnLeft        = 0x0E,
        SideStepRight   = 0x0F,
        SideStepLeft    = 0x10
    }
}
