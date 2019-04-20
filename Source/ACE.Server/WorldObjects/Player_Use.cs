using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// This is set by HandleActionUseItem / TryUseItem
        /// </summary>
        public ObjectGuid LastOpenedContainerId { get; set; }

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

            // handle casters with built-in spells
            if (sourceItemIsEquipped)
            {
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
                        HandleActionCastTargetedSpell(targetObjectGuid, sourceItem.SpellDID ?? 0, true);
                }
                else
                {
                    SendUseDoneEvent();
                }

                return;
            }

            // Resolve the guid to an object that is either in our possession or on the Landblock
            var target = FindObject(targetObjectGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems | SearchLocations.Landblock);

            if (target == null)
            {
                log.Warn($"{Name}.HandleActionUseWithTarget({sourceObjectGuid:X8}, {targetObjectGuid:X8}): couldn't find {targetObjectGuid:X8}");
                SendUseDoneEvent();
                return;
            }

            sourceItem.HandleActionUseOnTarget(this, target);
        }

        /// <summary>
        /// Handles the 'GameAction 0x36 - UseItem' network message
        /// when player double clicks an item
        /// </summary>
        public void HandleActionUseItem(uint itemGuid)
        {
            StopExistingMoveToChains();

            var item = FindObject(itemGuid, SearchLocations.MyInventory | SearchLocations.MyEquippedItems | SearchLocations.Landblock);

            if (item != null)
            {
                if (item.CurrentLandblock != null && !item.Visibility && item.Guid != LastOpenedContainerId)
                    CreateMoveToChain(item, (success) => TryUseItem(item, success));
                else
                    TryUseItem(item);
            }
            else
            {
                log.Debug($"{Name}.HandleActionUseItem({itemGuid:X8}): couldn't find object");
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
                item.OnActivate(this);

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


        /// <summary>
        /// This method processes the Game Action (F7B1) No Longer Viewing Contents (0x0195)
        /// This is raised when we:
        /// - have a container open and open up a second container without closing the first container.
        /// </summary>
        public void HandleActionNoLongerViewingContents(uint objectGuid)
        {
            var container = CurrentLandblock?.GetObject(objectGuid) as Container;

            if (container != null && container.Viewer == Guid.Full)
                container.Close(this);
        }

        public CombatPet CurrentActiveCombatPet { get; set; }

        public void StartBarber()
        {
            BarberActive = true;
            Session.Network.EnqueueSend(new GameEventStartBarber(Session));
        }
    }
}
