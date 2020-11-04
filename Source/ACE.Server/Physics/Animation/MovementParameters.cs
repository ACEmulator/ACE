using System;
using ACE.Entity.Enum;
using ACE.Server.Network.Structure;

namespace ACE.Server.Physics.Animation
{
    public class MovementParameters
    {
        public MovementParamFlags Flags
        {
            get => MovementParamFlagsHelper.Get(this);
            set => MovementParamFlagsHelper.Set(this, value);
        }

        public bool CanWalk;
        public bool CanRun;
        public bool CanSidestep;
        public bool CanWalkBackwards;
        public bool CanCharge;
        public bool FailWalk;
        public bool UseFinalHeading;
        public bool Sticky;
        public bool MoveAway;
        public bool MoveTowards;
        public bool UseSpheres;
        public bool SetHoldKey;
        public bool Autonomous;
        public bool ModifyRawState;
        public bool ModifyInterpretedState;
        public bool CancelMoveTo;
        public bool StopCompletely;
        public bool DisableJumpDuringLink;

        public float DistanceToObject;
        public float MinDistance;
        public float DesiredHeading;
        public float Speed;
        public float FailDistance;
        public float WalkRunThreshold;

        public int ContextID;
        public HoldKey HoldKeyToApply;
        public int ActionStamp;

        public static readonly float Default_DistanceToObject = 0.6f;
        public static readonly float Default_FailDistance = float.MaxValue;
        public static readonly float Default_MinDistance = 0.0f;
        public static readonly float Default_Speed = 1.0f;
        //public static readonly float Default_WalkRunThreshold = 15.0f;
        public static readonly float Default_WalkRunThreshold = 1.0f;

        public MovementParameters()
        {
            CanWalk = true;
            CanRun = true;
            CanSidestep = true;
            CanWalkBackwards = true;
            CanCharge = true;
            //FailWalk = false;
            //UseFinalHeading = false;
            //Sticky = false;
            //MoveAway = false;
            MoveTowards = true;
            UseSpheres = true;
            SetHoldKey = true;
            //Autonomous = false;
            ModifyRawState = true;
            ModifyInterpretedState = true;
            CancelMoveTo = true;
            StopCompletely = true;
            //DisableJumpDuringLink = false;

            DistanceToObject = Default_DistanceToObject;
            FailDistance = Default_FailDistance;
            //DesiredHeading = 0;
            MinDistance = Default_MinDistance;
            Speed = Default_Speed;
            WalkRunThreshold = Default_WalkRunThreshold;

            //ContextID = 0;
            HoldKeyToApply = HoldKey.Invalid;
            //ActionStamp = 0;

            //Flags = (MovementParamFlags)0x1EE0F;
        }

        /// <summary>
        /// Copy construtor
        /// </summary>
        public MovementParameters(MovementParameters mvp, bool onlyBits = false)
        {
            Flags = mvp.Flags;

            if (!onlyBits)
                CopyNonFlag(mvp);
        }

        /// <summary>
        /// Only copies some fields from input movement params
        /// </summary>
        public void CopySome(MovementParameters mvp)
        {
            // TODO: should this really be only copying some fields?? review

            CanWalk = mvp.CanWalk;
            CanRun = mvp.CanRun;
            CanSidestep = mvp.CanSidestep;
            CanWalkBackwards = mvp.CanWalkBackwards;
            CanCharge = mvp.CanCharge;
            // - failwalk
            // - use final heading
            // - sticky
            // - moveaway
            MoveTowards = mvp.MoveTowards;
            UseSpheres = mvp.UseSpheres;
            SetHoldKey = mvp.SetHoldKey;
            // - autonomous
            ModifyRawState = mvp.ModifyRawState;
            ModifyInterpretedState = mvp.ModifyInterpretedState;
            CancelMoveTo = mvp.CancelMoveTo;
            StopCompletely = mvp.StopCompletely;
            // - disable jump during link

            DistanceToObject = mvp.DistanceToObject;
            FailDistance = mvp.FailDistance;
            // - desired heading
            MinDistance = mvp.MinDistance;
            Speed = mvp.Speed;
            WalkRunThreshold = mvp.WalkRunThreshold;

            // - context id
            HoldKeyToApply = mvp.HoldKeyToApply;
            // - action stamp
        }

        public void CopyNonFlag(MovementParameters mvp)
        {
            // TODO: review again, take bitflags into consideration

            DistanceToObject = mvp.DistanceToObject;
            FailDistance = mvp.FailDistance;
            DesiredHeading = mvp.DesiredHeading;
            MinDistance = mvp.MinDistance;
            Speed = mvp.Speed;
            WalkRunThreshold = mvp.WalkRunThreshold;

            ContextID = mvp.ContextID;
            HoldKeyToApply = mvp.HoldKeyToApply;
            ActionStamp = mvp.ActionStamp;
        }

        public void get_command(float dist, float heading, ref uint motion, ref HoldKey holdKey, ref bool movingAway)
        {
            if (MoveTowards || !MoveAway)
            {
                if (MoveAway)
                    towards_and_away(dist, heading, ref motion, ref movingAway);
                else
                {
                    if (dist > DistanceToObject)
                    {
                        motion = (uint)MotionCommand.WalkForward;
                        movingAway = false;
                    }
                    else
                        motion = 0;
                }
            }
            else if (MoveAway)
            {
                if (dist < MinDistance)
                {
                    motion = (uint)MotionCommand.WalkForward;
                    movingAway = true;
                }
                else
                    motion = 0;
            }

            if (CanRun && (!CanWalk || dist - DistanceToObject > WalkRunThreshold))
                holdKey = HoldKey.Run;
            else
                holdKey = HoldKey.None;
        }

        public float get_desired_heading(uint motion, bool movingAway)
        {
            switch (motion)
            {
                case (uint)MotionCommand.RunForward:
                case (uint)MotionCommand.WalkForward:
                    return movingAway ? 180.0f : 0.0f;
                case (uint)MotionCommand.WalkBackwards:
                    return movingAway ? 0.0f : 180.0f;
                default:
                    return 0.0f;
            }
        }

        public void towards_and_away(float dist, float heading, ref uint command, ref bool movingAway)
        {
            if (dist > DistanceToObject)
            {
                command = (uint)MotionCommand.WalkForward;
                movingAway = false;
            }
            else if (dist - MinDistance < PhysicsGlobals.EPSILON)
            {
                command = (uint)MotionCommand.WalkBackwards;
                movingAway = true;
            }
            else
                command = 0;
        }

        public MovementParameters(MoveToParameters mvp)
        {
            Flags = (MovementParamFlags)mvp.MovementParameters;

            DistanceToObject = mvp.DistanceToObject;
            MinDistance = mvp.MinDistance;
            DesiredHeading = mvp.DesiredHeading;
            Speed = mvp.Speed;
            FailDistance = mvp.FailDistance;
            WalkRunThreshold = mvp.WalkRunThreshold;
        }
    }
}
