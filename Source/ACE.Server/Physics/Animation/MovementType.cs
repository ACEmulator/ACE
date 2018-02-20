namespace ACE.Server.Physics.Animation
{
    public enum MovementType
    {
        Invalid = 0x0,
        RawCommand = 0x1,
        InterpretedCommand = 0x2,
        StopRawCommand = 0x3,
        StopInterpretedCommand = 0x4,
        StopCompletely = 0x5,
        MoveToObject = 0x6,
        MoveToPosition = 0x7,
        TurnToObject = 0x8,
        TurnToHeading = 0x9,
    };
}
