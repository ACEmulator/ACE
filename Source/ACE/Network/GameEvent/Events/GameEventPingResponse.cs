
namespace ACE.Network.GameEvent.Events
{
    public class GameEventPingResponse : GameEventPacket
    {
        public override GameEventOpcode Opcode { get { return GameEventOpcode.PingResponse; } }

        public GameEventPingResponse(Session session) : base(session) { }
    }
}

