
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
            var objectID = message.Payload.ReadUInt32();
            // TODO

            var identifyObjectResponse = new GameEventIdentifyObjectResponse(session, objectID);
            session.Network.EnqueueSend(identifyObjectResponse);
        }
    }
}
