using System;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateAttribute : GameMessage
    {
        public GameMessagePrivateUpdateAttribute(WorldObject worldObject, PropertyAttribute attribute, uint ranks, uint baseValue, uint totalInvestment)
            : base(GameMessageOpcode.PrivateUpdateAttribute, GameMessageGroup.UIQueue)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdateAttribute, attribute));
            Writer.Write((uint)attribute);
            Writer.Write(ranks);
            Writer.Write(baseValue);
            Writer.Write(totalInvestment);
        }
    }
}
