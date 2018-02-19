using System;

using ACE.Entity.Enum;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateVital : GameMessage
    {
        public GameMessagePrivateUpdateVital(Session session, Ability ability, CreatureVital cv) :
            this(session, ability, cv.Ranks, cv.StartingValue, cv.ExperienceSpent, cv.Current) { }

        public GameMessagePrivateUpdateVital(Session session, Ability ability, uint ranks, uint baseValue, uint totalInvestment, uint currentValue)
            : base(GameMessageOpcode.PrivateUpdateVital, GameMessageGroup.UIQueue)
        {
            // TODO We shouldn't be passing session. Insetad, we should pass the value after session.UpdateSkillSequence++.

            Vital vital;

            switch (ability)
            {
                case Ability.Health:
                    vital = Vital.Health;
                    break;
                case Ability.Stamina:
                    vital = Vital.Stamina;
                    break;
                case Ability.Mana:
                    vital = Vital.Mana;
                    break;
                default:
                    throw new ArgumentException("invalid ability specified");
            }

            Writer.Write(session.Player.Sequences.GetNextSequence(Sequence.SequenceType.PrivateUpdateAttribute));
            Writer.Write((uint)vital);
            Writer.Write(ranks);
            Writer.Write(baseValue);
            Writer.Write(totalInvestment);
            Writer.Write(currentValue);
        }
    }
}
