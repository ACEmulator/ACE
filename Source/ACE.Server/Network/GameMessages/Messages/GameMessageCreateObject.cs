using ACE.Server.Entity;

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