using ACE.Entity;
using ACE.Network;
using ACE.Network.GameAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public partial class Player
    {
        [GameAction(GameActionType.DropItem)]
        private void DropItemAction(ClientMessage message)
        {
            var objectGuid = new ObjectGuid(message.Payload.ReadUInt32());
            QueuedGameAction action = new QueuedGameAction(Guid.Full, objectGuid.Full, GameActionType.DropItem);
            AddToActionQueue(action);
        }

        [GameAction(GameActionType.IdentifyObject)]
        private void IdentifyObjectAction(ClientMessage message)
        {
            var id = message.Payload.ReadUInt32();
            QueuedGameAction action = new QueuedGameAction(id, GameActionType.IdentifyObject);
            AddToExaminationQueue(action);
        }

        [GameAction(GameActionType.PutItemInContainer)]
        private void PutItemInContainerAction(ClientMessage message)
        {
            var itemGuid = new ObjectGuid(message.Payload.ReadUInt32());
            var containerGuid = new ObjectGuid(message.Payload.ReadUInt32());
            QueuedGameAction action = new QueuedGameAction(containerGuid.Full, itemGuid.Full, GameActionType.PutItemInContainer);
            AddToActionQueue(action);
        }

        [GameAction(GameActionType.Use)]
        private void UseItemAction(ClientMessage message)
        {
            uint fullId = message.Payload.ReadUInt32();
            QueuedGameAction action = new QueuedGameAction(fullId, GameActionType.EvtInventoryUseEvent);
            AddToActionQueue(action);
            return;
        }

        [GameAction(GameActionType.QueryHealth)]
        private void QueryHealthAction(ClientMessage message)
        {
            uint fullId = message.Payload.ReadUInt32();
            QueuedGameAction action = new QueuedGameAction(fullId, GameActionType.QueryHealth);
            AddToActionQueue(action);
        }
    }
}
