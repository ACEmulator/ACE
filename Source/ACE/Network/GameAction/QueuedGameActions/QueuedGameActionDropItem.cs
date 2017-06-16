using ACE.Entity;
using ACE.InGameManager;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionDropItem : QueuedGameAction
    {
        public QueuedGameActionDropItem(uint objectId, uint secondaryObjectId, LandblockId landBlockID)
        {
            ObjectId = objectId;
            SecondaryObjectId = secondaryObjectId;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = this.StartTime;
            LandBlockId = landBlockID;
        }

        protected override void Handle(GameMediator mediator, Player player)
        {
            player.NotifyAndDropItem(new ObjectGuid(SecondaryObjectId));
        }
    }
}
