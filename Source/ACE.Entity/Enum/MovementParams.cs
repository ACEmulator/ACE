using System;

namespace ACE.Entity.Enum
{
    [Flags]
    public enum MovementParams
    {
        CanWalk                 = (1 << 0),
        CanRun                  = (1 << 1),
        CanSideStep             = (1 << 2),
        CanWalkBackwards        = (1 << 3),
        CanCharge               = (1 << 4),
        FailWalk                = (1 << 5),
        UseFinalHeading         = (1 << 6),
        Sticky                  = (1 << 7),
        MoveAway                = (1 << 8),
        MoveTowards             = (1 << 9),
        UseSpheres              = (1 << 10),
        SetHoldKey              = (1 << 11),
        Autonomous              = (1 << 12),
        ModifyRawState          = (1 << 13),
        ModifyInterpretedState  = (1 << 14),
        CancelMoveTo            = (1 << 15),
        StopCompletely          = (1 << 16),
        DisableJumpDuringLink   = (1 << 17)
    }

    public static class MovementParamsExtensions
    {
        public static MovementParams Default =
            MovementParams.CanWalk | MovementParams.CanRun | MovementParams.CanSideStep | MovementParams.CanWalkBackwards |
            MovementParams.MoveTowards | MovementParams.UseSpheres | MovementParams.SetHoldKey |
            MovementParams.ModifyRawState | MovementParams.ModifyInterpretedState | MovementParams.CancelMoveTo;
    }
}
