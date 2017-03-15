
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.HouseQuery)]
    public class GameActionHouseQuery : GameActionPacket
    {
        public GameActionHouseQuery(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Handle()
        {
            Session.Network.EnqueueSend(new GameEventHouseStatus(Session));
        }
    }
}
