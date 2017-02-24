using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCreateObject : GameMessage
    {
        public GameMessageCreateObject(WorldObject worldObject) 
            : base(GameMessageOpcode.ObjectCreate, 0xA)
        {
            worldObject.WriteCreateObjectPayload(this.Writer);
        }
    }
}