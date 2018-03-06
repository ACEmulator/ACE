using System;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateAbility : GameMessage
    {
        public GameMessagePrivateUpdateAbility(Session session, PropertyAttribute attribute, uint ranks, uint baseValue, uint totalInvestment)
            : base(GameMessageOpcode.PrivateUpdateAttribute, GameMessageGroup.UIQueue)
        {
            // TODO We shouldn't be passing session. Insetad, we should pass the value after session.UpdateSkillSequence++.

            //PropertyAttribute networkAbility;

            switch (attribute)
            {
                case PropertyAttribute.Strength:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeStrength));
                    break;
                case PropertyAttribute.Endurance:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeEndurance));
                    break;
                case PropertyAttribute.Coordination:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeCoordination));
                    break;
                case PropertyAttribute.Quickness:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeQuickness));
                    break;
                case PropertyAttribute.Focus:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeFocus));
                    break;
                case PropertyAttribute.Self:
                    Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttributeSelf));
                    break;
                default:
                    throw new ArgumentException("invalid ability specified");
            }

            //Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute));
            Writer.Write((uint)attribute);
            Writer.Write(ranks);
            Writer.Write(baseValue);
            Writer.Write(totalInvestment);
        }
    }
}
