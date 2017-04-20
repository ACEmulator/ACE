using ACE.Entity;
using ACE.Entity.Objects;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePutObjectInContainer : GameMessage
    {
        public GameMessagePutObjectInContainer(Session session, WorldObject worldObject, ObjectGuid itemGuid)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(session.GameEventSequence.NextValue);
            Writer.Write((uint)GameEvent.GameEventType.InventoryPutObjInContainer);
            Writer.Write(itemGuid.Full);
            Writer.Write(session.Player.Guid.Full); // TODO: They could be draging to a pack would need the guid of the pack
            Writer.Write((uint)(0)); // TODO: This is the slot - 0 based. 0 lets us drop it in the 0 slot of the main pack.
            Writer.Write((uint)0); // TODO: make enum 0 item, 1 pack, 2 foci
            Writer.Align();
        }
    }
}