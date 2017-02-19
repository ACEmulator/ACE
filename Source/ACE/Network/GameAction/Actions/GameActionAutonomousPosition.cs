
using ACE.Entity;

namespace ACE.Network.GameAction
{
    [GameAction(GameActionOpcode.AutonomousPosition)]
    public class GameActionAutonomousPosition : GameActionPacket
    {
        private Position position;
        private ushort instanceTimestamp;
        private ushort serverControlTimestamp;
        private ushort teleportTimestamp;
        private ushort forcePositionTimestamp;

        public GameActionAutonomousPosition(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            position               = new Position(Fragment.Payload);
            instanceTimestamp      = Fragment.Payload.ReadUInt16();
            serverControlTimestamp = Fragment.Payload.ReadUInt16();
            teleportTimestamp      = Fragment.Payload.ReadUInt16();
            forcePositionTimestamp = Fragment.Payload.ReadUInt16();
            Fragment.Payload.ReadByte();
        }

        public override void Handle()
        {
            Session.Player.UpdatePosition(position);
        }
    }
}
