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

        public bool InqJumpVelocity(float extent, out float velocity_z)
        {
            velocity_z = 0.0f;

            var player = WorldObject as Player;

            if (player == null)
                return false;

            var burden = InqBurden();
            if (burden == null)
                return false;

            var stamina = player.Stamina.Current;

            var jumpSkill = player.GetCreatureSkill(Skill.Jump).Current;

            if (stamina == 0)
                jumpSkill = 0;

            var height = MovementSystem.GetJumpHeight((float)burden, jumpSkill, extent, 1.0f);

            velocity_z = (float)Math.Sqrt(height * 19.6);

            return true;
        }

        /// <summary>
        /// Returns the player's load / burden as a percentage,
        /// usually in the range 0.0 - 3.0 (max 300% typically)
        /// </summary>
        public float? InqBurden()
        {
            var player = WorldObject as Player;

            if (player == null)
                return null;

            var strength = (int)player.Strength.Current;

            var numAugs = player.AugmentationIncreasedCarryingCapacity;

            var capacity = EncumbranceSystem.EncumbranceCapacity(strength, numAugs);

            var encumbrance = player.EncumbranceVal ?? 0;

            var burden = EncumbranceSystem.GetBurden(capacity, encumbrance);

            return burden;
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
            return WorldObject is Creature;

            //return true;
        }

        public bool IsStorage()
        {
            return false;
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
            WorldObject.HandleMotionDone(motionID, success);
        }

        public void OnMoveComplete(WeenieError status, int cycles)
        {
            WorldObject.OnMoveComplete(status, cycles);
        }

        public void OnSticky()
        {
            WorldObject.OnSticky();
        }

        public void OnUnsticky()
        {
            WorldObject.OnUnsticky();
        }
    }
}
