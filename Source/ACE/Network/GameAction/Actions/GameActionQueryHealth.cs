using ACE.Entity;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using System;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.QueryHealth)]
    public class GameActionQueryHealth : GameActionPacket
    {
        private ObjectGuid objectId;

        public GameActionQueryHealth(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            uint fullId = Fragment.Payload.ReadUInt32();
            this.objectId = new ObjectGuid(fullId);
        }

        public override void Handle()
        {
            LandblockManager.HandleQueryHealth(this.Session, objectId);
        }
    }
}
