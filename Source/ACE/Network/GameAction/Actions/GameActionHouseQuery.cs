
using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.HouseQuery)]
    public class GameActionHouseQuery : GameActionPacket
    {
        public GameActionHouseQuery(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            new GameEventHouseStatus(session).Send();
        }
    }
}
