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
            switch (attribute)
            {
                case PropertyAttribute.Strength:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeStrength));
                    break;
                case PropertyAttribute.Endurance:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeEndurance));
                    break;
                case PropertyAttribute.Coordination:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeCoordination));
                    break;
                case PropertyAttribute.Quickness:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeQuickness));
                    break;
                case PropertyAttribute.Focus:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeFocus));
                    break;
                case PropertyAttribute.Self:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeSelf));
                    break;
                default:
                    throw new ArgumentException("invalid ability specified");
            }

            Writer.Write((uint)attribute);
            Writer.Write(ranks);
            Writer.Write(baseValue);
            Writer.Write(totalInvestment);
        }
    }
}
