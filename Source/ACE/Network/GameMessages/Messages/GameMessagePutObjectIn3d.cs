using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePutObjectIn3D : GameMessage
    {
        public GameMessagePutObjectIn3D(Session session, WorldObject worldObject, ObjectGuid itemGuid)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.InventoryPutObjIn3D);
            Writer.Write(itemGuid.Full);
            Writer.Align();
        }
    }
}