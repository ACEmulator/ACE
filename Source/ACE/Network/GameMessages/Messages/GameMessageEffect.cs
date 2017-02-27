using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageEffect : GameMessage
    {
        public GameMessageEffect(Entity.ObjectGuid guid, uint effectId)
            : base(GameMessageOpcode.PlayEffect, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(guid);
            Writer.Write(effectId);
            Writer.Write(1f); // scale ?
        }
    }
}