
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


        private ActionChain CreateMoveToChain(ObjectGuid target, float distance)
        {
            ActionChain moveToChain = new ActionChain();
            // While !at(thing) moveToThing
            ActionChain moveToBody = new ActionChain();

            moveToChain.AddAction(this, () =>
            {
                var targetObject = CurrentLandblock.GetObject(target);

                if (targetObject == null)
                {
                    // Is the item we're trying to move to in the container we have open?
                    var lastUsedContainer = CurrentLandblock.GetObject(lastUsedContainerId) as Container;

                    if (lastUsedContainer != null)
                    {
                        if (lastUsedContainer.Inventory.ContainsKey(target))
                            targetObject = lastUsedContainer;
                        else
                        {
                            // could be a child container of this container
                            log.Error("Player CreateMoveToChain container inception not finished");
                            return;
                        }
                    }
                }

                if (targetObject == null)
                {
                    log.Error("Player CreateMoveToChain targetObject null");
                    return;
                }

                if (targetObject.Location == null)
                {
                    log.Error("Player CreateMoveToChain targetObject.Location null");
                    return;
                }

                if (targetObject.WeenieType == WeenieType.Portal)
                    OnAutonomousMove(targetObject.Location, Sequences, MovementTypes.MoveToPosition, target);
                else
                    OnAutonomousMove(targetObject.Location, Sequences, MovementTypes.MoveToObject, target);
            });

            // poll for arrival every .1 seconds
            moveToBody.AddDelaySeconds(.1);

            moveToChain.AddLoop(this, () =>
            {
                float outdistance;

                // Break loop if CurrentLandblock == null (we portaled or logged out), or if we arrive at the item
                if (CurrentLandblock == null)
                    return false;

                bool ret = !CurrentLandblock.WithinUseRadius(Guid, target, out outdistance, out var valid);

                // If one of the items isn't on a landblock
                if (!valid)
                    ret = false;

                return ret;
            }, moveToBody);

            return moveToChain;
        }

        private void DoMoveTo(WorldObject wo)
        {
            ActionChain moveToObjectChain = new ActionChain();

            moveToObjectChain.AddChain(CreateMoveToChain(wo.Guid, 0.2f));
            moveToObjectChain.AddDelaySeconds(0.50);

            moveToObjectChain.AddAction(wo, () => wo.ActOnUse(this));

            moveToObjectChain.EnqueueChain();
        }

        public void SendUseDoneEvent()
        {
            Session.Network.EnqueueSend(new GameEventUseDone(Session));
        }

 
        // ===============================
        // Game Action Handlers - Use Item
        // ===============================
        // These are raised by client actions

        public void HandleActionUseItem(ObjectGuid usedItemId)
        {
            new ActionChain(this, () =>
            {
                var iwo = GetInventoryItem(usedItemId);

                // todo, I think for this, DoActionUseItem should be renamed BuildUseItemActionChain
                // Then, we can add the GameEventUseDone and queue the action.
                // Overrides of the DoActionUseItem fn shouldn't have to be concerned with the GameEventUseDone message.
                if (iwo != null)
                    iwo.DoActionUseItem(this);
                else
                {
                    if (CurrentLandblock != null)
                    {
                        // Just forward our action to the appropriate user...
                        var wo = CurrentLandblock.GetObject(usedItemId);

                        if (wo != null)
                        {
                            if (wo is Container)
                                lastUsedContainerId = usedItemId;

                            if (!IsWithinUseRadiusOf(wo))
                                DoMoveTo(wo);
                            else
                                wo.ActOnUse(this);
                        }
                    }
                }
            }).EnqueueChain();
        }

        public void HandleActionUseWithTarget(ObjectGuid sourceObjectId, ObjectGuid targetObjectId)
        {
            new ActionChain(this, () =>
            {
                var invSource = GetInventoryItem(sourceObjectId);
                var invTarget = GetInventoryItem(targetObjectId);

                if (invTarget != null)
                {
                    // inventory on inventory, we can do this now
                    RecipeManager.UseObjectOnTarget(this, invSource, invTarget);
                }
                else if (invSource.WeenieType == WeenieType.Key)
                {
                    var theTarget = CurrentLandblock.GetObject(targetObjectId);
                    var key = invSource as Key;
                    key.HandleActionUseOnTarget(this, theTarget);
                }
                else if (targetObjectId == Guid)
                {
                    // using something on ourselves
                    RecipeManager.UseObjectOnTarget(this, invSource, this);
                }
                else
                {
                    var theTarget = CurrentLandblock.GetObject(targetObjectId);
                    RecipeManager.UseObjectOnTarget(this, invSource, theTarget);
                }
            }).EnqueueChain();
        }
    }
}
