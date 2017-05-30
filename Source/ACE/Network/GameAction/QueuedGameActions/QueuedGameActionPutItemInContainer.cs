using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.InGameManager;
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
        public QueuedGameActionPutItemInContainer(uint objectId, uint secondaryObjectId, LandblockId landBlockId)
        {
            ObjectId = objectId;
            SecondaryObjectId = secondaryObjectId;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = this.StartTime;
            LandBlockId = landBlockId;
        }

        protected override void Handle(GameMediator mediator, Player player)
        {
            var playerGuid = new ObjectGuid(ObjectId);
            var inventoryGuid = new ObjectGuid(SecondaryObjectId);

            // var inventoryItem = LandManager.OpenWorld.ReadOnlyClone(inventoryGuid);

                float arrivedRadiusSquared = 0.00f;
                bool validGuids;
            // if (WithinUseRadius(playerGuid, inventoryGuid, out arrivedRadiusSquared, out validGuids))
            // player.NotifyAndAddToInventory(inventoryItem);
            if (true == true) // junk line.
                validGuids = true; // junk line
            else
            {
                if (validGuids)
                {
                    // player.SetDestinationInformation(inventoryItem.PhysicsData.Position, arrivedRadiusSquared);
                    player.BlockedGameAction = this;
                    // player.OnAutonomousMove(inventoryItem.PhysicsData.Position, player.Sequences, MovementTypes.MoveToObject, inventoryGuid);
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
            var playerPosition = InGameManager.InGameManager.ReadOnlyClone(playerGuid).Location;
            // var targetPosition = LandManager.OpenWorld.ReadOnlyClone(targetGuid).Location;

            if (playerPosition != null) // && targetPosition != null)
            {
                validGuids = true;

                // var wo = LandManager.OpenWorld.ReadOnlyClone(targetGuid);
                WorldObject wo = null; // junk
                if (wo != null)
                {
                    var csetup = SetupModel.ReadFromDat(wo.PhysicsData.CSetup);
                    arrivedRadiusSquared = (float)Math.Pow((wo.GameData.UseRadius + csetup.Radius + 1.5), 2);
                }
                else
                    arrivedRadiusSquared = 0.00f;

                // return (playerPosition.SquaredDistanceTo(targetPosition) <= arrivedRadiusSquared);
                return false;
            }
            arrivedRadiusSquared = 0.00f;
            validGuids = false;
            return false;
        }
    }
}
