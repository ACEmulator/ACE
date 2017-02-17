
using ACE.Network.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventSound : GameEventFraglessPacket
    {
        private Sound soundId;
        private float volume;

        public override GameEventOpcode Opcode { get { return GameEventOpcode.Sound; } }

        public GameEventSound(Session session, Sound soundId, float volume) 
            : base(session)
        {
            this.soundId = soundId;
            this.volume = volume;
        }

        protected override void WriteEventBody()
        {
            fragment.Payload.WriteGuid(session.Player.Guid);
            fragment.Payload.Write((uint)soundId);
            fragment.Payload.Write(volume);
        }
    }
}