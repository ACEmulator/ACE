
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageEffect : GameMessageOnChannel
    {
        public GameMessageEffect(Entity.ObjectGuid guid, uint effectId) : base(GameMessageOpcode.PlayEffect,GameMessageChannel.Channel0)
        {
            Writer.WriteGuid(guid);
            Writer.Write(effectId);
            Writer.Write(1f); // scale ?
        }
    }
}