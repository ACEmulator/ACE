
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages;
using ACE.Network.Managers;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.IdentifyObject)]
    public class GameActionIdentifyObject : GameActionPacket
    {
        public GameActionIdentifyObject(Session session, ClientPacketFragment fragment) : base(session, fragment)
        {
        }

        private uint objectID;

        public override void Read()
        {
            objectID = Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            // TODO

            var identifyObjectResponse = new GameEventIdentifyObjectResponse(Session, objectID);
            Session.Network.EnqueueSend(identifyObjectResponse);
        }
    }
}
