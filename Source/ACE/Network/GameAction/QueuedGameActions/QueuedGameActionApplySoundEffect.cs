using ACE.Entity;
using ACE.Entity.Events;
using ACE.Managers;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionApplySoundEffect : QueuedGameAction
    {
        public QueuedGameActionApplySoundEffect(uint objectId, uint secondaryObjectId, GameActionType actionType, LandblockId landBlockId)
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
            var soundEffect = (Sound)SecondaryObjectId;
            WorldObject wo = LandblockManager.GetWorldObject(player.Session, new ObjectGuid(ObjectId));

            BroadcastEventArgs args = BroadcastEventArgs.CreateSoundAction(wo, soundEffect);
            LandblockManager.BroadcastByLandblockID(args, true, Quadrant.All, LandBlockId);
        }
    }
}