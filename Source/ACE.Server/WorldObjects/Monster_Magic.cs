using System;
using System.Collections.Generic;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics.Animation;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Monster casting for magic spells
    /// </summary>
    partial class Creature
    {
        /// <summary>
        /// Returns TRUE if monster is a spell caster
        /// </summary>
        public bool IsCaster { get => Biota.HasKnownSpell(BiotaDatabaseLock); }

        /// <summary>
        /// The next spell the monster will attempt to cast
        /// </summary>
        public KeyValuePair<int, float> CurrentSpell { get; set; }

        /// <summary>
        /// The delay after casting a magic spell
        /// </summary>
        public static readonly float MagicDelay = 2.0f;

        /// <summary>
        /// Returns the monster's current magic skill
        /// for the school containing the current spell
        /// </summary>
        public uint GetMagicSkill()
        {
            var currentSpell = GetCurrentSpell();
            return GetCreatureSkill(currentSpell.School).Current;
        }

        /// <summary>
        /// Returns the magic skill level used for spell range checks.
        /// (initial points + points due to directly raising the skill)
        /// </summary>
        /// <returns></returns>
        public uint GetMagicSkillForRangeCheck()
        {
            var currentSpell = GetCurrentSpell();
            var skill = GetCreatureSkill(currentSpell.School);
            return skill.InitLevel + skill.Ranks;
        }

        public float GetProbabilityAny()
        {
            var probabilities = new List<float>();

            foreach (var spell in Biota.GetKnownSpellsProbabilities(BiotaDatabaseLock))
            {
                var probability = spell > 2.0f ? spell - 2.0f : spell / 100.0f;

                probabilities.Add(probability);
            }

            return Probability.GetProbabilityAny(probabilities);
        }

        public Spell TryRollSpell()
        {
            CurrentSpell = new KeyValuePair<int, float>();

            //Console.WriteLine($"{Name}.TryRollSpell(), probability={GetProbabilityAny()}");

            // monster spellbooks have probabilities with base 2.0
            // ie. a 5% chance would be 2.05 instead of 0.05

            // much less common, some monsters will have spells with just base 2.0 probability
            // there were probably other criteria used to select these spells (emote responses, monster ai responses)
            // for now, 2.0 base just becomes a 2% chance

            if (Biota.PropertiesSpellBook == null)
                return null;

            // We don't use thread safety here. Monster spell books aren't mutated cross-threads.
            // This reduces memory consumption by not cloning the spell book every single TryRollSpell()
            //foreach (var spell in Biota.CloneSpells(BiotaDatabaseLock)) // Thread-safe
            foreach (var spell in Biota.PropertiesSpellBook) // Not thread-safe
            {
                var probability = spell.Value > 2.0f ? spell.Value - 2.0f : spell.Value / 100.0f;

                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                if (rng < probability)
                {
                    CurrentSpell = spell;
                    return new Spell(spell.Key);
                }
            }

            return null;
        }

        // todo: monster spellcasting anim speed?
        public static float CastSpeed = 1.5f;

        /// <summary>
        /// Perform the first part of monster spell casting animation - spreading arms out
        /// </summary>
        public float PreCastMotion(WorldObject target)
        {
            var motion = new Motion(this, MotionCommand.CastSpell, CastSpeed);
            motion.MotionState.TurnSpeed = 2.25f;
            //motion.HasTarget = true;
            //motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);

            return MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.CastSpell, CastSpeed);
        }

        /// <summary>
        /// Perform the animations after casting a spell,
        /// ie. moving arms back in, returning to previous stance
        /// </summary>
        public float PostCastMotion()
        {
            var motion = new Motion(this, MotionCommand.Ready, CastSpeed);
            motion.MotionState.TurnSpeed = 2.25f;
            //motion.HasTarget = true;
            //motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);

            return MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.CastSpell, MotionCommand.Ready, CastSpeed);
        }

        /// <summary>
        /// Performs the monster windup spell animation,
        /// casts the spell, and returns to attack stance
        /// </summary>
        public void MagicAttack()
        {
            var target = AttackTarget as Creature;

            if (target == null || !target.IsAlive)
            {
                FindNextTarget();
                return;
            }

            var spell = GetCurrentSpell();
            //Console.WriteLine(spell.Name);
            //Console.WriteLine($"BaseRangeConstant: {spell.BaseRangeConstant}, BaseRangeMod: {spell.BaseRangeMod}");
            //Console.WriteLine($"MaxRange: {GetSpellMaxRange()}");

            if (AiUsesMana && !UseMana()) return;

            var preCastTime = PreCastMotion(AttackTarget);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(preCastTime);
            actionChain.AddAction(this, () =>
            {
                if (IsDead || AttackTarget == null || target.IsDead)
                    return;

                CastSpell(spell);
                PostCastMotion();
            });
            actionChain.EnqueueChain();

            var postCastTime = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.CastSpell, MotionCommand.Ready, 1.5f);
            var animTime = preCastTime + postCastTime;

            //Console.WriteLine($"{Name}.MagicAttack(): preCastTime({preCastTime}), postCastTime({postCastTime})");

            NextAttackTime = Timers.RunningTime + animTime + MagicDelay;
            NextMoveTime = NextAttackTime - 1.0f;
        }

        /// <summary>
        /// Casts the current monster spell on target
        /// </summary>
        public void CastSpell(Spell spell)
        {
            if (AttackTarget == null) return;

            var targetSelf = spell.Flags.HasFlag(SpellFlags.SelfTargeted);
            var untargeted = spell.NonComponentTargetType == ItemType.None;

            var target = AttackTarget;
            if (untargeted)
                target = null;
            else if (targetSelf)
                target = this;

            // try to resist spell, if applicable
            if (TryResistSpell(target, spell))
                return;

            switch (spell.School)
            {
                case MagicSchool.WarMagic:

                    WarMagic(target, spell, this);
                    break;

                case MagicSchool.LifeMagic:

                    var targetDeath = LifeMagic(spell, out uint damage, out bool critical, out var msg, target);
                    if (targetDeath && target is Creature targetCreature)
                    {
                        targetCreature.OnDeath(new DamageHistoryInfo(this), DamageType.Health, false);
                        targetCreature.Die();
                    }
                    if (target != null)
                        EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));

                    break;

                case MagicSchool.CreatureEnchantment:

                    CreatureMagic(target, spell);

                    if (target != null)
                        EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));

                    break;

                case MagicSchool.VoidMagic:

                    VoidMagic(target, spell, this);

                    if (spell.NumProjectiles == 0 && target != null)
                        EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));

                    break;
            }
        }

        /// <summary>
        /// Returns the maximum range for the current spell
        /// </summary>
        public float GetSpellMaxRange()
        {
            var spell = GetCurrentSpell();
            var skill = GetMagicSkillForRangeCheck();

            var maxRange = spell.BaseRangeConstant + skill * spell.BaseRangeMod;
            if (maxRange == 0.0f)
                maxRange = float.PositiveInfinity;

            return maxRange;
        }

        /// <summary>
        /// Returns the current Spell for the monster
        /// </summary>
        public Spell GetCurrentSpell()
        {
            return new Spell(CurrentSpell.Key);
        }

        public bool UseMana()
        {
            // do any monsters have mana conversion?
            var currentSpell = GetCurrentSpell();

            var target = GetSpellMaxRange() < float.PositiveInfinity ? AttackTarget : this;

            var manaUsed = CalculateManaUsage(this, currentSpell, target);
            if (manaUsed > Mana.Current)
                return false;

            Mana.Current -= manaUsed;
            return true;
        }

        public bool AiUsesMana
        {
            get => GetProperty(PropertyBool.AiUsesMana) ?? true;    // default?
            set { if (!value) RemoveProperty(PropertyBool.AiUsesMana); else SetProperty(PropertyBool.AiUsesMana, value); }
        }

        public bool IsSelfCast()
        {
            if (CurrentAttack != CombatType.Magic)
                return false;

            return GetSpellMaxRange() == float.PositiveInfinity;
        }
    }
}
