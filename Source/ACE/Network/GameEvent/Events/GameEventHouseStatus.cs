namespace ACE.Network.GameEvent
{
    public class GameEventHouseStatus : GameEventMessage
    {
        public override GameEventOpcode EventType { get { return GameEventOpcode.HouseStatus; } }

        public GameEventHouseStatus(Session session) : base(session) { }

        protected override void WriteEventBody()
        {
            // TODO
            writer.Write(2u);
        }
    }
}
