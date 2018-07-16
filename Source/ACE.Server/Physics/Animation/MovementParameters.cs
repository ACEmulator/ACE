namespace ACE.Server.Physics.Animation
{
    public class MovementParameters
    {
        public uint Bitfield;
        public bool CanWalk;    
        public bool CanRun;
        public bool CanSidestep;
        public bool CanWalkBackwards;
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
            MinDistance = Default_MinDistance;
            DistanceToObject = Default_DistanceToObject;
            FailDistance = Default_FailDistance;
            Speed = Default_Speed;
            WalkRunThreshold = Default_WalkRunThreshold;
            HoldKeyToApply = HoldKey.Invalid;
            StopCompletely = true;
            CancelMoveTo = true;
            ModifyInterpretedState = true;
            ModifyRawState = true;
            SetHoldKey = true;
            UseSpheres = true;
            MoveTowards = true;
            CanWalkBackwards = true;
            CanSidestep = true;
            CanRun = true;
            CanWalk = true;
            Bitfield = 0x1EE0F;     // todo: union of bools
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
                        motion = 0x45000005;
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
                    motion = 0x45000005;
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
                case 0x44000007:
                case 0x45000005:
                    return movingAway ? 180.0f : 0.0f;
                case 0x45000006:
                    return movingAway ? 0.0f : 180.0f;
                default:
                    return 0.0f;
            }
        }

        public void towards_and_away(float dist, float heading, ref uint command, ref bool movingAway)
        {
            if (dist > DistanceToObject)
            {
                command = 0x45000005;
                movingAway = false;
            }
            else if (dist - MinDistance < PhysicsGlobals.EPSILON)
            {
                command = 0x45000006;
                movingAway = true;
            }
            else
                command = 0;
        }
    }
}
