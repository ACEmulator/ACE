namespace ACE.Network.GameEvent
{
    public class GameEventCharacterTitle : GameEventPacket
    {
        public override GameEventOpcode Opcode { get { return GameEventOpcode.CharacterTitle; } }

        public GameEventCharacterTitle(Session session) : base(session) { }

        protected override void WriteEventBody()
        {
			fragment.Payload.Write(1u);
			fragment.Payload.Write(1u); // TODO: get current title from database
			fragment.Payload.Write(1000u); // TODO: get player's title list from database
			for (uint i = 1; i <= 1000; i++)
				fragment.Payload.Write(i);
		}
    }
}
