using System;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateVital : GameMessage
    {
        public GameMessagePrivateUpdateVital(WorldObject worldObject, PropertyAttribute2nd attribute, uint ranks, uint baseValue, uint totalInvestment, uint currentValue)
            : base(GameMessageOpcode.PrivateUpdateVital, GameMessageGroup.UIQueue)
        {
            switch (attribute)
            {
                case PropertyAttribute2nd.MaxHealth:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelHealth));
                    break;
                case PropertyAttribute2nd.MaxStamina:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelStamina));
                    break;
                case PropertyAttribute2nd.MaxMana:
                    Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelMana));
                    break;
                default:
                    throw new ArgumentException("invalid ability specified");
            }

            Writer.Write((uint)attribute);
            Writer.Write(ranks);
            Writer.Write(baseValue);
            Writer.Write(totalInvestment);
            Writer.Write(currentValue);
        }
    }
}
