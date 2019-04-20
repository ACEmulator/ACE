using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageScript : GameMessage
    {
        public GameMessageScript(ObjectGuid guid, PlayScript scriptId, float speed = 1.0f)
            : base(GameMessageOpcode.PlayEffect, GameMessageGroup.SmartboxQueue)
        {
            Writer.WriteGuid(guid);
            Writer.Write((uint)scriptId);
            Writer.Write(speed);
        }
    }
}
