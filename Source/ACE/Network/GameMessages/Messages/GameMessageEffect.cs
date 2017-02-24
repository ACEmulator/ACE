using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageEffect : GameMessage
    {
        public GameMessageEffect(Entity.ObjectGuid guid, uint effectId)
            : base(GameMessageOpcode.PlayEffect, 0xA)
        {
            Writer.WriteGuid(guid);
            Writer.Write(effectId);
            Writer.Write(1f); // scale ?
        }
    }
}