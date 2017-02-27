
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.PingRequest)]
    public class GameActionPingRequest : GameActionPacket
    {
        public GameActionPingRequest(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            Session.WorldSession.EnqueueSend(new GameEventPingResponse(Session));
        }
    }
}
