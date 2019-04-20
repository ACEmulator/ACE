using ACE.Entity.Enum.Properties;
using ACE.Server.Network.Sequence;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageSetStackSize : GameMessage
    {
        public GameMessageSetStackSize(WorldObject worldObject)
            : base(GameMessageOpcode.SetStackSize, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(SequenceType.UpdatePropertyInt, PropertyInt.StackSize));
            Writer.WriteGuid(worldObject.Guid);
            Writer.Write((uint)(worldObject.StackSize ?? 0));
            Writer.Write((uint)(worldObject.Value ?? 0));
        }
    }
}
