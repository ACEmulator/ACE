using ACE.Entity;
using ACE.Managers;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionPutItemInContainer : QueuedGameAction
    {
        public QueuedGameActionPutItemInContainer(uint objectId, uint secondaryObjectId, GameActionType actionType, LandblockId landBlockId)
        {
            ObjectId = objectId;
            SecondaryObjectId = secondaryObjectId;
            ActionType = actionType;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = this.StartTime;
            LandBlockId = landBlockId;
        }

        protected override void Handle(Player player)
        {
            var playerGuid = new ObjectGuid(ObjectId);
            var inventoryGuid = new ObjectGuid(SecondaryObjectId);

                var inventoryItem = LandblockManager.GetWorldObject(player.Session, inventoryGuid);

                float arrivedRadiusSquared = 0.00f;
                bool validGuids;
                if (WithinUseRadius(playerGuid, inventoryGuid, out arrivedRadiusSquared, out validGuids))
                    player.NotifyAndAddToInventory(inventoryItem);
                else
                {
                    if (validGuids)
                    {
                        player.SetDestinationInformation(inventoryItem.PhysicsData.Position, arrivedRadiusSquared);
                        player.BlockedGameAction = this;
                        player.OnAutonomousMove(inventoryItem.PhysicsData.Position, player.Sequences, MovementTypes.MoveToObject, inventoryGuid);
                    }
                }
        }

        /// <summary>
        /// Check to see if we are close enough to interact.   Adds a fudge factor of 1.5f
        /// </summary>
        /// <param name="playerGuid"></param>
        /// <param name="targetGuid"></param>
        /// <param name="arrivedRadiusSquared"></param>
        /// <param name="arrivedRadiusSquared"></param>
        /// <returns></returns>
        public bool WithinUseRadius(ObjectGuid playerGuid, ObjectGuid targetGuid, out float arrivedRadiusSquared, out bool validGuids)
        {
            var playerPosition = LandblockManager.GetWorldObjectPositionByLandblockID(playerGuid, LandBlockId);
            var targetPosition = LandblockManager.GetWorldObjectPositionByLandblockID(targetGuid, LandBlockId);

            if (playerPosition != null && targetPosition != null)
            {
                validGuids = true;
                arrivedRadiusSquared = LandblockManager.GetWorldObjectEffectiveUseRadiusByLandblockID(targetGuid, LandBlockId);
                return (playerPosition.SquaredDistanceTo(targetPosition) <= arrivedRadiusSquared);
            }
            arrivedRadiusSquared = 0.00f;
            validGuids = false;
            return false;
        }
    }
}
