using System;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster AI functions
    /// </summary>
    partial class Creature
    {
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
