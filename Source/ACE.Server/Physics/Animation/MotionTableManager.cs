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

        public bool AnimationDone(bool success)
        {
            return false;
        }

        public void CheckForCompletedMotions()
        {

        }

        public static MotionTableManager Create(int mtableID)
        {
            return null;
        }

        public int GetMotionTableID(int mtableID)
        {
            return -1;
        }

        public void HandleEnterWorld()
        {

        }

        public void HandleExitWorld()
        {

        }

        public void InitializeState(Sequence sequence)
        {

        }

        public Sequence PerformMovement(MovementStruct mvs, Sequence sequence)
        {
            return null;
        }

        public void SetPhysicsObject(PhysicsObj obj)
        {

        }

        public void UseTime()
        {

        }
    }
}
