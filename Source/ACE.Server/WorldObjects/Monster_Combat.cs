using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster combat general functions
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// The current attack target for the monster
        /// </summary>
        public WorldObject AttackTarget;

        /// <summary>
        /// A monster chooses 1 attack height
        /// </summary>
        public AttackHeight? AttackHeight;

        /// <summary>
        /// The next type of attack (melee/range/magic)
        /// </summary>
        public AttackType? CurrentAttack;

        /// <summary>
        /// The maximum distance for the next attack
        /// </summary>
        public float MaxRange;

        /// <summary>
        /// The time when monster can perform its next attack
        /// </summary>
        public double NextAttackTime;

        /// <summary>
        /// Returns true if monster is dead
        /// </summary>
        public bool IsDead => Health.Current <= 0;

        public AttackType GetAttackType()
        {
            // is this monster a spellcaster?
            if (!IsCaster)
                return AttackType.Melee;

            // chance for casting spell
            if (!IsCaster || !RollCastMagic())
                return AttackType.Melee;
            else
                return AttackType.Magic;
        }

        /// <summary>
        /// Switch to attack stance
        /// </summary>
        public void DoAttackStance()
        {
            // TODO: get attack stance based on weapon type
            var attackStance = MotionStance.UaNoShieldAttack;

            var motion = new UniversalMotion(attackStance);
            motion.MovementData.CurrentStyle = (uint)attackStance;
            motion.MovementData.ForwardCommand = (uint)MotionCommand.Ready;

            CurrentMotionState = motion;
            CurrentLandblock.EnqueueBroadcastMotion(this, motion);
        }

        public float GetMaxRange()
        {
            if (CurrentAttack == AttackType.Magic)
            {
                // select a magic spell
                CurrentSpell = GetRandomSpell();

                return GetSpellMaxRange();
            }
            return MaxMeleeRange;   // distance_to_target?
        }

        /// <summary>
        /// Returns TRUE if creature can perform its next attack
        /// </summary>
        /// <returns></returns>
        public bool AttackReady()
        {
            return IsAttackRange() && Timer.CurrentTime >= NextAttackTime;
        }

        /// <summary>
        /// Performs the current attack on the target
        /// </summary>
        public void Attack()
        {
            switch (CurrentAttack)
            {
                case AttackType.Melee:
                    MeleeAttack();
                    break;
                case AttackType.Magic:
                    MagicAttack();
                    break;
            }

            ResetAttack();
        }

        /// <summary>
        /// Called after attack has completed
        /// </summary>
        public void ResetAttack()
        {
            CurrentAttack = null;
            MaxRange = 0.0f;
        }
    }
}
