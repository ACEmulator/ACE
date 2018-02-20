using System.Collections.Generic;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class MotionInterp
    {
        public int Initted;
        public WeenieObject WeenieObj;
        public PhysicsObj PhysicsObj;
        public RawMotionState RawState;
        public InterpretedMotionState InterpretedState;
        public float CurrentSpeedFactor;
        public int StandingLongJump;
        public float JumpExtent;
        public int ServerActionStamp;
        public float MyRunRate;
        public List<MotionNode> PendingMotions;
    }
}
