using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum MovementParams: uint
    {
        CanWalk                 = 0x1,
        CanRun                  = 0x2,
        CanSideStep             = 0x4,
        CanWalkBackwards        = 0x8,
        CanCharge               = 0x10,
        FailWalk                = 0x20,
        UseFinalHeading         = 0x40,
        Sticky                  = 0x80,
        MoveAway                = 0x100,
        MoveTowards             = 0x200,
        UseSpheres              = 0x400,
        SetHoldKey              = 0x800,
        Autonomous              = 0x1000,
        ModifyRawState          = 0x2000,
        ModifyInterpretedState  = 0x4000,
        CancelMoveTo            = 0x8000,
        StopCompletely          = 0x10000,
        DisableJumpDuringLink   = 0x20000
    }

    public static class MovementParamsExtensions
    {
        public static MovementParams Default =
            MovementParams.CanWalk |
            MovementParams.CanRun |
            MovementParams.CanSideStep |
            MovementParams.CanWalkBackwards |
            MovementParams.MoveTowards |
            MovementParams.UseSpheres |
            MovementParams.SetHoldKey |
            MovementParams.ModifyRawState |
            MovementParams.ModifyInterpretedState |
            MovementParams.CancelMoveTo;
    }
}
