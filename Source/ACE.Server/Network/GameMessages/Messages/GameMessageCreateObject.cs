using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCreateObject : GameMessage
    {
        public GameMessageCreateObject(WorldObject worldObject)
            : base(GameMessageOpcode.ObjectCreate, GameMessageGroup.SmartboxQueue)
        {
            worldObject.SerializeCreateObject(this.Writer);
        }
    }
}
