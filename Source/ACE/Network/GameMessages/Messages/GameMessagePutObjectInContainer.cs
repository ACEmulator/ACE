using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePutObjectInContainer : GameMessage
    {
        public GameMessagePutObjectInContainer(Session session, WorldObject worldObject, WorldObject item)
            : base(GameMessageOpcode.GameEvent, GameMessageGroup.Group09)
        {
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write(session.GameEventSequence++);
            Writer.Write((uint)GameEvent.GameEventType.InventoryPutObjInContainer);
            Writer.Write(item.Guid.Full);
            Writer.Write(session.Player.Guid.Full); // TODO: They could be draging to a pack would need the guid of the pack
            Writer.Write((uint)(0)); // TODO: This is the slot - 0 based. 0 lets us drop it in the 0 slot of the main pack.
            if (item.WeenieClassid != 136)
                Writer.Write((uint)0); // TODO: make enum 0 item, 1 pack, 2 foci
            else
                Writer.Write((uint)1); // TODO: make enum 0 item, 1 pack, 2 foci
            Writer.Align();
        }
    }
}