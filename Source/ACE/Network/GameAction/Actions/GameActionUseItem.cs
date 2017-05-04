using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionUseItem
    {
        [GameAction(GameActionType.EvtInventoryUseEvent)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint fullId = message.Payload.ReadUInt32();
            QueuedGameAction action = new QueuedGameAction(fullId, GameActionType.EvtInventoryUseEvent);
            session.Player.AddToActionQueue(action);
            return;
        }
    }
}
