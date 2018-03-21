
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// This is set by HandleActionUseItem
        /// </summary>
        private ObjectGuid lastUsedContainerId;


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
                    iwo.DoActionUseItem(Session);
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

                            wo.ActOnUse(Guid);
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
