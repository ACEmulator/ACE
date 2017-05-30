using ACE.Entity;
using ACE.InGameManager;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionIdentifyObject : QueuedGameAction
    {
        public QueuedGameActionIdentifyObject(uint objectId)
        {
            ObjectId = objectId;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime;
        }

        protected override void Handle(GameMediator mediator, Player player)
        {
            // TODO: Throttle this request. The live servers did this, likely for a very good reason, so we should, too.
            var identifyResponse = new GameEventIdentifyObjectResponse(player.Session, ObjectId, player);
            player.Session.Network.EnqueueSend(identifyResponse);
        }
    }
}
