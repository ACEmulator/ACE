using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateVital : GameMessage
    {
        public GameMessagePrivateUpdateVital(WorldObject worldObject, PropertyAttribute2nd attribute, uint ranks, uint baseValue, uint totalInvestment, uint currentValue)
            : base(GameMessageOpcode.PrivateUpdateVital, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdateAttribute2ndLevel, attribute));

            Writer.Write((uint)attribute);
            Writer.Write(ranks);
            Writer.Write(baseValue);
            Writer.Write(totalInvestment);
            Writer.Write(currentValue);
        }
    }
}
