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
        public QueuedGameActionApplySoundEffect(uint objectId, uint secondaryObjectIds, LandblockId landBlockId)
        {
            ObjectId = objectId;
            SecondaryObjectId = secondaryObjectIds;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = this.StartTime;
            LandBlockId = landBlockId;
        }

        protected override void Handle(Player player)
        {
            var soundEffect = (Sound)SecondaryObjectId;
            // WorldObject wo = LandManager.OpenWorld.ReadOnlyClone(new ObjectGuid(ObjectId));

            BroadcastEventArgs args = BroadcastEventArgs.CreateSoundAction(this.WorldObject, soundEffect);
            // LandManager.OpenWorld.Broadcast(args, Quadrant.All);
        }
    }
}