using System;
using ACE.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Combat;
using ACE.Server.Physics.Collision;
using ACE.Server.WorldObjects;

namespace ACE.Server.Physics.Common
{
    public class WeenieObject
    {
        public uint ID;
        public double UpdateTime;
        public WorldObject WorldObject;

        public WeenieObject() { }

        public WeenieObject(WorldObject worldObject)
        {
            WorldObject = worldObject;
        }

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
            return true;
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

        public int DoCollision(AtkCollisionProfile prof, ObjectGuid guid, PhysicsObj obj)
        {
            if (WorldObject != null)
                obj.WeenieObj.WorldObject.HandleActionOnCollide(guid);

            return 0;
        }

        public int DoCollision(EnvCollisionProfile prof, ObjectGuid guid, PhysicsObj obj)
        {
            if (WorldObject != null)
                obj.WeenieObj.WorldObject.HandleActionOnCollide(guid);

            return 0;
        }

        public void DoCollisionEnd(ObjectGuid guid)
        {
            if (WorldObject != null)
                WorldObject.HandleActionOnCollideEnd(guid);
        }

        public void OnMotionDone(uint motionID, bool success)
        {

        }
    }
}
