using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionIdentifyObject
    {
        [GameAction(GameActionType.IdentifyObject)]
        public static void Handle(ClientMessage message, Session session)
        {
            var id = message.Payload.ReadUInt32();
            QueuedGameAction action = new QueuedGameAction(id, GameActionType.IdentifyObject);
            session.Player.AddToExaminationQueue(action);
        }
    }
}