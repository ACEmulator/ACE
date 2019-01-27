using System;
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
        /// Handles the 'GameAction 0x35 - UseWithTarget' network message
        /// when player double clicks an inventory item resulting in a target indicator
        /// and then clicks another item
        /// </summary>
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
                    var result = caster.CheckUseRequirements(this);
                    if (!result.Success)
                    {
                        if (result.Message != null)
                            Session.Network.EnqueueSend(result.Message);

                        SendUseDoneEvent();
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

        /// <summary>
        /// This is set by HandleActionUseItem / TryUseItem
        /// </summary>
        public ObjectGuid lastUsedContainerId { get; set; }

        /// <summary>
        /// Handles the 'GameAction 0x36 - UseItem' network message
        /// when player double clicks an item in the 3d world
        /// </summary>
        public void HandleActionUseItem(uint itemGuid)
        {
            StopExistingMoveToChains();

            var item = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems | SearchLocations.Landblock, out Container foundInContainer, out Container rootOwner, out bool wasEquipped);

            if (item != null)
            {
                if (item.CurrentLandblock != null && !item.Visibility && item.Guid != lastUsedContainerId)
                    CreateMoveToChain(item, (success) => TryUseItem(item, success));
                else
                    TryUseItem(item);
            }
            else
            {
                log.Warn($"{Name}.HandleActionUseItem({itemGuid:X8}): couldn't find object");
                SendUseDoneEvent();
            }
        }

        public float LastUseTime;

        /// <summary>
        /// Attempts to use an item - checks activation requirements
        /// </summary>
        public void TryUseItem(WorldObject item, bool success = true)
        {
            //Console.WriteLine($"{Name}.TryUseItem({item.Name}, {success})");
            LastUseTime = 0.0f;

            if (success)
            {
                if (item is Container)
                    lastUsedContainerId = item.Guid;

                item.OnActivate(this);
            }

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(LastUseTime);
            actionChain.AddAction(this, () => SendUseDoneEvent());
            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Sends the GameEventUseDone network message for a player
        /// </summary>
        /// <param name="errorType">An optional error message</param>
        public void SendUseDoneEvent(WeenieError errorType = WeenieError.None)
        {
            Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType));
        }
    }
}
