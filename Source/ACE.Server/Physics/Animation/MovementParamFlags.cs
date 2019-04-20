using System;

namespace ACE.Server.Physics.Animation
{
    [Flags]
    public enum MovementParamFlags: uint
    {
        CanWalk                 = 0x00001,
        CanRun                  = 0x00002,
        CanSidestep             = 0x00004,
        CanWalkBackwards        = 0x00008,
        CanCharge               = 0x00010,
        FailWalk                = 0x00020,
        UseFinalHeading         = 0x00040,
        Sticky                  = 0x00080,
        MoveAway                = 0x00100,
        MoveTowards             = 0x00200,
        UseSpheres              = 0x00400,
        SetHoldKey              = 0x00800,
        Autonomous              = 0x01000,
        ModifyRawState          = 0x02000,
        ModifyInterpretedState  = 0x04000,
        CancelMoveTo            = 0x08000,
        StopCompletely          = 0x10000,
        DisableJumpDuringLink   = 0x20000,
    }

    public static class MovementParamFlagsHelper
    {
        public static MovementParamFlags Get(MovementParameters mvp)
        {
            var flags = new MovementParamFlags();
            if (mvp.CanWalk)
                flags |= MovementParamFlags.CanWalk;
            if (mvp.CanRun)
                flags |= MovementParamFlags.CanRun;
            if (mvp.CanSidestep)
                flags |= MovementParamFlags.CanSidestep;
            if (mvp.CanWalkBackwards)
                flags |= MovementParamFlags.CanWalkBackwards;
            if (mvp.CanCharge)
                flags |= MovementParamFlags.CanCharge;
            if (mvp.FailWalk)
                flags |= MovementParamFlags.FailWalk;
            if (mvp.UseFinalHeading)
                flags |= MovementParamFlags.UseFinalHeading;
            if (mvp.Sticky)
                flags |= MovementParamFlags.Sticky;
            if (mvp.MoveAway)
                flags |= MovementParamFlags.MoveAway;
            if (mvp.MoveTowards)
                flags |= MovementParamFlags.MoveTowards;
            if (mvp.UseSpheres)
                flags |= MovementParamFlags.UseSpheres;
            if (mvp.SetHoldKey)
                flags |= MovementParamFlags.SetHoldKey;
            if (mvp.Autonomous)
                flags |= MovementParamFlags.Autonomous;
            if (mvp.ModifyRawState)
                flags |= MovementParamFlags.ModifyRawState;
            if (mvp.ModifyInterpretedState)
                flags |= MovementParamFlags.ModifyInterpretedState;
            if (mvp.CancelMoveTo)
                flags |= MovementParamFlags.CancelMoveTo;
            if (mvp.StopCompletely)
                flags |= MovementParamFlags.StopCompletely;
            if (mvp.DisableJumpDuringLink)
                flags |= MovementParamFlags.DisableJumpDuringLink;

            return flags;
        }

        public static void Set(MovementParameters mvp, MovementParamFlags flags)
        {
            mvp.CanWalk = flags.HasFlag(MovementParamFlags.CanWalk);
            mvp.CanRun = flags.HasFlag(MovementParamFlags.CanRun);
            mvp.CanSidestep = flags.HasFlag(MovementParamFlags.CanSidestep);
            mvp.CanWalkBackwards = flags.HasFlag(MovementParamFlags.CanWalkBackwards);
            mvp.CanCharge = flags.HasFlag(MovementParamFlags.CanCharge);
            mvp.FailWalk = flags.HasFlag(MovementParamFlags.FailWalk);
            mvp.UseFinalHeading = flags.HasFlag(MovementParamFlags.UseFinalHeading);
            mvp.Sticky = flags.HasFlag(MovementParamFlags.Sticky);
            mvp.MoveAway = flags.HasFlag(MovementParamFlags.MoveAway);
            mvp.MoveTowards = flags.HasFlag(MovementParamFlags.MoveTowards);
            mvp.UseSpheres = flags.HasFlag(MovementParamFlags.UseSpheres);
            mvp.SetHoldKey = flags.HasFlag(MovementParamFlags.SetHoldKey);
            mvp.Autonomous = flags.HasFlag(MovementParamFlags.Autonomous);
            mvp.ModifyRawState = flags.HasFlag(MovementParamFlags.ModifyRawState);
            mvp.ModifyInterpretedState = flags.HasFlag(MovementParamFlags.ModifyInterpretedState);
            mvp.CancelMoveTo = flags.HasFlag(MovementParamFlags.CancelMoveTo);
            mvp.StopCompletely = flags.HasFlag(MovementParamFlags.StopCompletely);
            mvp.DisableJumpDuringLink = flags.HasFlag(MovementParamFlags.DisableJumpDuringLink);
        }
    }
}
