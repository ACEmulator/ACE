namespace ACE.Network.GameEvent
{
    public class GameEventCharacterTitle : GameEventPacket
    {
        public override GameEventOpcode Opcode { get { return GameEventOpcode.CharacterTitle; } }

        public GameEventCharacterTitle(Session session) : base(session) { }

        protected override void WriteEventBody()
        {
            // TODO
            fragment.Payload.Write(1u);
            fragment.Payload.Write(1u);
            fragment.Payload.Write(0u);
        }
    }
}
