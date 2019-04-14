using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionPingRequest
    {
        [GameAction(GameActionType.PingRequest)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.EnqueueSend(new GameEventPingResponse(session));
        }
    }
}