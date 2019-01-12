using System;
using System.Threading;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// This is set by HandleActionUseItem
        /// </summary>
        private ObjectGuid lastUsedContainerId;

        private TimeSpan defaultMoveToTimeout = TimeSpan.FromSeconds(15); // This is just a starting point number. It may be far off from retail.

        private int moveToChainCounter;
        private DateTime moveToChainStartTime;

        private int GetNextMoveToChainNumber()
        {
            return Interlocked.Increment(ref moveToChainCounter);
        }

        private void StopExistingMoveToChains()
        {
            Interlocked.Increment(ref moveToChainCounter);
        }

        // ===============================
        // Game Action Handlers - Use Item
        // ===============================
        // These are raised by client actions

        public void HandleActionUseWithTarget(uint sourceObjectGuid, uint targetObjectGuid)
        {
            StopExistingMoveToChains();

            var invSource = GetInventoryItem(sourceObjectGuid);
            var invTarget = GetInventoryItem(targetObjectGuid);
            var invWielded = GetEquippedItem(targetObjectGuid);

            if (invSource == null)
            {
                // is this caster with a built-in spell?
                var caster = GetEquippedItem(sourceObjectGuid);
                if (caster != null && caster.SpellDID != null)
                {
                    // check activation requirements
                    var skillFailed = CheckActivationRequirement(caster);
                    if (skillFailed != Skill.None)
                    {
                        Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, skillFailed.ToSentence()));
                        SendUseDoneEvent(WeenieError.SkillTooLow);
                    }
                    else
                        HandleActionCastTargetedSpell(targetObjectGuid, caster.SpellDID ?? 0);
                }
                else
                {
                    log.Warn($"{Name}.HandleActionUseWithTarget({sourceObjectGuid:X8}, {targetObjectGuid:X8}): couldn't find {sourceObjectGuid:X8}");
                    SendUseDoneEvent(WeenieError.None);
                }
                return;
            }

            var worldTarget = (invTarget == null) ? CurrentLandblock?.GetObject(targetObjectGuid) : null;

            if (invTarget != null)
            {
                // inventory on inventory, we can do this now
                if (invSource.WeenieType == WeenieType.ManaStone)
                {
                    var stone = invSource as ManaStone;
                    stone.HandleActionUseOnTarget(this, invTarget);
                }
                else
                    RecipeManager.UseObjectOnTarget(this, invSource, invTarget);
            }
            else if (invSource.WeenieType == WeenieType.Healer)
            {
                if (!(worldTarget is Player))
                {
                    SendUseDoneEvent(WeenieError.YouCantHealThat);
                    return;
                }

                var healer = invSource as Healer;
                healer.HandleActionUseOnTarget(this, worldTarget as Player);
            }
            else if (invSource.WeenieType == WeenieType.Key)
            {
                var key = invSource as Key;
                key.HandleActionUseOnTarget(this, worldTarget);
            }
            else if (invSource.WeenieType == WeenieType.Lockpick && worldTarget is Lock)
            {
                var lp = invSource as Lockpick;
                lp.HandleActionUseOnTarget(this, worldTarget);
            }
            else if (targetObjectGuid == Guid.Full)
            {
                // using something on ourselves
                if (invSource.WeenieType == WeenieType.ManaStone)
                {
                    var stone = invSource as ManaStone;
                    stone.HandleActionUseOnTarget(this, this);
                }
                else
                    RecipeManager.UseObjectOnTarget(this, invSource, this);
            }
            else if (invWielded != null)
            {
                if (invSource.WeenieType == WeenieType.ManaStone)
                {
                    var stone = invSource as ManaStone;
                    stone.HandleActionUseOnTarget(this, invWielded);
                }
                else
                    RecipeManager.UseObjectOnTarget(this, invSource, invWielded);
            }
            else
            {
                RecipeManager.UseObjectOnTarget(this, invSource, worldTarget);
            }
        }

        private Skill CheckActivationRequirement(WorldObject item)
        {
            if (item.ItemDifficulty != null)
            {
                if (GetCreatureSkill(Skill.ArcaneLore).Current < item.ItemDifficulty.Value)
                    return Skill.ArcaneLore;
            }

            if (item.ItemSkillLimit != null && item.ItemSkillLevelLimit != null)
            {
                if (GetCreatureSkill((Skill)item.ItemSkillLimit.Value).Current < item.ItemSkillLevelLimit.Value)
                    return (Skill)item.ItemSkillLimit.Value;
            }
            return Skill.None;
        }

        public void HandleActionUseItem(uint itemGuid)
        {
            StopExistingMoveToChains();

            // Search our inventory first
            var item = GetInventoryItem(itemGuid);

            if (item != null)
                item.UseItem(this);
            else
            {
                // Search the world second
                item = CurrentLandblock?.GetObject(itemGuid);

                if (item == null)
                {
                    Session.Network.EnqueueSend(new GameEventUseDone(Session)); // todo add an argument that indicates the item was not found
                    return;
                }

                var moveTo = true;
                if (item is Container container)
                {
                    lastUsedContainerId = new ObjectGuid(itemGuid);

                    // if the container is already open by this player,
                    // this packet indicates to close the container.
                    if (container.IsOpen && container.Viewer == Guid.Full)
                    {
                        // closing the container does not require moving towards it
                        moveTo = false;
                    }
                }

                // already there?
                if (!moveTo || IsWithinUseRadiusOf(item))
                {
                    item.ActOnUse(this);
                    return;
                }

                // if required, move to
                var actionChain = CreateMoveToChain(item, out var thisMoveToChainNumber);

                actionChain.AddAction(item, () =>
                {
                    if (thisMoveToChainNumber == moveToChainCounter)
                    {
                        item.ActOnUse(this);
                    }
                    else
                    {
                        // Action is cancelled
                        // this needs to happen much earlier,
                        // when a player event happens that actually cancels an existing moveto chain
                        // as it currently stands, the player will get a perpetual hourglass
                        // if they legitimately cancel a moveto event in progress...

                        Session.Network.EnqueueSend(new GameEventUseDone(Session));
                    }
                });
                actionChain.EnqueueChain();
            }
        }

        public ActionChain CreateMoveToChain(WorldObject target, out int thisMoveToChainNumber)
        {
            thisMoveToChainNumber = GetNextMoveToChainNumber();

            ActionChain moveToChain = new ActionChain();

            moveToChain.AddAction(this, () =>
            {
                if (target.Location == null)
                {
                    log.Error($"{Name}.CreateMoveToChain({target.Name}): target.Location is null");
                    return;
                }

                if (target.WeenieType == WeenieType.Portal)
                    MoveToPosition(target.Location);
                else
                    MoveToObject(target);
            });

            // poll for arrival every .1 seconds
            // Ideally, this should be switched away from using the DelayManager, and instead be checked on every Player Tick()
            ActionChain moveToBody = new ActionChain();
            moveToBody.AddDelaySeconds(.1);

            var thisMoveToChainNumberCopy = thisMoveToChainNumber;

            moveToChainStartTime = DateTime.UtcNow;

            moveToChain.AddLoop(this, () =>
            {
                if (thisMoveToChainNumberCopy != moveToChainCounter)
                    return false;

                // Break loop if CurrentLandblock == null (we portaled or logged out)
                if (CurrentLandblock == null)
                {
                    StopExistingMoveToChains(); // This increments our moveToChainCounter and thus, should stop any additional actions in this chain
                    return false;
                }

                // Have we timed out?
                if (moveToChainStartTime + defaultMoveToTimeout <= DateTime.UtcNow)
                {
                    StopExistingMoveToChains(); // This increments our moveToChainCounter and thus, should stop any additional actions in this chain
                    return false;
                }

                // Are we within use radius?
                bool ret = !CurrentLandblock.WithinUseRadius(this, target.Guid, out var valid);

                // If one of the items isn't on a landblock
                if (!valid)
                {
                    ret = false;
                    StopExistingMoveToChains(); // This increments our moveToChainCounter and thus, should stop any additional actions in this chain
                }

                return ret;
            }, moveToBody);

            return moveToChain;
        }

        /// <summary>
        /// Returns the amount of time until this item's cooldown expires
        /// </summary>
        public TimeSpan GetCooldown(WorldObject item)
        {
            if (!LastUseTracker.TryGetValue(item.CooldownId.Value, out var lastUseTime))
                return TimeSpan.FromSeconds(0);

            var nextUseTime = lastUseTime + TimeSpan.FromSeconds(item.CooldownDuration.Value);

            if (DateTime.UtcNow < nextUseTime)
                return nextUseTime - DateTime.UtcNow;
            else
                return TimeSpan.FromSeconds(0);
        }

        /// <summary>
        /// Returns TRUE if this item can be activated at this time
        /// </summary>
        public bool CheckCooldown(WorldObject item)
        {
            return GetCooldown(item).TotalSeconds == 0.0f;
        }

        /// <summary>
        /// Maintains the cooldown timers for an item
        /// </summary>
        public void UpdateCooldown(WorldObject item)
        {
            if (!LastUseTracker.ContainsKey(item.CooldownId.Value))
                LastUseTracker.Add(item.CooldownId.Value, DateTime.UtcNow);
            else
                LastUseTracker[item.CooldownId.Value] = DateTime.UtcNow;
        }

        /// <summary>
        /// Verifies the use requirements for activating an item
        /// </summary>
        public GameEventWeenieErrorWithString CheckUseRequirements(WorldObject item)
        {
            // verify arcane lore requirement
            if (item.ItemDifficulty != null)
            {
                // TODO: more specific error messages
                var arcaneLore = GetCreatureSkill(Skill.ArcaneLore);
                if (arcaneLore.Current < item.ItemDifficulty.Value)
                    return new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, arcaneLore.Skill.ToSentence());
            }

            // verify skill - does this have to be trained, or only in conjunction with UseRequiresSkillLevel?
            // only seems to be used for summoning so far...
            if (item.UseRequiresSkill != null)
            {
                var skill = (Skill)item.UseRequiresSkill.Value;
                var playerSkill = GetCreatureSkill(skill);

                if (playerSkill.AdvancementClass < SkillAdvancementClass.Trained)
                    return new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.Your_SkillMustBeTrained, playerSkill.Skill.ToSentence());

                // verify skill level
                if (item.UseRequiresSkillLevel != null)
                {
                    if (playerSkill.Current < item.UseRequiresSkillLevel.Value)
                        return new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, playerSkill.Skill.ToSentence());
                }
            }

            // verify skill specialized
            // is this always in conjunction with UseRequiresSkill?
            // again, only seems to be for summoning so far...
            if (item.UseRequiresSkillSpec != null)
            {
                var skill = (Skill)item.UseRequiresSkillSpec.Value;
                var playerSkill = GetCreatureSkill(skill);

                if (playerSkill.AdvancementClass < SkillAdvancementClass.Specialized)
                    return new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouMustSpecialize_ToUseItemMagic, playerSkill.Skill.ToSentence());

                // verify skill level
                if (item.UseRequiresSkillLevel != null)
                {
                    if (playerSkill.Current < item.UseRequiresSkillLevel.Value)
                        return new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.Your_IsTooLowToUseItemMagic, playerSkill.Skill.ToSentence());
                }
            }

            // verify player level
            if (item.UseRequiresLevel != null)
            {
                var playerLevel = Level ?? 1;
                if (playerLevel < item.UseRequiresLevel.Value)
                    return new GameEventWeenieErrorWithString(Session, WeenieErrorWithString.YouMustBe_ToUseItemMagic, $"level {item.UseRequiresLevel.Value}");
            }

            return null;
        }

        public void SendUseDoneEvent(WeenieError errorType = WeenieError.None)
        {
            Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType));
        }
    }
}
