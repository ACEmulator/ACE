using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventInventoryRemoveObject : GameEventMessage
    {
        public GameEventInventoryRemoveObject(Session session, WorldObject worldObject)
            : base(GameEventType.InventoryRemoveObject, GameMessageGroup.UIQueue, session)
        {
            Writer.WriteGuid(worldObject.Guid);
        }
    }
}
