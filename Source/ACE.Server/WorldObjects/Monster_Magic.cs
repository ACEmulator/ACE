using System;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
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
        public bool IsCaster { get => Biota.BiotaPropertiesSpellBook.Count > 0; }

        /// <summary>
        /// The next spell the monster will attempt to cast
        /// </summary>
        public BiotaPropertiesSpellBook CurrentSpell { get; set; }

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

        /// <summary>
        /// Returns the sum of all probabilities from monster's spell_book
        /// </summary>
        public float GetSpellProbability()
        {
            var probability = 0.0f;

            foreach (var spell in Biota.BiotaPropertiesSpellBook)
                probability += spell.Probability;

            return probability;
        }

        /// <summary>
        /// Rolls for a chance to cast magic spell
        /// </summary>
        public bool RollCastMagic()
        {
            var probability = GetSpellProbability();
            //Console.WriteLine("Spell probability: " + probability);

            var rng = ThreadSafeRandom.Next(0.0f, 100.0f);
            //var rng = ThreadSafeRandom.Next(0.0f, probability);
            return rng < probability;
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
                Sleep();
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

            bool? resisted;
            //var spell = GetCurrentSpell();

            var targetSelf = spell.Flags.HasFlag(SpellFlags.SelfTargeted);
            var target = targetSelf ? this : AttackTarget;

            var player = AttackTarget as Player;
 
            switch (spell.School)
            {
                case MagicSchool.WarMagic:

                    WarMagic(AttackTarget, spell);
                    break;

                case MagicSchool.LifeMagic:

                    resisted = ResistSpell(target, spell);
                    if (!targetSelf && (resisted == true)) break;
                    if (resisted == null)
                    {
                        log.Error("Something went wrong with the Magic resistance check");
                        break;
                    }
                    var targetDeath = LifeMagic(target, spell, out uint damage, out bool critical, out var msg);
                    if (targetDeath && target is Creature targetCreature)
                    {
                        targetCreature.OnDeath(this, DamageType.Health, false);
                        targetCreature.Die();
                    }
                    EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                    break;

                case MagicSchool.CreatureEnchantment:

                    resisted = ResistSpell(target, spell);
                    if (!targetSelf && (resisted == true)) break;
                    if (resisted == null)
                    {
                        log.Error("Something went wrong with the Magic resistance check");
                        break;
                    }
                    CreatureMagic(target, spell);
                    EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                    break;

                case MagicSchool.VoidMagic:

                    if (spell.NumProjectiles == 0)
                    {
                        resisted = ResistSpell(target, spell);
                        if (!targetSelf && (resisted == true)) break;
                        if (resisted == null)
                        {
                            log.Error("Something went wrong with the Magic resistance check");
                            break;
                        }
                    }
                    VoidMagic(AttackTarget, spell);
                    if (spell.NumProjectiles == 0)
                        EnqueueBroadcast(new GameMessageScript(target.Guid, spell.TargetEffect, spell.Formula.Scale));
                    break;
            }
        }

        /// <summary>
        /// Selects a random spell from the monster's spell book
        /// according to the probabilities
        /// </summary>
        public BiotaPropertiesSpellBook GetRandomSpell()
        {
            var probability = GetSpellProbability();
            var rng = ThreadSafeRandom.Next(0.0f, probability);

            var currentSpell = 0.0f;
            foreach (var spell in Biota.BiotaPropertiesSpellBook)
            {
                if (rng < currentSpell + spell.Probability)
                    return spell;

                currentSpell += spell.Probability;
            }
            return Biota.BiotaPropertiesSpellBook.Last();
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
            return new Spell(CurrentSpell.Spell);
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
