using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageUpdatePosition : GameMessage
    {
        public GameMessageUpdatePosition(WorldObject worldObject) 
            : base(GameMessageOpcode.UpdatePosition, 0xA)
        {
            worldObject.WriteUpdatePositionPayload(Writer);
        }
    }
}