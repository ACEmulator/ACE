using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageUpdateObject : GameMessage
    {
        public GameMessageUpdateObject(WorldObject worldObject, bool adminvision = false, bool changenodraw = false)
            : base(GameMessageOpcode.UpdateObject, GameMessageGroup.SmartboxQueue)
        {
            worldObject.SerializeUpdateObject(Writer, adminvision, changenodraw);
        }
    }
}
