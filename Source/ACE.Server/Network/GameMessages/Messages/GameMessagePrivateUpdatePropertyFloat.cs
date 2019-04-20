using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdatePropertyFloat : GameMessage
    {
        public GameMessagePrivateUpdatePropertyFloat(WorldObject worldObject, PropertyFloat property, double value)
            : base(GameMessageOpcode.PrivateUpdatePropertyFloat, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdatePropertyDouble, property));
            Writer.Write((uint)property);
            Writer.Write(value);
        }
    }
}
