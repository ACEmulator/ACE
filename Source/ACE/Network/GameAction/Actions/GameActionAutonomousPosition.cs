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
            position               = new Position(fragment.Payload);
            instanceTimestamp      = fragment.Payload.ReadUInt16();
            serverControlTimestamp = fragment.Payload.ReadUInt16();
            teleportTimestamp      = fragment.Payload.ReadUInt16();
            forcePositionTimestamp = fragment.Payload.ReadUInt16();
            fragment.Payload.ReadByte();
        }

        public override void Handle()
        {
            session.Character.UpdatePosition(position);
        }
    }
}
