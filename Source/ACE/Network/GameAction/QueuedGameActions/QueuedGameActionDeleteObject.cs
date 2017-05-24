using ACE.Entity;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionDeleteObject : QueuedGameAction
    {
        public QueuedGameActionDeleteObject(uint objectId, WorldObject worldObject, bool respectDelay, bool onlyRemove, LandblockId landBlockID)
        {
            ObjectId = objectId;
            WorldObject = worldObject;
            RespectDelay = respectDelay;
            OnlyRemove = onlyRemove;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = this.StartTime;
            LandBlockId = landBlockID;
        }

        protected override void Handle(Player player)
        {
            LandManager.OpenWorld.UnRegister(WorldObject);
        }
    }
}
