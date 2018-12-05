using System;
using ACE.Server.Entity;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Determines when a monster wakes up from idle state
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// Determines when a monster will attack
        /// </summary>
        [Flags]
        public enum Tolerance
        {
            None        = 0,  // attack targets in range
            NoAttack    = 1,  // never attack
            ID          = 2,  // attack when ID'd or attacked
            Unknown     = 4,  // unused?
            Provoke     = 8,  // used in conjunction with 32
            Unknown2    = 16, // unused?
            Target      = 32, // only target original attacker
            Retaliate   = 64  // only attack after attacked
        };

        /// <summary>
        /// Determines when a monster wakes up from idle state
        /// </summary>
        public const float RadiusAwareness = 35.0f;
        public const float RadiusAwarenessSquared = RadiusAwareness * RadiusAwareness;

        /// <summary>
        /// Monsters wake up when players are in visual range
        /// </summary>
        public bool IsAwake = false;

        /// <summary>
        /// Transitions a monster from idle to awake state
        /// </summary>
        public void WakeUp()
        {
            MonsterState = State.Awake;
            IsAwake = true;
            //DoAttackStance();
            EmoteManager.OnAttack(AttackTarget as Creature);
        }

        /// <summary>
        /// Transitions a monster from awake to idle state
        /// </summary>
        public void Sleep()
        {
            // temporary
            if (IsPet) return;

            AttackTarget = null;
            IsAwake = false;
            IsMoving = false;
            MonsterState = State.Idle;
        }

        public bool FindNextTarget()
        {
            if (!IsPet)
                return false;

            // rebuild visible objects (handle this better for monsters)
            PhysicsObj.ObjMaint.RemoveAllObjects();
            PhysicsObj.handle_visible_cells();

            return PetFindTarget() != null;
        }
    }
}
