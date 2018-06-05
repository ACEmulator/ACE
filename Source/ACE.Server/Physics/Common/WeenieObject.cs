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

        public int DoCollision(AtkCollisionProfile prof, ObjectGuid guid, PhysicsObj target)
        {
            // no collision with self
            if (WorldObject.Guid.Equals(target.WeenieObj.WorldObject.Guid))
                return -1;

            /*Console.WriteLine("AtkCollisionProfile");
            Console.WriteLine("Source: " + WorldObject.Name);
            Console.WriteLine("Target: " + obj.WeenieObj.WorldObject.Name);*/

            if (WorldObject != null)
                WorldObject.OnCollideObject(target.WeenieObj.WorldObject);

            return 0;
        }

        public int DoCollision(EnvCollisionProfile prof, ObjectGuid guid, PhysicsObj target)
        {
            /*Console.WriteLine("EnvCollisionProfile");
            Console.WriteLine("Source: " + WorldObject.Name);
            Console.WriteLine("Target: " + obj.WeenieObj.WorldObject.Name);*/

            if (WorldObject != null)
                WorldObject.OnCollideEnvironment();

            return 0;
        }

        public void DoCollisionEnd(ObjectGuid targetGuid)
        {
            var target = WorldObject.CurrentLandblock.GetObject(targetGuid);

            if (WorldObject != null)
                WorldObject.OnCollideObjectEnd(target);
        }

        public void OnMotionDone(uint motionID, bool success)
        {

        }
    }
}
