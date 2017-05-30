using ACE.Entity;
using ACE.Entity.Events;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network.Motion;
using ACE.InGameManager;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionMovementEvent : QueuedGameAction
    {
        public QueuedGameActionMovementEvent(uint objectId, UniversalMotion motion, GameActionType actionType, LandblockId landBlockId)
        {
            ObjectId = objectId;
            Motion = motion;
            ActionType = actionType;
            LandBlockId = landBlockId;
        }

        public QueuedGameActionMovementEvent(uint objectId, Position newQueuedPosition, GameActionType actionType, LandblockId landBlockId)
        {
            ObjectId = objectId;
            ActionLocation = newQueuedPosition;
            ActionType = actionType;
            LandBlockId = landBlockId;
        }

        // not implemented yet
        public QueuedGameActionMovementEvent(uint objectId, UniversalMotion motion, double duration, bool respectDelay, GameActionType actionType, LandblockId landBlockId)
        {
            ObjectId = objectId;
            Motion = motion;
            RespectDelay = respectDelay;
            ActionType = actionType;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime + duration;
            LandBlockId = landBlockId;
        }

        protected override void Handle(GameMediator mediator, Player player)
        {
            // WorldObject wo = LandManager.OpenWorld.ReadOnlyClone(new ObjectGuid(ObjectId));
            // BroadcastEventArgs args = BroadcastEventArgs.CreateMovementEvent(wo,  Motion);
            // LandManager.OpenWorld.Broadcast(args, Quadrant.All);
        }
    }
}
