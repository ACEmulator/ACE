using ACE.Server.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageUpdatePosition : GameMessage
    {
        public GameMessageUpdatePosition(WorldObject worldObject)
            : base(GameMessageOpcode.UpdatePosition, GameMessageGroup.SmartboxQueue)
        {
            worldObject.WriteUpdatePositionPayload(Writer);
        }
    }
}