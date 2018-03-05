using ACE.Server.Physics.Combat;

namespace ACE.Server.Physics.Common
{
    public class WeenieObject
    {
        public double UpdateTime;

        public bool CanJump(float extent)
        {
            return false;
        }

        public bool InqJumpVelocity(float extent, ref float velocityZ)
        {
            return false;
        }

        public bool InqRunRate(ref float rate)
        {
            return false;
        }

        public bool IsCorpse()
        {
            return false;
        }

        public bool IsImpenetable()
        {
            return false;
        }

        public bool IsPK()
        {
            return false;
        }

        public bool IsPKLite()
        {
            return false;
        }

        public bool IsPlayer()
        {
            return false;
        }

        public bool IsCreature()
        {
            return false;
        }

        public bool IsStorage()
        {
            return false;
        }

        public int JumpStaminaCost(float extent, float staminaCost)
        {
            return -1;
        }

        public void DoCollision(AtkCollisionProfile prof)
        {

        }

        public void DoCollisionEnd(int objectID)
        {

        }

        public void OnMotionDone(int motionID, bool success)
        {

        }
    }
}
