using System;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;

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

        public virtual AttackType GetAttackType()
        {
            if (IsRanged)
                return AttackType.Missile;

            // if caster, roll for spellcasting chance
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
            var combatMode = IsRanged ? CombatMode.Missile : CombatMode.Melee;

            if (IsRanged)
                GiveAmmo();

            SetCombatMode(combatMode);
        }

        public float GetMaxRange()
        {
            if (CurrentAttack == AttackType.Magic)
            {
                // select a magic spell
                CurrentSpell = GetRandomSpell();

                return GetSpellMaxRange();
            }
            else if (CurrentAttack == AttackType.Missile)
            {
                var weapon = GetEquippedWeapon();
                if (weapon == null) return MaxMissileRange;

                var maxRange = weapon.GetProperty(PropertyInt.WeaponRange) ?? MaxMissileRange;
                return Math.Min(maxRange, MaxMissileRange);     // in-game cap @ 80 yds.
            }
            else
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
            if (DebugMove)
                Console.WriteLine(Name + " Attack");

            switch (CurrentAttack)
            {
                case AttackType.Melee:
                    MeleeAttack();
                    break;
                case AttackType.Missile:
                    RangeAttack();
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
            // wait for missile to strike
            if (CurrentAttack == AttackType.Missile)
                return;

            IsTurning = false;
            IsMoving = false;

            CurrentAttack = null;
            MaxRange = 0.0f;
        }

        public DamageType GetDamageType(BiotaPropertiesBodyPart attackPart)
        {
            if (CurrentAttack != AttackType.Missile)
                return (DamageType)attackPart.DType;
            else
                return GetDamageType(false);
        }
    }
}
