using System;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateVital : GameMessage
    {
        public GameMessagePrivateUpdateVital(Session session, PropertyAttribute2nd attribute, CreatureVital cv) :
            this(session, attribute, cv.Ranks, cv.StartingValue, cv.ExperienceSpent, cv.Current) { }

        public GameMessagePrivateUpdateVital(Session session, PropertyAttribute2nd attribute, uint ranks, uint baseValue, uint totalInvestment, uint currentValue)
            : base(GameMessageOpcode.PrivateUpdateVital, GameMessageGroup.UIQueue)
        {
            // TODO We shouldn't be passing session. Insetad, we should pass the value after session.UpdateSkillSequence++.

            //Vital vital;

            switch (attribute)
            {
                case PropertyAttribute2nd.MaxHealth:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelHealth));
                    break;
                case PropertyAttribute2nd.MaxStamina:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelStamina));
                    break;
                case PropertyAttribute2nd.MaxMana:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute2ndLevelMana));
                    break;
                default:
                    throw new ArgumentException("invalid ability specified");
            }

            //Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute));
            Writer.Write((uint)attribute);
            Writer.Write(ranks);
            Writer.Write(baseValue);
            Writer.Write(totalInvestment);
            Writer.Write(currentValue);
        }
    }
}
