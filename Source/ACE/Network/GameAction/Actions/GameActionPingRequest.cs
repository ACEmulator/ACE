
using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.PingRequest)]
    public class GameActionPingRequest : GameActionPacket
    {
        public GameActionPingRequest(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            new GameEventPingResponse(session).Send();
        }
    }
}
