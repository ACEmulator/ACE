using System;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster AI functions
    /// </summary>
    partial class Creature
    {
        public bool IsMonster;

        /// <summary>
        /// The exclusive state of the monster
        /// </summary>
        public State MonsterState = State.Idle;

        /// <summary>
        /// The exclusive states the monster can be in
        /// </summary>
        public enum State
        {
            Idle,
            Awake
        };

        public void EquipWieldedTreasure()
        {
            var wielded = SelectWieldedTreasure();

            if (wielded != null)
            {
                var weenie = DatabaseManager.World.GetCachedWeenie(wielded.WeenieClassId);
                var wo = WorldObjectFactory.CreateWorldObject(weenie, new ObjectGuid(wielded.Id));

                if (wo == null) return;

                if (wielded.PaletteId != 0)
                    wo.PaletteTemplate = (int)wielded.PaletteId;

                if (wielded.Shade != 0)
                    wo.Shade = wielded.Shade;

                if (wo.ValidLocations != null)
                    TryEquipObject(wo, (int)wo.ValidLocations);

                var combatStance = GetCombatStance();
                //Console.WriteLine($"{Name} equipped {wo.Name} - {combatStance}");
            }
        }

        /// <summary>
        /// Called every ~1 second
        /// </summary>
        public void DoTick()
        {
            Think();
            QueueNextTick();
        }

        /// <summary>
        /// Primary dispatch for monster think
        /// </summary>
        public void Think()
        {
            if (!IsAwake || IsDead) return;

            IsMonster = true;

            if (AttackTarget != null && AttackTarget.IsDestroyed)
            {
                Sleep();
                return;
            }

            if (!AttackTarget.IsVisible(this))
            {
                Sleep();
                return;
            }

            // decide current type of attack
            if (CurrentAttack == null)
            {
                CurrentAttack = GetAttackType();
                MaxRange = GetMaxRange();

                if (CurrentAttack == AttackType.Magic)
                    MaxRange = MaxMeleeRange;   // FIXME: server position sync
            }

            // get distance to target
            var targetDist = GetDistanceToTarget();
            //Console.WriteLine($"{Name} ({Guid}) - Dist: {targetDist}");

            if (Sticky)
                UpdatePosition();

            if (CurrentAttack != AttackType.Missile)
            {
                if (targetDist > MaxRange)
                {
                    // move towards
                    if (!IsTurning && !IsMoving)
                        StartTurn();
                    else
                        Movement();
                }
                else
                {
                    // perform attack
                    if (AttackReady()) Attack();
                }
            }
            else
            {
                if (!IsFacing(AttackTarget))
                {
                    if (!IsTurning)
                        StartTurn();
                }
                else if (targetDist <= MaxRange)
                {
                    // perform attack
                    if (AttackReady()) Attack();
                }
            }
        }

        /// <summary>
        /// Cleans up state on monster death
        /// </summary>
        public void OnDeath()
        {
            IsTurning = false;
            IsMoving = false;

            //SetFinalPosition();
        }
    }
}
