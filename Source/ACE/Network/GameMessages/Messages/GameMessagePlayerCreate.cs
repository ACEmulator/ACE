
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePlayerCreate : GameMessage
    {
        public GameMessagePlayerCreate(ObjectGuid guid) : base(GameMessageOpcode.PlayerCreate, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(guid);
        }
    }
}