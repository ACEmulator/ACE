using System;
using ACE.Entity.Enum.Properties;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateAbility : GameMessage
    {
        public GameMessagePrivateUpdateAbility(Session session, Entity.Enum.Ability ability, uint ranks, uint baseValue, uint totalInvestment)
            : base(GameMessageOpcode.PrivateUpdateAttribute, GameMessageGroup.Group09)
        {
            // TODO We shouldn't be passing session. Insetad, we should pass the value after session.UpdateSkillSequence++.

            PropertyAttribute networkAbility;

            switch (ability)
            {
                case Entity.Enum.Ability.Strength:
                    networkAbility = PropertyAttribute.Strength;
                    break;
                case Entity.Enum.Ability.Endurance:
                    networkAbility = PropertyAttribute.Endurance;
                    break;
                case Entity.Enum.Ability.Coordination:
                    networkAbility = PropertyAttribute.Coordination;
                    break;
                case Entity.Enum.Ability.Quickness:
                    networkAbility = PropertyAttribute.Quickness;
                    break;
                case Entity.Enum.Ability.Focus:
                    networkAbility = PropertyAttribute.Focus;
                    break;
                case Entity.Enum.Ability.Self:
                    networkAbility = PropertyAttribute.Self;
                    break;
                default:
                    throw new ArgumentException("invalid ability specified");
            }

            Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute));
            Writer.Write((uint)networkAbility);
            Writer.Write(ranks);
            Writer.Write(baseValue);
            Writer.Write(totalInvestment);
        }
    }
}
