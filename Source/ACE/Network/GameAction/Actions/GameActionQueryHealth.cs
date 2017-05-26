using ACE.Entity;
using ACE.Entity.PlayerActions;
using ACE.Managers;
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

            /*
            QueuedGameAction action = new QueuedGameAction(fullId, GameActionType.QueryHealth);
            session.Player.AddToActionQueue(action);
            */
            session.Player.AddNonBlockingAction(new DelegateAction(() => session.Player.ActQueryHealth(new ObjectGuid(fullId))));
        }
    }
}
