using ACE.Network.Enum;

namespace ACE.Network
{
    public class GameMessageSound : GameMessage
    {
        private Entity.ObjectGuid guid;
        private Sound soundId;
        private float volume;

        public GameMessageSound(Session target, Entity.ObjectGuid guid, Sound soundId, float volume) : base(target, GameMessageOpcode.Sound)
        {
            this.guid = guid;
            this.soundId = soundId;
            this.volume = volume;
        }

        protected override void WriteBody()
        {
            writer.WriteGuid(guid);
            writer.Write((uint)soundId);
            writer.Write(volume);
        }
    }
}