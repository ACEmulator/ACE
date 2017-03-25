using ACE.Network.GameEvent.Events;
using System;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.QueryHealth)]
    public class GameActionQueryHealth : GameActionPacket
    {
        private uint objectid;

        public GameActionQueryHealth(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            objectid = Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            // ONLY A TEST
            // Real solution: Check if objectid is a player or monster and get the health info from that
            // compute percentage from max health and current health (this.Session.Player.Health.Current), if a player factor in vitae

            var updateHealth = new GameEventUpdateHealth(Session, objectid, 1.0f);

            Session.Network.EnqueueSend(updateHealth);
        }
    }
}
