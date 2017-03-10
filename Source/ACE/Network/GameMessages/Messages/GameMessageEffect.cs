using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageEffect : GameMessage
    {
        public GameMessageEffect(Entity.ObjectGuid guid, Effect effectId, float scale = 1f)
            : base(GameMessageOpcode.PlayEffect, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)effectId);
            Writer.Write(scale); // scale ?
        }
    }
}