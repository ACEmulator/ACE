using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageSound : GameMessage
    {
        public GameMessageSound(Entity.ObjectGuid guid, Sound soundId, float volume) 
            : base(GameMessageOpcode.Sound, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)soundId);
            Writer.Write(volume);
        }
    }
}
