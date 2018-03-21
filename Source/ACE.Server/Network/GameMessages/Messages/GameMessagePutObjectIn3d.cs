using ACE.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePutObjectIn3d : GameMessage
    {
        public GameMessagePutObjectIn3d(Session session, WorldObject worldObject, ObjectGuid itemGuid)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.UIQueue)
        {
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.InventoryPutObjectIn3D);
            Writer.Write(itemGuid.Full);
            //Writer.Align();
        }
    }
}
