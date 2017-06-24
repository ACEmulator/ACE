using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePutObjectInContainer : GameMessage
    {
        public GameMessagePutObjectInContainer(Session session, WorldObject worldObject, WorldObject item, uint location)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.Write(session.Player.Guid.Full);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.InventoryPutObjInContainer);
            Writer.Write(item.Guid.Full);
            Writer.Write(worldObject.Guid.Full);
            Writer.Write(location);
            Writer.Write((uint)item.ContainerType);
            Writer.Align();
        }
    }
}