using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageScript : GameMessage
    {
        public GameMessageScript(global::ACE.Entity.ObjectGuid guid, PlayScript scriptId, float scale = 1f)
            : base(GameMessageOpcode.PlayEffect, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)scriptId);
            Writer.Write(scale); // scale ?
        }
    }
}
