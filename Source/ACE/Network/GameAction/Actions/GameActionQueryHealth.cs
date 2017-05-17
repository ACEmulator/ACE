using ACE.Entity;
using ACE.Managers;
using ACE.Network.GameAction.QueuedGameActions;
using ACE.Network.GameEvent.Events;
using System;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionQueryHealth
    {
        [GameAction(GameActionType.QueryHealth)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint fullId = message.Payload.ReadUInt32();

            QueuedGameAction action = new QueuedGameActionQueryHealth(fullId, GameActionType.QueryHealth);
            session.Player.AddToActionQueue(action);
        }
    }
}
