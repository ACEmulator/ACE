
namespace ACE.Network.GameEvent.Events
{
    public class GameEventPingResponse : GameEventMessage
    {
        public GameEventPingResponse(Session session) : base(GameEventType.PingResponse, 0x9, session) { }
    }
}

