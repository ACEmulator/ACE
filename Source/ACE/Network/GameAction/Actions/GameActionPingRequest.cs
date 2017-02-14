using ACE.Network.GameEvent;

namespace ACE.Network.GameAction
{
    [GameAction(GameActionOpcode.PingRequest)]
    public class GameActionPingRequest : GameActionMessage
    {
        public GameActionPingRequest(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            new GameEventPingResponse(session).Send();
        }
    }
}
