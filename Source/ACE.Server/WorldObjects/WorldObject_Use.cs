using System;
using ACE.Common;
using ACE.Server.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        private double? useTimestamp;
        protected double? UseTimestamp
        {
            get { return useTimestamp; }
            set => useTimestamp = Time.GetUnixTime();
        }

        protected double? ResetInterval
        {
            get => GetProperty(PropertyFloat.ResetInterval);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResetInterval); else SetProperty(PropertyFloat.ResetInterval, value.Value); }
        }

        protected bool DefaultLocked { get; set; }

        protected bool DefaultOpen { get; set; }

        /// <summary>
        /// Used to determine how close you need to be to use an item.
        /// </summary>
        public bool IsWithinUseRadiusOf(WorldObject wo)
        {
            var useRadius = wo.UseRadius ?? 0.6f;
            var cylDist = GetCylinderDistance(wo);

            return cylDist <= useRadius;
        }

        public float GetCylinderDistance(WorldObject wo)
        {
            return (float)Physics.Common.Position.CylinderDistance(PhysicsObj.GetRadius(), PhysicsObj.GetHeight(), PhysicsObj.Position,
                wo.PhysicsObj.GetRadius(), wo.PhysicsObj.GetHeight(), wo.PhysicsObj.Position);
        }

        public virtual void OnActivate(WorldObject activator)
        {
            //Console.WriteLine($"{Name}.OnActivate({activator.Name})");

            // when players double click an object in the 3d world,
            // and the packet comes in as GameAction 0x35 - UseWithTarget
            // from the game perspective, technically this starts as an 'Activate',
            // which can have a list of possible ActivationResponses - 
            // Use (by far the most common), Animate, Talk, Emote, CastSpell, Generate

            // PropertyInt.Active indicates if this object can be activated, default is true
            if (!Active) return;

            // verify use requirements
            var result = CheckUseRequirements(activator);

            if (!result.Success)
            {
                if (result.Message != null && activator is Player player)
                    player.Session.Network.EnqueueSend(result.Message);

                return;
            }

            // find activation target
            var target = ActivationTarget != 0 ? CurrentLandblock?.GetObject(new ObjectGuid(ActivationTarget)) : this;

            // special case for creatures - redirect through emote chain?
            if (this is Creature) target = this;

            if (target == null)
            {
                log.Warn($"{Name}.OnActivate({activator.Name}): couldn't find activation target {ActivationTarget:X8}");
                return;
            }

            // if ActivationTarget is another object,
            // should this be checking the ActivationResponse of the target object?

            // default use action
            if (ActivationResponse.HasFlag(ActivationResponse.Use))
                target.ActOnUse(activator);

            // perform motion animation - rarely used (only 4 instances in PY16 db)
            if (ActivationResponse.HasFlag(ActivationResponse.Animate))
                target.OnAnimate(activator);

            // send chat text - rarely used (only 8 instances in PY16 db)
            if (ActivationResponse.HasFlag(ActivationResponse.Talk))
                target.OnTalk(activator);

            // perform activation emote
            if (ActivationResponse.HasFlag(ActivationResponse.Emote))
                target.OnEmote(activator);

            // cast a spell on the player (spell traps)
            if (ActivationResponse.HasFlag(ActivationResponse.CastSpell))
                target.OnCastSpell(activator);

            // call to generator to spawn new object
            if (ActivationResponse.HasFlag(ActivationResponse.Generate))
                target.OnGenerate(activator);
        }

        public virtual void ActOnUse(WorldObject activator)
        {
            // empty base - individual WorldObject types should override

            var msg = $"{Name}.ActOnUse({activator.Name}) - undefined for wcid {WeenieClassId} type {WeenieType}";
            log.Error(msg);

            if (activator is Player _player)
                _player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
        }

        public virtual void OnAnimate(WorldObject activator)
        {
            var motion = new Motion(this, ActivationAnimation);
            EnqueueBroadcastMotion(motion);
        }

        public virtual void OnTalk(WorldObject activator)
        {
            // todo: verify the format of this message
            if (activator is Player player)
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(ActivationTalk, ChatMessageType.Broadcast));
        }

        public virtual void OnEmote(WorldObject activator)
        {
            if (activator is Creature creature)
                EmoteManager.OnActivation(creature);
        }

        public virtual void OnCastSpell(WorldObject activator)
        {
            if (SpellDID != null)
            {
                var spell = new Spell(SpellDID.Value);
                TryCastSpell(spell, activator);
            }
        }

        public virtual void OnGenerate(WorldObject activator)
        {
            if (IsGenerator)
                Generator_HeartBeat();
        }

        /// <summary>
        /// Verifies the use requirements for activating an item
        /// </summary>
        public virtual ActivationResult CheckUseRequirements(WorldObject activator)
        {
            //Console.WriteLine($"{Name}.CheckUseRequirements({activator.Name})");

            if (!(activator is Player))
                return new ActivationResult(false);

            var player = activator as Player;

            // verify arcane lore requirement
            if (ItemDifficulty != null)
            {
                var arcaneLore = player.GetCreatureSkill(Skill.ArcaneLore);
                if (arcaneLore.Current < ItemDifficulty.Value)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, arcaneLore.Skill.ToSentence()));
            }

            // verify skill - does this have to be trained, or only in conjunction with UseRequiresSkillLevel?
            // only seems to be used for summoning so far...
            if (ItemSkillLimit != null && ItemSkillLevelLimit != null)
            {
                var skill = activator.ConvertToMoASkill((Skill)ItemSkillLimit.Value);
                var playerSkill = player.GetCreatureSkill(skill);

                if (playerSkill.Current < ItemSkillLevelLimit.Value)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, playerSkill.Skill.ToSentence()));
            }

            if (UseRequiresSkill != null)
            {
                var skill = activator.ConvertToMoASkill((Skill)UseRequiresSkill.Value);
                var playerSkill = player.GetCreatureSkill(skill);

                if (playerSkill.AdvancementClass < SkillAdvancementClass.Trained)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_SkillMustBeTrained, playerSkill.Skill.ToSentence()));

                // verify skill level
                if (UseRequiresSkillLevel != null)
                {
                    if (playerSkill.Current < UseRequiresSkillLevel.Value)
                        return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, playerSkill.Skill.ToSentence()));
                }
            }

            // verify skill specialized
            // is this always in conjunction with UseRequiresSkill?
            // again, only seems to be for summoning so far...
            if (UseRequiresSkillSpec != null)
            {
                var skill = activator.ConvertToMoASkill((Skill)UseRequiresSkillSpec.Value);
                var playerSkill = player.GetCreatureSkill(skill);

                if (playerSkill.AdvancementClass < SkillAdvancementClass.Specialized)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouMustSpecialize_ToUseItemMagic, playerSkill.Skill.ToSentence()));

                // verify skill level
                if (UseRequiresSkillLevel != null)
                {
                    if (playerSkill.Current < UseRequiresSkillLevel.Value)
                        return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, playerSkill.Skill.ToSentence()));
                }
            }

            // verify player level
            if (UseRequiresLevel != null)
            {
                var playerLevel = player.Level ?? 1;
                if (playerLevel < UseRequiresLevel.Value)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouMustBe_ToUseItemMagic, $"level {UseRequiresLevel.Value}"));
            }

            return new ActivationResult(true);
        }
    }
}
