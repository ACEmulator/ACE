using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageUpdatePosition : GameMessage
    {
        public GameMessageUpdatePosition(WorldObject worldObject, bool forcePos = false)
            : base(GameMessageOpcode.UpdatePosition, GameMessageGroup.SmartboxQueue)
        {
            worldObject.WriteUpdatePositionPayload(Writer, forcePos);
        }
    }
}
