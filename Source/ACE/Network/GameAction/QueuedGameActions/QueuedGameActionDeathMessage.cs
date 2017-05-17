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
    public class QueuedGameActionDeathMessage : QueuedGameAction
    {
        public QueuedGameActionDeathMessage(string broadcastMessage, uint objectId, uint secondaryObjectId, GameActionType actionType, LandblockId landBlockId)
        {
            ObjectId = objectId;
            SecondaryObjectId = secondaryObjectId;
            ActionBroadcastMessage = broadcastMessage;
            ActionType = actionType;
            LandBlockId = landBlockId;
        }

        protected override void Handle(Player player)
        {
            DeathMessageArgs d = new DeathMessageArgs(ActionBroadcastMessage, new ObjectGuid(ObjectId), new ObjectGuid(SecondaryObjectId));
            BroadcastEventArgs args = BroadcastEventArgs.CreateDeathMessage(player, d);
            LandblockManager.BroadcastByLandblockID(args, false, Quadrant.All, LandBlockId);
        }
    }
}
