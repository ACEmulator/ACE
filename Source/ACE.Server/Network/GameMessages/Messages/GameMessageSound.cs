using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageSound : GameMessage
    {
        public GameMessageSound(global::ACE.Entity.ObjectGuid guid, Sound soundId, float volume)
            : base(GameMessageOpcode.Sound, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)soundId);
            Writer.Write(volume);
        }
    }
}
