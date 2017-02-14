namespace ACE.Network.GameEvent
{
    public class GameEventPingResponse : GameEventMessage
    {
        public override GameEventOpcode EventType { get { return GameEventOpcode.PingResponse; } }

        public GameEventPingResponse(Session session) : base(session) { }
    }
}

