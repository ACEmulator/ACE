using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Combat;

namespace ACE.Server.Physics.Common
{
    public class WeenieObject
    {
        public int ID;
        public double UpdateTime;

        // connect to ACE wobj

        public bool CanJump(float extent)
        {
            return true;
        }

        public bool InqJumpVelocity(float extent, ref float velocityZ)
        {
            velocityZ = MovementSystem.GetJumpHeight(1.0f, 100, 1.0f, 1.0f) * 19.6f;
            return true;
        }

        public bool InqRunRate(ref float rate)
        {
            // get run skill from WorldObject
            rate = (float)MovementSystem.GetRunRate(0.0f, 300, 1.0f);
            return true;
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
            return true;
        }

        public bool IsStorage()
        {
            return false;
        }

        public float JumpStaminaCost(float extent, int staminaCost)
        {
            return 0;
        }

        public int DoCollision(AtkCollisionProfile prof)
        {
            return 0;
        }

        public void DoCollisionEnd(int objectID)
        {

        }

        public void OnMotionDone(uint motionID, bool success)
        {

        }
    }
}
