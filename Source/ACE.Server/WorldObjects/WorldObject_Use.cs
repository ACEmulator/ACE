using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public double? UseTimestamp
        {
            get => GetProperty(PropertyFloat.UseTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.UseTimestamp); else SetProperty(PropertyFloat.UseTimestamp, value.Value); }
        }

        protected double? ResetTimestamp
        {
            get => GetProperty(PropertyFloat.ResetTimestamp);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResetTimestamp); else SetProperty(PropertyFloat.ResetTimestamp, value.Value); }
        }

        protected double? ResetInterval
        {
            get => GetProperty(PropertyFloat.ResetInterval);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResetInterval); else SetProperty(PropertyFloat.ResetInterval, value.Value); }
        }

        protected bool DefaultLocked
        {
            get => GetProperty(PropertyBool.DefaultLocked) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.DefaultLocked); else SetProperty(PropertyBool.DefaultLocked, value); }
        }

        protected bool DefaultOpen
        {
            get => GetProperty(PropertyBool.DefaultOpen) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.DefaultOpen); else SetProperty(PropertyBool.DefaultOpen, value); }
        }

        /// <summary>
        /// Used to determine how close you need to be to use an item.
        /// </summary>
        public bool IsWithinUseRadiusOf(WorldObject wo, float? useRadius = null)
        {
            if (useRadius == null)
                useRadius = wo.UseRadius ?? 0.6f;

            var cylDist = GetCylinderDistance(wo);

            return cylDist <= useRadius;
        }

        public float GetCylinderDistance(WorldObject wo)
        {
            return (float)Physics.Common.Position.CylinderDistance(PhysicsObj.GetRadius(), PhysicsObj.GetHeight(), PhysicsObj.Position,
                wo.PhysicsObj.GetRadius(), wo.PhysicsObj.GetHeight(), wo.PhysicsObj.Position);
        }

        /// <summary>
        /// Handles the 'GameAction 0x35 - UseWithTarget' network message
        /// on a per-object type basis.
        public virtual void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            RecipeManager.UseObjectOnTarget(player, this, target);
        }

        public virtual void OnActivate(WorldObject activator)
        {
            //Console.WriteLine($"{Name}.OnActivate({activator.Name})");

            // when players double click an object,
            // and the packet comes in as GameAction 0x36 - UseItem
            // from the game perspective, technically this starts as an 'Activate',
            // which can have a list of possible ActivationResponses - 
            // Use (by far the most common), Animate, Talk, Emote, CastSpell, Generate

            // PropertyInt.Active indicates if this object can be activated, default is true
            if (!Active) return;

            // verify use requirements
            var result = CheckUseRequirements(activator);

            var player = activator as Player;
            if (!result.Success)
            {
                if (result.Message != null && player != null)
                    player.Session.Network.EnqueueSend(result.Message);

                return;
            }

            if (player != null)
                player.EnchantmentManager.StartCooldown(this);

            // perform motion animation - rarely used (only 4 instances in PY16 db)
            if (ActivationResponse.HasFlag(ActivationResponse.Animate))
                OnAnimate(activator);

            // perform activation emote
            if (ActivationResponse.HasFlag(ActivationResponse.Emote))
                OnEmote(activator);

            // cast a spell on the player (spell traps)
            if (ActivationResponse.HasFlag(ActivationResponse.CastSpell))
                OnCastSpell(activator);

            // call to generator to spawn new object
            if (ActivationResponse.HasFlag(ActivationResponse.Generate))
                OnGenerate(activator);

            // default use action
            if (ActivationResponse.HasFlag(ActivationResponse.Use))
            {
                if (activator is Creature creature)
                {
                    //target.EmoteManager.OnActivation(creature); // found a few things with Activation on them but not ActivationResponse.Emote...
                    EmoteManager.OnUse(creature);
                }

                ActOnUse(activator);
            }

            // send chat text - rarely used (only 8 instances in PY16 db)
            if (ActivationResponse.HasFlag(ActivationResponse.Talk))
                OnTalk(activator);

            if (!(this is Creature) && ActivationTarget > 0)
            {
                var activationTarget = CurrentLandblock?.GetObject(new ObjectGuid(ActivationTarget));
                if (activationTarget != null)
                    activationTarget.OnActivate(activator);
                else
                {
                    log.Warn($"{Name}.OnActivate({activator.Name}): couldn't find activation target {ActivationTarget:X8}");
                }
            }
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
                TryCastSpell(spell, activator, this);
            }
        }

        public virtual void OnGenerate(WorldObject activator)
        {
            if (IsGenerator)
                Generator_Generate();
        }

        /// <summary>
        /// Verifies the use requirements for activating an item
        /// </summary>
        public virtual ActivationResult CheckUseRequirements(WorldObject activator)
        {
            //Console.WriteLine($"{Name}.CheckUseRequirements({activator.Name})");

            if (activator == null)
            {
                log.Error($"0x{Guid}:{Name}.CheckUseRequirements() (wcid: {WeenieClassId}): activator is null");
                return new ActivationResult(false);
            }

            if (!(activator is Player player))
                return new ActivationResult(true);

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
                {
                    //return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_SkillMustBeTrained, playerSkill.Skill.ToSentence()));
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"You must have {playerSkill.Skill.ToSentence()} trained to use that item's magic"));
                    return new ActivationResult(false);
                }

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
                    //return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.YouMustBe_ToUseItemMagic, $"level {UseRequiresLevel.Value}")); // not retail
                    return new ActivationResult(new GameEventCommunicationTransientString(player.Session, "You are not high enough level to use that!"));
            }

            // verify attribute / vital limits
            if (ItemAttributeLimit != null)
            {
                var playerAttr = player.Attributes[ItemAttributeLimit.Value];

                if (playerAttr.Current < ItemAttributeLevelLimit)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, playerAttr.Attribute.ToString()));
            }

            if (ItemAttribute2ndLimit != null)
            {
                var playerVital = player.Vitals[ItemAttribute2ndLimit.Value];

                if (playerVital.MaxValue < ItemAttribute2ndLevelLimit)
                    return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, playerVital.Vital.ToSentence()));
            }

            // Check for a cooldown
            if (!player.EnchantmentManager.CheckCooldown(CooldownId))
            {
                // TODO: werror/string not found, find exact message

                /*var cooldown = player.GetCooldown(this);
                var timer = cooldown.GetFriendlyString();
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{Name} can be activated again in {timer}", ChatMessageType.Broadcast));*/

                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You have used this item too recently"));
                return new ActivationResult(false);
            }

            if (player.IsOlthoiPlayer)
            {
                //player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "Olthoi can't interact with that!"));
                //player.SendWeenieError(WeenieError.OlthoiCannotInteractWithThat);
                //return new ActivationResult(false);

                if (this is Creature)
                {
                    if (CreatureType == ACE.Entity.Enum.CreatureType.Olthoi)
                        return new ActivationResult(true);
                    else
                    {
                        if (this is Vendor)
                            player.SendWeenieError(WeenieError.OlthoiVendorLooksInHorror);
                        else if (NpcLooksLikeObject ?? false)
                            player.SendWeenieError(WeenieError.OlthoiCannotInteractWithThat);
                        else
                            player.SendWeenieErrorWithString(WeenieErrorWithString._CowersFromYou, Name);

                        return new ActivationResult(false);
                    }
                }
                else if (this is Lifestone)
                {
                    player.SendWeenieError(WeenieError.OlthoiCannotUseLifestones);
                    return new ActivationResult(false);
                }
                else if (this is Container && !(this is Corpse))
                {
                    player.SendWeenieError(WeenieError.OlthoiCannotInteractWithThat);
                    return new ActivationResult(false);
                }
                else if (this is AttributeTransferDevice || this is AugmentationDevice || this is Bindstone || this is Book
                    || this is Game || this is Gem || this is GenericObject || this is Key || this is SkillAlterationDevice)
                {
                    player.SendWeenieError(WeenieError.OlthoiCannotInteractWithThat);
                    return new ActivationResult(false);
                }
            }

            return new ActivationResult(true);
        }
    }
}
