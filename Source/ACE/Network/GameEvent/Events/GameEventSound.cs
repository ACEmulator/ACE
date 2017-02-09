using ACE.Network.Enum;

namespace ACE.Network.GameEvent
{
    public class GameEventSound : GameEventPacket
    {
        private Sound soundId;
        private uint volume;

        public override GameEventOpcode Opcode { get { return GameEventOpcode.Sound; } }

        public GameEventSound(Session session, Sound soundId, uint volume) 
            : base(session)
        {
            this.soundId = soundId;
            this.volume = volume;
        }

        protected override void WriteEventBody()
        {
            fragment.Payload.Write(session.Player.Guid.Full);
            fragment.Payload.Write((uint)soundId);
            fragment.Payload.Write(volume);
        }
    }
}