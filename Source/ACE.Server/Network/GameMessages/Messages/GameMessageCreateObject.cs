using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCreateObject : GameMessage
    {
        public GameMessageCreateObject(WorldObject worldObject, bool adminvision = false, bool adminnodraw = false)
            : base(GameMessageOpcode.ObjectCreate, GameMessageGroup.SmartboxQueue)
        {
            worldObject.SerializeCreateObject(Writer, adminvision, adminnodraw);
        }
    }
}
