using ACE.Network.GameEvent;

namespace ACE.Network.GameAction
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
