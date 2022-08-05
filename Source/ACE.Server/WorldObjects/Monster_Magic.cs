using System;
using System.Collections.Generic;

using ACE.Common;
using ACE.DatLoader;
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
        private bool AiUsesMana
        {
            get => GetProperty(PropertyBool.AiUsesMana) ?? true;    // default true?
            set { if (!value) RemoveProperty(PropertyBool.AiUsesMana); else SetProperty(PropertyBool.AiUsesMana, value); }
        }

        /// <summary>
        /// Very specific monsters will be set to use the human casting animations,
        /// ie. the windup and casting gestures from the spell
        /// </summary>
        private bool AiUseHumanMagicAnimations
        {
            get => GetProperty(PropertyBool.AiUseHumanMagicAnimations) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.AiUseHumanMagicAnimations); else SetProperty(PropertyBool.AiUseHumanMagicAnimations, value); }
        }

        /// <summary>
        /// The amount of time a monster waits to cast a magic spell
        /// defined as seconds from the start of the previous attack
        /// the most common value in the db is 3s
        /// some other common values include 2s and 1s, with some mobs having values up to 1m
        /// </summary>
        private double? AiUseMagicDelay
        {
            get => GetProperty(PropertyFloat.AiUseMagicDelay);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.AiUseMagicDelay); else SetProperty(PropertyFloat.AiUseMagicDelay, value.Value); }
        }

        /// <summary>
        /// Returns TRUE if monster has known spells
        /// </summary>
        private bool HasKnownSpells => Biota.HasKnownSpell(BiotaDatabaseLock);

        /// <summary>
        /// The next spell the monster will attempt to cast
        /// </summary>
        private Spell CurrentSpell { get; set; }

        private bool TryRollSpell()
        {
            CurrentSpell = null;

            //Console.WriteLine($"{Name}.TryRollSpell(), probability={GetProbabilityAny()}");

            // monster spellbooks have probabilities with base 2.0
            // ie. a 5% chance would be 2.05 instead of 0.05

            // much less common, some monsters will have spells with just base 2.0 probability
            // there were probably other criteria used to select these spells (emote responses, monster ai responses)
            // for now, 2.0 base just becomes a 2% chance

            if (Biota.PropertiesSpellBook == null)
                return false;

            // We don't use thread safety here. Monster spell books aren't mutated cross-threads.
            // This reduces memory consumption by not cloning the spell book every single TryRollSpell()
            //foreach (var spell in Biota.CloneSpells(BiotaDatabaseLock)) // Thread-safe
            foreach (var spell in Biota.PropertiesSpellBook) // Not thread-safe
            {
                var probability = spell.Value > 2.0f ? spell.Value - 2.0f : spell.Value / 100.0f;

                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

                if (rng < probability)
                {
                    CurrentSpell = new Spell(spell.Key);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the probability of this monster casting a spell for an attack
        /// </summary>
        private float GetProbabilityAny()
        {
            var probabilities = new List<float>();

            foreach (var spell in Biota.GetKnownSpellsProbabilities(BiotaDatabaseLock))
            {
                var probability = spell > 2.0f ? spell - 2.0f : spell / 100.0f;

                probabilities.Add(probability);
            }

            return Probability.GetProbabilityAny(probabilities);
        }

        /// <summary>
        /// Returns the maximum range for the current spell
        /// </summary>
        private float GetSpellMaxRange()
        {
            var skill = GetMagicSkillForRangeCheck();

            var maxRange = Math.Min(CurrentSpell.BaseRangeConstant + skill * CurrentSpell.BaseRangeMod, Player.MaxRadarRange_Outdoors);

            if (maxRange == 0.0f)
                maxRange = float.PositiveInfinity;

            return maxRange;
        }

        private bool IsSelfCast()
        {
            if (CurrentAttack != CombatType.Magic)
                return false;

            return GetSpellMaxRange() == float.PositiveInfinity;
        }

        /// <summary>
        /// Performs the monster windup spell animation,
        /// casts the spell, and returns to attack stance
        /// </summary>
        private void MagicAttack()
        {
            var target = AttackTarget as Creature;

            if (target == null || !target.IsAlive)
            {
                FindNextTarget();
                return;
            }

            var spell = CurrentSpell;

            // turn to?
            if (AiUsesMana && !UseMana()) return;

            // spell words
            if (AiUseHumanMagicAnimations)
            {
                var spellWords = spell._spellBase.GetSpellWords(DatManager.PortalDat.SpellComponentsTable);
                if (!string.IsNullOrWhiteSpace(spellWords))
                    EnqueueBroadcast(new GameMessageHearSpeech(spellWords, Name, Guid.Full, ChatMessageType.Spellcasting), LocalBroadcastRange, ChatMessageType.Spellcasting);
            }

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

            var postCastTime = GetPostCastTime(spell);
            var animTime = preCastTime + postCastTime;

            //Console.WriteLine($"{Name}.MagicAttack(): preCastTime({preCastTime}), postCastTime({postCastTime})");

            // slight variation here
            PrevAttackTime = Timers.RunningTime + preCastTime;
            var powerupTime = (float)(PowerupTime ?? 1.0f);

            var postDelay = ThreadSafeRandom.Next(0.0f, powerupTime);

            NextMoveTime = NextAttackTime = PrevAttackTime + postCastTime + postDelay;
        }

        private bool UseMana()
        {
            // do any monsters have mana conversion?
            var target = GetSpellMaxRange() < float.PositiveInfinity ? AttackTarget : this;

            var manaUsed = CalculateManaUsage(this, CurrentSpell, target);

            if (manaUsed > Mana.Current)
                return false;

            Mana.Current -= manaUsed;
            return true;
        }

        private static readonly float PreCastSpeed = 2.0f;
        private static readonly float PostCastSpeed = 1.0f;
        private static readonly float PostCastSpeed_Ranged = 1.66f;  // ??

        /// <summary>
        /// Perform the first part of monster spell casting animation - spreading arms out
        /// </summary>
        public float PreCastMotion(WorldObject target, bool fallback = false)
        {
            if (AiUseHumanMagicAnimations && !fallback)
                return PreCastMotion_Human(target);

            var motion = new Motion(this, MotionCommand.CastSpell, PreCastSpeed);
            motion.MotionState.TurnSpeed = 2.25f;
            //motion.HasTarget = true;
            //motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);

            return MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.CastSpell, PreCastSpeed);
        }

        /// <summary>
        /// For monsters with AiUseHumanMagicAnimations = true,
        /// performs the windup gestures from the spell scarabs
        /// 
        /// <returns>The amount of time for the windup gestures to complete</returns>
        private float PreCastMotion_Human(WorldObject target)
        {
            // todo: play each motion at the proper time,
            // ensuring the monster is still alive at each step
            CurrentSpell.Formula.GetMonsterFormula();

            // FIXME: data
            var castAnimTime = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, CurrentSpell.Formula.CastGesture, PreCastSpeed);

            if (castAnimTime == 0)
                return PreCastMotion(target, true);

            var animTime = 0.0f;

            foreach (var windupGesture in CurrentSpell.Formula.WindupGestures)
            {
                var motion = new Motion(this, windupGesture, PreCastSpeed);
                motion.MotionState.TurnSpeed = 2.25f;
                CurrentMotionState = motion;

                EnqueueBroadcastMotion(motion);

                animTime += MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, windupGesture, PreCastSpeed);
            }

            var castMotion = new Motion(this, CurrentSpell.Formula.CastGesture, PreCastSpeed);
            castMotion.MotionState.TurnSpeed = 2.25f;
            CurrentMotionState = castMotion;

            EnqueueBroadcastMotion(castMotion);

            animTime += castAnimTime;

            return animTime;
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

            var caster = GetEquippedWand();

            // handle self procs
            if (spell.IsHarmful && target != this)
                TryProcEquippedItems(this, this, true, caster);

            // If the target is too far away, don't cast. This checks to see of this monster and the target are on separate landblock groups, and potentially separate threads.
            // This also fixes cross-threading issues
            if (target != null && (CurrentLandblock == null || target.CurrentLandblock == null || CurrentLandblock.CurrentLandblockGroup != target.CurrentLandblock.CurrentLandblockGroup))
                return;

            // try to resist spell, if applicable
            if (TryResistSpell(target, spell))
            {
                TryHandleFactionMob(target);
                return;
            }

            var targetCreature = target as Creature;

            // TODO: see if this can be coalesced
            switch (spell.School)
            {
                case MagicSchool.CreatureEnchantment:

                    HandleCastSpell(spell, target);

                    if (spell.IsHarmful)
                    {
                        // handle target procs
                        if (targetCreature != null && targetCreature != this)
                            TryProcEquippedItems(this, targetCreature, false, caster);
                    }
                    break;

                case MagicSchool.ItemEnchantment:

                    TryCastItemEnchantment_WithRedirects(spell, target);
                    break;

                case MagicSchool.LifeMagic:

                    HandleCastSpell(spell, target, null, caster);

                    if (spell.MetaSpellType != SpellType.LifeProjectile)
                    {
                        TryHandleFactionMob(target);

                        if (spell.IsHarmful)
                        {
                            // handle target procs
                            if (targetCreature != null && targetCreature != this)
                                TryProcEquippedItems(this, targetCreature, false, caster);
                        }
                    }
                    break;

                case MagicSchool.WarMagic:
                case MagicSchool.VoidMagic:

                    HandleCastSpell(spell, target, caster);
                    break;
            }
        }

        /// <summary>
        /// Perform the animations after casting a spell,
        /// ie. moving arms back in, returning to previous stance
        /// </summary>
        public void PostCastMotion()
        {
            var animSpeed = IsRanged ? PostCastSpeed_Ranged : PostCastSpeed;

            var motion = new Motion(this, MotionCommand.Ready, animSpeed);
            motion.MotionState.TurnSpeed = 2.25f;
            //motion.HasTarget = true;
            //motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            EnqueueBroadcastMotion(motion);
        }

        public float GetPostCastTime(Spell spell, bool fallback = false)
        {
            if (AiUseHumanMagicAnimations && !fallback)
                return GetPostCastTime_Human(spell);

            var animSpeed = IsRanged ? PostCastSpeed_Ranged : PostCastSpeed;

            return MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, MotionCommand.CastSpell, MotionCommand.Ready, animSpeed);
        }

        private float GetPostCastTime_Human(Spell spell)
        {
            var animSpeed = IsRanged ? PostCastSpeed_Ranged : PostCastSpeed;

            var animTime = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, spell.Formula.CastGesture, MotionCommand.Ready, animSpeed);

            // FIXME: data
            if (animTime == 0.0f)
                return GetPostCastTime(spell, true);

            return animTime;
        }

        /// <summary>
        /// Returns the magic skill level used for spell range checks.
        /// (initial points + points due to directly raising the skill)
        /// </summary>
        /// <returns></returns>
        private uint GetMagicSkillForRangeCheck()
        {
            var skill = GetCreatureSkill(CurrentSpell.School);

            // verify this - should it be using base?
            // seems like it could be off, player formula uses current + cap?

            return skill.InitLevel + skill.Ranks;
        }

        public void TryHandleFactionMob(WorldObject target)
        {
            if (target == this || target is Player)
                return;

            var creatureTarget = target as Creature;

            if (creatureTarget == null || !AllowFactionCombat(creatureTarget) && !PotentialFoe(creatureTarget))
                return;

            MonsterOnAttackMonster(creatureTarget);
        }
        /// <summary>
        /// Checks for AiUseHumanMagicAnimations and if true, sets CurrentSpell and sets combat mode to Magic
        /// </summary>
        public void CheckForHumanPreCast(Spell spell)
        {
            if (AiUseHumanMagicAnimations)
            {
                CurrentSpell = new Spell(spell.Id);
                SetCombatMode(CombatMode.Magic);
            }
        }
    }
}
