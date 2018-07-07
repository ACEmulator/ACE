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

        private ActionChain CreateMoveToChain(ObjectGuid target, float distance, out int thisMoveToChainNumber)
        {
            thisMoveToChainNumber = GetNextMoveToChainNumber();

            ActionChain moveToChain = new ActionChain();

            moveToChain.AddAction(this, () =>
            {
                var targetObject = CurrentLandblock?.GetObject(target);

                if (targetObject == null)
                {
                    // Is the item we're trying to move to in the container we have open?
                    var lastUsedContainer = CurrentLandblock?.GetObject(lastUsedContainerId) as Container;

                    if (lastUsedContainer != null)
                    {
                        if (lastUsedContainer.Inventory.ContainsKey(target))
                            targetObject = lastUsedContainer;
                        else
                        {
                            // could be a child container of this container
                            log.Error("Player_Use CreateMoveToChain container inception not finished");
                            return;
                        }
                    }
                }

                if (targetObject == null)
                {
                    log.Error("Player_Use CreateMoveToChain targetObject null");
                    return;
                }

                if (targetObject.Location == null)
                {
                    log.Error("Player_Use CreateMoveToChain targetObject.Location null");
                    return;
                }

                if (targetObject.WeenieType == WeenieType.Portal)
                    OnAutonomousMove(targetObject.Location, Sequences, MovementTypes.MoveToPosition, target);
                else
                    OnAutonomousMove(targetObject.Location, Sequences, MovementTypes.MoveToObject, target);
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
                bool ret = CurrentLandblock != null ? CurrentLandblock.WithinUseRadius(this, target, out valid) : false;

                // If one of the items isn't on a landblock
                if (!valid)
                    ret = false;

                return ret;
            }, moveToBody);

            return moveToChain;
        }

        private ActionChain CreateMoveToChain(WorldObject target, float distance, out int thisMoveToChainNumber)
        {
            thisMoveToChainNumber = GetNextMoveToChainNumber();

            ActionChain moveToChain = new ActionChain();

            moveToChain.AddAction(this, () =>
            {
                if (target.Location == null)
                {
                    log.Error("Player_Use CreateMoveToChain targetObject.Location null");
                    return;
                }

                if (target.WeenieType == WeenieType.Portal)
                    OnAutonomousMove(target.Location, Sequences, MovementTypes.MoveToPosition, target.Guid);
                else
                    OnAutonomousMove(target.Location, Sequences, MovementTypes.MoveToObject, target.Guid);
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
                bool ret = CurrentLandblock != null ? CurrentLandblock.WithinUseRadius(this, target.Guid, out valid) : false;

                // If one of the items isn't on a landblock
                if (!valid)
                    ret = false;

                return ret;
            }, moveToBody);

            return moveToChain;
        }


        public void SendUseDoneEvent(WeenieError errorType = WeenieError.None)
        {
            Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType));
        }


        // ===============================
        // Game Action Handlers - Use Item
        // ===============================
        // These are raised by client actions

        public void HandleActionUseWithTarget(ObjectGuid sourceObjectId, ObjectGuid targetObjectId)
        {
            StopExistingMoveToChains();

            new ActionChain(this, () =>
            {
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
                }
                else
                {
                    RecipeManager.UseObjectOnTarget(this, invSource, worldTarget);
                }
            }).EnqueueChain();
        }

        public void HandleActionUseItem(ObjectGuid usedItemId)
        {
            StopExistingMoveToChains();

            var actionChain = new ActionChain();

            actionChain.AddAction(this, () =>
            {
                // Search our inventory first
                var item = GetInventoryItem(usedItemId);

                if (item != null)
                    item.UseItem(this, actionChain);
                else
                {
                    // Search the world second
                    item = CurrentLandblock?.GetObject(usedItemId);

                    if (item == null)
                    {
                        Session.Network.EnqueueSend(new GameEventUseDone(Session)); // todo add an argument that indicates the item was not found
                        return;
                    }

                    if (item is Container)
                        lastUsedContainerId = usedItemId;

                    if (!IsWithinUseRadiusOf(item))
                    {
                        var moveToChain = CreateMoveToChain(item, item.UseRadiusSquared, out var thisMoveToChainNumber);

                        actionChain.AddChain(moveToChain);
                        actionChain.AddDelaySeconds(0.50);

                        // Make sure that after we've executed our MoveToChain, and waited our delay, we're still within use radius.
                        actionChain.AddAction(this, () =>
                        {
                            if (IsWithinUseRadiusOf(item) && thisMoveToChainNumber == moveToChainCounter)
                                actionChain.AddAction(item, () => item.ActOnUse(this));
                            else
                                // Action is cancelled
                                Session.Network.EnqueueSend(new GameEventUseDone(Session));
                        });
                    }
                    else
                        actionChain.AddAction(item, () => item.ActOnUse(this));
                }
            });

            actionChain.EnqueueChain();
        }
    }
}
