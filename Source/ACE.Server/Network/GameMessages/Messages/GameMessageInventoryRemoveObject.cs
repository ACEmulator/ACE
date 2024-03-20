
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageInventoryRemoveObject : GameMessage
    {
        public GameMessageInventoryRemoveObject(WorldObject worldObject)
            : base(GameMessageOpcode.InventoryRemoveObject, GameMessageGroup.UIQueue, 8)
        {
            Writer.WriteGuid(worldObject.Guid);
        }
    }
}
