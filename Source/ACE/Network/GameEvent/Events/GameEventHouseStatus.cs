namespace ACE.Network.GameEvent
{
    public class GameEventHouseStatus : GameEventPacket
    {
        public override GameEventOpcode Opcode { get { return GameEventOpcode.HouseStatus; } }

        public GameEventHouseStatus(Session session) : base(session) { }

        protected override void WriteEventBody()
        {
            // TODO
            fragment.Payload.Write(2u);
        }
    }
}
