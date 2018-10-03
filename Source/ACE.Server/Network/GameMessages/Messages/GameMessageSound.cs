using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageSound : GameMessage
    {
        public GameMessageSound(ObjectGuid guid, Sound soundId, float volume = 1.0f)
            : base(GameMessageOpcode.Sound, GameMessageGroup.SmartboxQueue)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)soundId);
            Writer.Write(volume);
        }
    }
}
