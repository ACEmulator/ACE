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

        public void UseTime()
        {

        }

        public void InitializeState(Sequence sequence)
        {

        }

        public int GetMotionTableID(int mtableID)
        {
            return -1;
        }

        public int PerformMovement(MovementStruct mvs, Sequence sequence)
        {
            return -1;
        }

        public int AnimationDone(int success)
        {
            return -1;
        }

        public void CheckForCompletedMotions()
        {

        }
    }
}
