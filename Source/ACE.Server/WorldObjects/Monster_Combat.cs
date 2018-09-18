using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
        public AttackType? CurrentAttack;

        /// <summary>
        /// The maximum distance for the next attack
        /// </summary>
        public float MaxRange;

        /// <summary>
        /// The time when monster can perform its next attack
        /// </summary>
        public DateTime NextAttackTime;

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
            var rng = Physics.Common.Random.RollDice(0, AttackHeights.Count - 1);
            return AttackHeights[rng];
        }

        public virtual AttackType GetAttackType()
        {
            if (CombatTable == null)
                GetCombatTable();

            if (IsRanged)
                return AttackType.Missile;

            // if caster, roll for spellcasting chance
            if (!IsCaster || !RollCastMagic())
                return AttackType.Melee;
            else
                return AttackType.Magic;
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
        public float DoAttackStance()
        {
            var combatMode = IsRanged ? CombatMode.Missile : CombatMode.Melee;

            return SetCombatMode(combatMode);
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
            return IsAttackRange() && DateTime.UtcNow >= NextAttackTime;
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
            var weapon = GetEquippedWeapon();

            if (weapon != null)
                return GetDamageType();
            else
                return (DamageType)attackPart.DType;
        }

        /// <summary>
        /// Simplified player take damage function, only called for DoTs currently
        /// </summary>
        public virtual void TakeDamageOverTime(WorldObject source, float amount)
        {
            TakeDamage(source, amount);

            // splatter effects
            EnqueueBroadcast(new GameMessageSound(Guid, Sound.HitFlesh1, 0.5f));
            var playerSource = source as Player;
            if (playerSource != null)
            {
                var splatter = (PlayScript)Enum.Parse(typeof(PlayScript), "Splatter" + playerSource.GetSplatterHeight() + playerSource.GetSplatterDir(this));
                EnqueueBroadcast(new GameMessageScript(Guid, splatter));

                playerSource.Session.Network.EnqueueSend(new GameEventUpdateHealth(playerSource.Session, Guid.Full, (float)Health.Current / Health.MaxValue));
            }

            if (Health.Current > 0)
            {
                if (amount >= Health.MaxValue * 0.25f)
                {
                    var painSound = (Sound)Enum.Parse(typeof(Sound), "Wound" + Physics.Common.Random.RollDice(1, 3), true);
                    EnqueueBroadcast(new GameMessageSound(Guid, painSound, 1.0f));
                }

                // notify player attacker?
                if (playerSource != null)
                {
                    var text = new GameMessageSystemChat($"You bleed {Name} for {amount} points of periodic damage!", ChatMessageType.CombatSelf);
                    playerSource.Session.Network.EnqueueSend(text);
                }
            }
        }

        /// <summary>
        /// Applies some amount of damage to this monster from source
        /// </summary>
        /// <param name="source">The attacker / source of damage</param>
        /// <param name="amount">The amount of damage rounded</param>
        public virtual void TakeDamage(WorldObject source, float amount, bool crit = false)
        {
            var tryDamage = (uint)Math.Round(amount);
            var damage = (uint)-UpdateVitalDelta(Health, (int)-tryDamage);

            // TODO: update monster stamina?

            DamageHistory.Add(source, damage);

            if (Health.Current <= 0)
            {
                OnDeath();
                Die();

                var player = source as Player;
                if (player != null)
                {
                    var deathMessage = GetDeathMessage(source, crit);
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(deathMessage, Name), ChatMessageType.Broadcast));
                    player.EarnXP((long)XpOverride);    // TODO: split xp between players in damage history?
                }
            }
        }
    }
}
