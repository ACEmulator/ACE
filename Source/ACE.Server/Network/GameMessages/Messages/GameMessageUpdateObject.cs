using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageUpdateObject : GameMessage
    {
        public GameMessageUpdateObject(WorldObject worldObject)
            : base(GameMessageOpcode.UpdateObject, GameMessageGroup.SmartboxQueue)
        {
            worldObject.SerializeUpdateObject(this.Writer);
        }
    }
}
