using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageScript : GameMessage
    {
        public GameMessageScript(Entity.ObjectGuid guid, PlayScript scriptId, float scale = 1f)
            : base(GameMessageOpcode.PlayEffect, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)scriptId);
            Writer.Write(scale); // scale ?
        }
    }
}
