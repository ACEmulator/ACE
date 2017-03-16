
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.AllegianceUpdateRequest)]
    public class GameActionAllegianceUpdateRequest : GameActionPacket
    {
        public GameActionAllegianceUpdateRequest(Session session, ClientPacketFragment fragment) : base(session, fragment)
        {
        }

        private uint unknown1;

        public override void Read()
        {
            unknown1 = Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            // TODO

            var allegianceUpdate = new GameEventAllegianceUpdate(Session);

            Session.Network.EnqueueSend(allegianceUpdate);
        }
    }
}
