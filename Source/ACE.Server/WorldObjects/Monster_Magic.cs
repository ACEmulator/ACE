using System;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
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
        public BiotaPropertiesSpellBook CurrentSpell;

        /// <summary>
        /// The delay after casting a magic spell
        /// </summary>
        public static readonly float MagicDelay = 4.0f;

        /// <summary>
        /// Returns the monster's current magic skill
        /// for the school containing the current spell
        /// </summary>
        public uint GetMagicSkill()
        {
            var currentSpell = GetCurrentSpell();
            return GetCreatureSkill((MagicSchool)currentSpell.School).Current;
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

            var rng = Physics.Common.Random.RollDice(0.0f, 100.0f);
            //var rng = Physics.Common.Random.RollDice(0.0f, probability);
            return rng < probability;
        }

        /// <summary>
        /// Perform the monster spell casting animation
        /// </summary>
        public void DoCastMotion(WorldObject target, out float animLength)
        {
            var castMotion = new MotionItem(MotionCommand.CastSpell, 1.5f);
            animLength = MotionTable.GetAnimationLength(MotionTableId, CurrentMotionState.Stance, castMotion);

            var motion = new UniversalMotion(CurrentMotionState.Stance, castMotion);
            motion.MovementData.CurrentStyle = (uint)CurrentMotionState.Stance;
            motion.MovementData.TurnSpeed = 2.25f;
            //motion.HasTarget = true;
            //motion.TargetGuid = target.Guid;
            CurrentMotionState = motion;

            if (CurrentLandblock != null)
                CurrentLandblock.EnqueueBroadcastMotion(this, motion);
        }

        /// <summary>
        /// Performs the monster windup spell animation,
        /// casts the spell, and returns to attack stance
        /// </summary>
        public void MagicAttack()
        {
            NextAttackTime = Timer.CurrentTime + MagicDelay;

            var spellBase = GetCurrentSpellBase();
            var spell = GetCurrentSpell();

            //Console.WriteLine(spell.Name);

            var actionChain = new ActionChain();

            DoCastMotion(AttackTarget, out var animLength);
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(this, () => CastSpell());
            actionChain.AddAction(this, () => DoAttackStance());

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Casts the current monster spell on target
        /// </summary>
        public void CastSpell()
        {
            var spellBase = GetCurrentSpellBase();
            var spell = GetCurrentSpell();

            var targetSelf = spellBase.Name.Contains("Self");   // better way?
            var target = targetSelf ? this : AttackTarget;

            var player = AttackTarget as Player;
            var scale = SpellAttributes(player.Session.Account, spell.SpellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            switch (spellBase.School)
            {
                case MagicSchool.WarMagic:

                    WarMagic(AttackTarget, spellBase, spell);
                    break;

                case MagicSchool.LifeMagic:

                    if (!targetSelf && ResistSpell(Skill.LifeMagic))break;
                    LifeMagic(target, spellBase, spell, out uint damage, out bool critical, out var msg);
                    if (CurrentLandblock != null) CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(target.Guid, (PlayScript)spellBase.TargetEffect, scale));
                    break;

                case MagicSchool.CreatureEnchantment:

                    if (!targetSelf && ResistSpell(Skill.CreatureEnchantment)) break;
                    CreatureMagic(target, spellBase, spell);
                    if (CurrentLandblock != null) CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(target.Guid, (PlayScript)spellBase.TargetEffect, scale));
                    break;
            }
        }

        /// <summary>
        /// Returns TRUE if player resists the magic spell
        /// </summary>
        public bool ResistSpell(Skill skill)
        {
            var player = AttackTarget as Player;

            var magicSkill = GetCreatureSkill(skill).Current;
            var difficulty = player.GetCreatureSkill(Skill.MagicDefense).Current;

            var resisted = MagicDefenseCheck(magicSkill, difficulty);

            if (resisted)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat("You resist the spell cast by " + Name, ChatMessageType.Magic));
                player.Session.Network.EnqueueSend(new GameMessageSound(player.Guid, Sound.ResistSpell, 1.0f));
            }
            return resisted;
        }

        /// <summary>
        /// Selects a random spell from the monster's spell book
        /// according to the probabilities
        /// </summary>
        public BiotaPropertiesSpellBook GetRandomSpell()
        {
            var probability = GetSpellProbability();
            var rng = Physics.Common.Random.RollDice(0.0f, probability);

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
            var spell = GetCurrentSpellBase();
            var skill = GetMagicSkill();

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
            return DatabaseManager.World.GetCachedSpell((uint)CurrentSpell.Spell);
        }

        /// <summary>
        /// Returns the current SpellBase for the monster
        /// </summary>
        public SpellBase GetCurrentSpellBase()
        {
            return DatManager.PortalDat.SpellTable.Spells[(uint)CurrentSpell.Spell];
        }
    }
}
