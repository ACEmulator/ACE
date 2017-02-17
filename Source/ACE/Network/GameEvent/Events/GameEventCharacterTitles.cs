﻿
namespace ACE.Network.GameEvent.Events
{
    public class GameEventCharacterTitle : GameEventPacket
    {
        public override GameEventOpcode Opcode { get { return GameEventOpcode.CharacterTitle; } }

        public GameEventCharacterTitle(Session session) : base(session) { }

        protected override void WriteEventBody()
        {
            fragment.Payload.Write(1u);
            fragment.Payload.Write(1u); // TODO: get current title from database
            fragment.Payload.Write(10u); // TODO: get player's title list from database
            for (uint i = 1; i <= 10; i++)
                fragment.Payload.Write(i);
        }
    }
}
