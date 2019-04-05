using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum ChessMoveFlag
    {
        None             = 0x0,
        Normal           = 0x1,
        Capture          = 0x2,
        BigPawn          = 0x4,
        EnPassantCapture = 0x8,
        Promotion        = 0x10,
        KingSideCastle   = 0x20,
        QueenSideCastle  = 0x40
    }
}
