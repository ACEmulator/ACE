namespace ACE.Network.GameEvent
{
    public class GameEventCharacterTitle : GameEventMessage
    {
        public override GameEventOpcode EventType { get { return GameEventOpcode.CharacterTitle; } }

        public GameEventCharacterTitle(Session session) : base(session) { }

        protected override void WriteEventBody()
        {
            // TODO
            writer.Write(1u);
            writer.Write(1u);
            writer.Write(0u);
        }
    }
}
