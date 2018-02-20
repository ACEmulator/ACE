using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public class MotionTableManager
    {
        public PhysicsObj PhysicsObj;
        public MotionTable Table;
        public MotionState State;
        public int AnimationCounter;
        public List<AnimSequenceNode> PendingAnimations;
    }
}
