using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionPingRequest
    {
        [GameAction(GameActionType.PingRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameEventPingResponse(session));
        }
    }
}