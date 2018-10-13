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

        private int moveToChainCounter;

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

        public void HandleActionUseWithTarget(ObjectGuid sourceObjectId, ObjectGuid targetObjectId)
        {
            StopExistingMoveToChains();

            var invSource = GetInventoryItem(sourceObjectId);
            var invTarget = GetInventoryItem(targetObjectId);
            var invWielded = GetWieldedItem(targetObjectId);

            var worldTarget = (invTarget == null) ? CurrentLandblock?.GetObject(targetObjectId) : null;

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
                    return;

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
            else if (targetObjectId == Guid)
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

        public void HandleActionUseItem(ObjectGuid usedItemId)
        {
            StopExistingMoveToChains();

            // Search our inventory first
            var item = GetInventoryItem(usedItemId);

            if (item != null)
                item.UseItem(this);
            else
            {
                // Search the world second
                item = CurrentLandblock?.GetObject(usedItemId);

                if (item == null)
                {
                    Session.Network.EnqueueSend(new GameEventUseDone(Session)); // todo add an argument that indicates the item was not found
                    return;
                }

                var moveTo = true;
                var container = item as Container;
                if (container != null)
                {
                    lastUsedContainerId = usedItemId;

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
                var moveToChain = CreateMoveToChain(item, out var thisMoveToChainNumber);

                var actionChain = new ActionChain();
                actionChain.AddChain(moveToChain);
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

        // TODO: refactor movetochainnumber, this is a confusing and patchwork concept
        // TODO: add proper hooks for canceling an existing moveto event
        // TODO: add reasonable max move time
        private ActionChain CreateMoveToChain(WorldObject target, out int thisMoveToChainNumber)
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
                    OnAutonomousMove(target.Location, Sequences, MovementTypes.MoveToPosition, target.Guid, (target.UseRadius ?? 0.6f));
                else
                    OnAutonomousMove(target.Location, Sequences, MovementTypes.MoveToObject, target.Guid, (target.UseRadius ?? 0.6f));
            });

            // poll for arrival every .1 seconds
            ActionChain moveToBody = new ActionChain();
            moveToBody.AddDelaySeconds(.1);

            var thisMoveToChainNumberCopy = thisMoveToChainNumber;

            moveToChain.AddLoop(this, () =>
            {
                if (thisMoveToChainNumberCopy != moveToChainCounter)
                    return false;

                // Break loop if CurrentLandblock == null (we portaled or logged out)
                if (CurrentLandblock == null)
                    return false;

                // Are we within use radius?
                var valid = false;
                bool ret = CurrentLandblock != null ? !CurrentLandblock.WithinUseRadius(this, target.Guid, out valid) : false;

                // If one of the items isn't on a landblock
                if (!valid)
                    ret = false;

                return ret;
            }, moveToBody);

            return moveToChain;
        }

        // TODO: deprecate this
        // it is not the responsibility of Player_Use to convert ObjectGuids into WorldObjects
        // this should be done much earlier, at the beginning of the HandleAction methods
        private ActionChain CreateMoveToChain(ObjectGuid targetGuid, out int thisMoveToChainNumber)
        {
            var targetObject = FindItemLocation(targetGuid);
            if (targetObject == null)
            {
                thisMoveToChainNumber = moveToChainCounter;
                return null;
            }
            return CreateMoveToChain(targetObject, out thisMoveToChainNumber);
        }

        /// <summary>
        /// Finds the location of an item in the world
        /// If item is within a container or corpse, returns the location of the parent
        /// </summary>
        private WorldObject FindItemLocation(ObjectGuid targetGuid)
        {
            var targetObject = CurrentLandblock?.GetObject(targetGuid);

            if (targetObject == null)
            {
                // Is the item we're trying to move to in the container we have open?
                var lastUsedContainer = CurrentLandblock?.GetObject(lastUsedContainerId) as Container;

                if (lastUsedContainer != null)
                {
                    if (lastUsedContainer.Inventory.ContainsKey(targetGuid))
                        targetObject = lastUsedContainer;
                }
            }
            if (targetObject == null)
                log.Error($"{Name}.FindItemLocation({targetGuid}): couldn't find item location on landblock"); ;

            return targetObject;
        }

        public void SendUseDoneEvent(WeenieError errorType = WeenieError.None)
        {
            Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType));
        }
    }
}
