
using ACE.Network.Enum;

namespace ACE.Network.Messages
{
    public class GameMessageSound : GameMessage
    {
        private Entity.ObjectGuid guid;
        private Sound soundId;
        private float volume;

        public GameMessageSound(Entity.ObjectGuid guid, Sound soundId, float volume) 
            : base(GameMessageOpcode.Sound)
        {
            this.guid = guid;
            this.soundId = soundId;
            this.volume = volume;
            writer.WriteGuid(guid);
            writer.Write((uint)soundId);
            writer.Write(volume);
        }
    }
}