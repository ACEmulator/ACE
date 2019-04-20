using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateDataID : GameMessage
    {
        public GameMessagePrivateUpdateDataID(WorldObject worldObject, PropertyDataId property, uint value)
            : base(GameMessageOpcode.PrivateUpdatePropertyDataID, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdatePropertyDataID, property));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
