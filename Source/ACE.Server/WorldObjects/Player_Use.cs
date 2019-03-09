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
        /// This is set by HandleActionUseItem / TryUseItem
        /// </summary>
        public ObjectGuid LastUsedContainerId { get; set; }

        /// <summary>
        /// Handles the 'GameAction 0x35 - UseWithTarget' network message
        /// when player double clicks an inventory item resulting in a target indicator
        /// and then clicks another item
        /// </summary>
        public void HandleActionUseWithTarget(uint sourceObjectGuid, uint targetObjectGuid)
        {
            StopExistingMoveToChains();

            // source item is always in our possession
            var sourceItem = FindObject(sourceObjectGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems, out _, out _, out var sourceItemIsEquipped);

            if (sourceItem == null)
            {
                log.Warn($"{Name}.HandleActionUseWithTarget({sourceObjectGuid:X8}, {targetObjectGuid:X8}): couldn't find {sourceObjectGuid:X8}");
                SendUseDoneEvent();
                return;
            }

            if (sourceItemIsEquipped)
            {
                // This could be a caster with a built-in spell
                if (sourceItem.SpellDID != null)
                {
                    // check activation requirements
                    var result = sourceItem.CheckUseRequirements(this);
                    if (!result.Success)
                    {
                        if (result.Message != null)
                            Session.Network.EnqueueSend(result.Message);

                        SendUseDoneEvent();
                    }
                    else
                        HandleActionCastTargetedSpell(targetObjectGuid, sourceItem.SpellDID ?? 0);
                }
                else
                {
                    SendUseDoneEvent();
                }

                return;
            }


            // Is the target the player?
            if (targetObjectGuid == Guid.Full)
            {
                // using something on ourselves
                if (sourceItem.WeenieType == WeenieType.ManaStone)
                    ((ManaStone)sourceItem).HandleActionUseOnTarget(this, this);
                else
                    RecipeManager.UseObjectOnTarget(this, sourceItem, this);

                return;
            }


            // Is the target item in our possession?
            var targetItem = FindObject(targetObjectGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems);

            if (targetItem != null)
            {
                if (sourceItem.WeenieType == WeenieType.ManaStone)
                    ((ManaStone)sourceItem).HandleActionUseOnTarget(this, targetItem);
                else
                    RecipeManager.UseObjectOnTarget(this, sourceItem, targetItem);

                return;
            }


            // Is the target on the landblock?
            targetItem = FindObject(targetObjectGuid, SearchLocations.Landblock);

            if (targetItem == null)
            {
                log.Warn($"{Name}.HandleActionUseWithTarget({sourceObjectGuid:X8}, {targetObjectGuid:X8}): couldn't find {targetObjectGuid:X8}");
                SendUseDoneEvent();
                return;
            }

            if (sourceItem.WeenieType == WeenieType.Healer)
            {
                if (targetItem is Player player)
                    ((Healer)sourceItem).HandleActionUseOnTarget(this, player);
                else
                    SendUseDoneEvent(WeenieError.YouCantHealThat);
            }
            else if (sourceItem.WeenieType == WeenieType.Key)
            {
                ((Key)sourceItem).HandleActionUseOnTarget(this, targetItem);
            }
            else if (sourceItem.WeenieType == WeenieType.Lockpick)
            {
                ((Lockpick)sourceItem).HandleActionUseOnTarget(this, targetItem);
            }
            else
            {
                RecipeManager.UseObjectOnTarget(this, sourceItem, targetItem);
            }
        }

        /// <summary>
        /// Handles the 'GameAction 0x36 - UseItem' network message
        /// when player double clicks an item in the 3d world
        /// </summary>
        public void HandleActionUseItem(uint itemGuid)
        {
            StopExistingMoveToChains();

            var item = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems | SearchLocations.Landblock);

            if (item != null)
            {
                if (item.CurrentLandblock != null && !item.Visibility && item.Guid != LastUsedContainerId)
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
                    LastUsedContainerId = item.Guid;

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
