using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageSound : GameMessage
    {
        public GameMessageSound(Entity.ObjectGuid guid, Sound soundId, float volume) 
            : base(GameMessageOpcode.Sound, 0xA)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)soundId);
            Writer.Write(volume);
        }
    }
}