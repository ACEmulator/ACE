﻿using System;
using ACE.Network.Enum;
using ACE.Entity;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateVital : GameMessage
    {
        public GameMessagePrivateUpdateVital(Session session, Entity.Enum.Ability ability, CreatureAbility ca) :
            this(session, ability, ca.Ranks, ca.Base, ca.ExperienceSpent, ca.Current) { }

        public GameMessagePrivateUpdateVital(Session session, Entity.Enum.Ability ability, uint ranks, uint baseValue, uint totalInvestment, uint currentValue)
            : base(GameMessageOpcode.PrivateUpdateVital, GameMessageGroup.Group09)
        {
            // TODO We shouldn't be passing session. Insetad, we should pass the value after session.UpdateSkillSequence++.

            Vital vital;

            switch (ability)
            {
                case Entity.Enum.Ability.Health:
                    vital = Vital.Health;
                    break;
                case Entity.Enum.Ability.Stamina:
                    vital = Vital.Stamina;
                    break;
                case Entity.Enum.Ability.Mana:
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