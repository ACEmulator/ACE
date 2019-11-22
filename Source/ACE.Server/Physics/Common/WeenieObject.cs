using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
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
        public readonly WorldObjectInfo WorldObjectInfo;
        public WorldObject WorldObject => WorldObjectInfo?.TryGetWorldObject();

        public bool IsMonster;

        public bool IsCombatPet;

        public WeenieObject() { }

        public WeenieObject(WorldObject worldObject)
        {
            WorldObjectInfo = new WorldObjectInfo(worldObject);

            IsCombatPet = worldObject is CombatPet;

            IsMonster = worldObject is Creature creature && creature.IsMonster && !IsCombatPet;
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
            uint runSkill = 0;
            if (WorldObject is Creature creature)
                runSkill = creature.GetCreatureSkill(Skill.Run).Current;

            //rate = (float)MovementSystem.GetRunRate(0.0f, 300, 1.0f);
            rate = (float)MovementSystem.GetRunRate(0.0f, (int)runSkill, 1.0f);
            //Console.WriteLine($"{WorldObject.Name} ({WorldObject.Guid}) - WeenieObject.InqRunRate: runSkill = {runSkill}, rate = {rate}");
            return true;
        }

        public bool IsCorpse()
        {
            return WorldObject is Corpse;
        }

        public bool IsImpenetrable()
        {
            return WorldObject is Player player && player.PlayerKillerStatus == PlayerKillerStatus.Free;
        }

        public bool IsPK()
        {
            return WorldObject is Player player && player.IsPK;
        }

        public bool IsPKLite()
        {
            return WorldObject is Player player && player.IsPKL;
        }

        public bool IsPlayer()
        {
            return WorldObject is Player;
        }

        public bool IsCreature()
        {
            return WorldObject is Creature;
        }

        public bool IsStorage()
        {
            return WorldObject is Storage;
        }

        public float JumpStaminaCost(float extent, int staminaCost)
        {
            return 0;
        }

        public void InqCollisionProfile(ObjCollisionProfile prof)
        {
            prof.WCID = ID;
            prof.ItemType = WorldObject.ItemType;

            if (WorldObject is Creature)
                prof.Flags |= ObjCollisionProfileFlags.Creature;

            if (WorldObject is Player)
                prof.Flags |= ObjCollisionProfileFlags.Player;

            if (WorldObject.Attackable)
                prof.Flags |= ObjCollisionProfileFlags.Attackable;

            if (WorldObject is Door)
                prof.Flags |= ObjCollisionProfileFlags.Door;
        }

        public int DoCollision(ObjCollisionProfile prof, ObjectGuid guid, PhysicsObj target)
        {
            var wo = WorldObject;

            if (wo == null)
                return -1;

            var targetWO = target.WeenieObj.WorldObject;

            if (targetWO == null)
                return -1;

            // no collision with self
            if (wo.Guid.Equals(targetWO.Guid))
                return -1;

            /*Console.WriteLine("ObjCollisionProfile");
            Console.WriteLine("Source: " + WorldObject.Name);
            Console.WriteLine("Target: " + obj.WeenieObj.WorldObject.Name);*/

            wo.OnCollideObject(targetWO);

            return 0;
        }

        public int DoCollision(EnvCollisionProfile prof, ObjectGuid guid, PhysicsObj target)
        {
            /*Console.WriteLine("EnvCollisionProfile");
            Console.WriteLine("Source: " + WorldObject.Name);
            Console.WriteLine("Target: " + target.WeenieObj.WorldObject.Name);
            Console.WriteLine("Velocity: " + prof.Velocity);*/

            var wo = WorldObject;

            if (wo == null)
                return 0;

            if (wo is Player player)
                player.HandleFallingDamage(prof);
            else
                wo.OnCollideEnvironment();

            return 0;
        }

        public void DoCollisionEnd(ObjectGuid targetGuid)
        {
            var wo = WorldObject;

            if (wo == null)
                return;

            var target = wo.CurrentLandblock?.GetObject(targetGuid);

            if (target != null)
                wo.OnCollideObjectEnd(target);
        }

        public void OnMotionDone(uint motionID, bool success)
        {

        }
    }
}
