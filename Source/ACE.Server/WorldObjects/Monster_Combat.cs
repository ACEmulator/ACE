using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

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
        public CombatType? CurrentAttack;

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

        /// <summary>
        /// The list of combat maneuvers performable by this monster
        /// </summary>
        public CombatManeuverTable CombatTable;

        /// <summary>
        /// A list of possible attack heights for this monster,
        /// as determined by the combat maneuvers table
        /// </summary>
        private List<AttackHeight> _attackHeights;

        public List<AttackHeight> AttackHeights
        {
            get
            {
                if (CombatTable == null) return null;

                if (_attackHeights == null)
                    _attackHeights = CombatTable.CMT.Select(m => (AttackHeight)m.AttackHeight).Distinct().ToList();

                return _attackHeights;
            }
        }

        /// <summary>
        /// Selects a random attack height for the next attack
        /// </summary>
        public AttackHeight ChooseAttackHeight()
        {
            var rng = ThreadSafeRandom.Next(0, AttackHeights.Count - 1);
            return AttackHeights[rng];
        }

        public CombatType GetNextAttackType()
        {
            if (CombatTable == null)
                GetCombatTable();

            if (IsRanged)
                return CombatType.Missile;

            // if caster, roll for spellcasting chance
            if (!IsCaster || !RollCastMagic())
                return CombatType.Melee;
            else
                return CombatType.Magic;
        }

        /// <summary>
        /// Reads the combat maneuvers table from the DAT file
        /// </summary>
        public void GetCombatTable()
        {
            if (CombatTableDID != null)
                CombatTable = DatManager.PortalDat.ReadFromDat<CombatManeuverTable>(CombatTableDID.Value);
        }

        /// <summary>
        /// Switch to attack stance
        /// </summary>
        public void DoAttackStance()
        {
            var combatMode = IsRanged ? CombatMode.Missile : CombatMode.Melee;

            var stanceTime = SetCombatMode(combatMode);

            var nextTime = Timers.RunningTime + stanceTime;

            if (NextMoveTime > Timers.RunningTime)
                NextMoveTime += stanceTime;
            else
                NextMoveTime = nextTime;

            if (NextAttackTime > Timers.RunningTime)
                NextAttackTime += stanceTime;
            else
                NextAttackTime = nextTime;

            if (IsRanged)
                NextAttackTime += 1.0f;

            if (DebugMove)
                Console.WriteLine($"[{Timers.RunningTime}] - {Name} ({Guid}) - DoAttackStance - stanceTime: {stanceTime}, isAnimating: {IsAnimating}");

            PhysicsObj.StartTimer();
        }

        public float GetMaxRange()
        {
            while (CurrentAttack == CombatType.Magic)
            {
                // select a magic spell
                CurrentSpell = GetRandomSpell();
                var currentSpell = GetCurrentSpell();

                if (currentSpell.IsProjectile)
                {
                    // ensure direct los
                    if (!IsDirectVisible(AttackTarget))
                    {
                        // reroll attack type
                        CurrentAttack = GetNextAttackType();
                        continue;

                        // max iterations to melee?
                    }
                }
                return GetSpellMaxRange();
            }

            if (CurrentAttack == CombatType.Missile)
            {
                /*var weapon = GetEquippedWeapon();
                if (weapon == null) return MaxMissileRange;

                var maxRange = weapon.GetProperty(PropertyInt.WeaponRange) ?? MaxMissileRange;
                return Math.Min(maxRange, MaxMissileRange);     // in-game cap @ 80 yds.*/
                return GetMaxMissileRange();
            }
            else
                return MaxMeleeRange;   // distance_to_target?
        }

        public bool MoveReady()
        {
            if (Timers.RunningTime < NextMoveTime)
                return false;

            PhysicsObj.update_object();

            return !PhysicsObj.IsAnimating;
        }

        /// <summary>
        /// Returns TRUE if creature can perform its next attack
        /// </summary>
        /// <returns></returns>
        public bool AttackReady()
        {
            if (Timers.RunningTime < NextAttackTime || !IsAttackRange())
                return false;

            PhysicsObj.update_object();

            return !PhysicsObj.IsAnimating;
        }

        /// <summary>
        /// Performs the current attack on the target
        /// </summary>
        public void Attack()
        {
            if (DebugMove)
                Console.WriteLine($"[{Timers.RunningTime}] - {Name} ({Guid}) - Attack");

            switch (CurrentAttack)
            {
                case CombatType.Melee:
                    MeleeAttack();
                    break;
                case CombatType.Missile:
                    RangeAttack();
                    break;
                case CombatType.Magic:
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
            if (CurrentAttack == CombatType.Missile)
                return;

            IsTurning = false;
            IsMoving = false;

            CurrentAttack = null;
            MaxRange = 0.0f;
        }

        public DamageType GetDamageType(BiotaPropertiesBodyPart attackPart)
        {
            var weapon = GetEquippedWeapon();

            if (weapon != null)
                return GetDamageType();
            else
                return (DamageType)attackPart.DType;
        }

        /// <summary>
        /// Simplified monster take damage over time function, only called for DoTs currently
        /// </summary>
        public virtual void TakeDamageOverTime(float amount, DamageType damageType)
        {
            if (IsDead) return;

            TakeDamage(null, damageType, amount);

            // splatter effects
            var hitSound = new GameMessageSound(Guid, Sound.HitFlesh1, 0.5f);
            //var splatter = (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + playerSource.GetSplatterHeight() + playerSource.GetSplatterDir(this));
            var splatter = new GameMessageScript(Guid, damageType == DamageType.Nether ? ACE.Entity.Enum.PlayScript.HealthDownVoid : ACE.Entity.Enum.PlayScript.DirtyFightingDamageOverTime);
            EnqueueBroadcast(hitSound, splatter);

            if (Health.Current <= 0) return;

            if (amount >= Health.MaxValue * 0.25f)
            {
                var painSound = (Sound)Enum.Parse(typeof(Sound), "Wound" + ThreadSafeRandom.Next(1, 3), true);
                EnqueueBroadcast(new GameMessageSound(Guid, painSound, 1.0f));
            }
        }

        /// <summary>
        /// Notifies the damage over time (DoT) source player of the tick damage amount
        /// </summary>
        public void TakeDamageOverTime_NotifySource(Player source, DamageType damageType, float amount)
        {
            var iAmount = (uint)Math.Round(amount);

            // damage text notification
            GameMessageSystemChat text = null;

            if (damageType == DamageType.Nether)
            {
                string verb = null, plural = null;
                var percent = amount / Health.MaxValue;
                Strings.GetAttackVerb(damageType, percent, ref verb, ref plural);
                text = new GameMessageSystemChat($"You {verb} {Name} for {iAmount} points of periodic nether damage!", ChatMessageType.Magic);
            }
            else
                text = new GameMessageSystemChat($"You bleed {Name} for {iAmount} points of periodic damage!", ChatMessageType.CombatSelf);

            var updateHealth = new GameEventUpdateHealth(source.Session, Guid.Full, (float)Health.Current / Health.MaxValue);

            source.Session.Network.EnqueueSend(text, updateHealth);
        }

        /// <summary>
        /// Applies some amount of damage to this monster from source
        /// </summary>
        /// <param name="source">The attacker / source of damage</param>
        /// <param name="amount">The amount of damage rounded</param>
        public virtual void TakeDamage(WorldObject source, DamageType damageType, float amount, bool crit = false)
        {
            var tryDamage = (uint)Math.Round(amount);
            var damage = (uint)-UpdateVitalDelta(Health, (int)-tryDamage);

            // TODO: update monster stamina?

            // source should only be null for combined DoT ticks from multiple sources
            if (source != null)
                DamageHistory.Add(source, damageType, damage);

            if (Health.Current <= 0)
            {
                OnDeath(DamageHistory.LastDamager, damageType, crit);

                Die();
            }
        }

        public void EmitSplatter(Creature target, float damage)
        {
            if (target.IsDead) return;

            target.EnqueueBroadcast(new GameMessageSound(target.Guid, Sound.HitFlesh1, 0.5f));
            if (damage >= target.Health.MaxValue * 0.25f)
            {
                var painSound = (Sound)Enum.Parse(typeof(Sound), "Wound" + ThreadSafeRandom.Next(1, 3), true);
                target.EnqueueBroadcast(new GameMessageSound(target.Guid, painSound, 1.0f));
            }
            var splatter = (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + GetSplatterHeight() + GetSplatterDir(target));
            target.EnqueueBroadcast(new GameMessageScript(target.Guid, splatter));
        }
    }
}
