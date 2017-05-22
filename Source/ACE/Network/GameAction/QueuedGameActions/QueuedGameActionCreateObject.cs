using ACE.Entity;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionCreateObject : QueuedGameAction
    {
        public QueuedGameActionCreateObject(uint objectId, WorldObject worldObject, bool respectDelay, LandblockId landBlockID)
        {
            ObjectId = objectId;
            WorldObject = worldObject;
            RespectDelay = respectDelay;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = this.StartTime;
            LandBlockId = landBlockID;
        }

        protected override void Handle(Player player)
        {
            OpenWorldManager.OpenWorld.Register(WorldObject);
        }
    }
}
