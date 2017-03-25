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
            // TODO: Extend to creatures as well, as we momentarily only have players

            if (objectId.IsPlayer())
            {
                var tmpLandblock = this.Session.Player.Position.LandblockId;
                var pl = (Player)LandblockManager.GetObjectByGuid(tmpLandblock, objectId);

                if (pl != null)
                {
                    float healthPercentage = (float)pl.Health.Current / (float)pl.Health.MaxValue;

                    var updateHealth = new GameEventUpdateHealth(Session, this.objectId.Full, healthPercentage);
                    Session.Network.EnqueueSend(updateHealth);
                }
            }

        }
    }
}
