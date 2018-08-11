namespace ACE.Entity.Enum
{
    /// <summary>
    /// These are used with various movement related messages.
    /// 0 & 6-9 are used with F74C Animation
    /// </summary>
    public enum MovementTypes
    {
        General                       = 0x0, // This was named Invalid in ACLogView
        RawCommand                    = 0x1,
        InterpretedCommand            = 0x2,
        StopRawCommand                = 0x3,
        StopInterpretedCommand        = 0x4,
        StopCompletely                = 0x5,
        MoveToObject                  = 0x6,
        MoveToPosition                = 0x7,
        TurnToObject                  = 0x8,
        TurnToHeading                 = 0x9,
    }
}
