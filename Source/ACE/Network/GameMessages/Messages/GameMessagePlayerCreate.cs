
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePlayerCreate : GameMessage
    {
        public GameMessagePlayerCreate(ObjectGuid guid) : base(GameMessageOpcode.PlayerCreate, 0xA)
        {
            Writer.WriteGuid(guid);
        }
    }
}