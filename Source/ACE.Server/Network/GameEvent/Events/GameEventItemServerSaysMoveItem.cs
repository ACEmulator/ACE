using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventItemServerSaysMoveItem : GameEventMessage
    {
        public GameEventItemServerSaysMoveItem(Session session, WorldObject worldObject)
            : base(GameEventType.InventoryPutObjectIn3D, GameMessageGroup.UIQueue, session, 8)
        {
            Writer.WriteGuid(worldObject.Guid);
        }
    }
}
