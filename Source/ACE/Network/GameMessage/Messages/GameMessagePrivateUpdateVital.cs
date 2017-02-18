using System;

using ACE.Network.Enum;

namespace ACE.Network.Messages
{
    public class GameMessagePrivateUpdateVital : GameMessage
    {
        private Vital vital;
        private uint baseValue;
        private uint ranks;
        private uint totalInvestment;
        private uint currentValue;

        public GameMessagePrivateUpdateVital(Session session, Entity.Enum.Ability vital, uint ranks, uint baseValue, uint totalInvestment, uint currentValue) 
            : base(GameMessageOpcode.PrivateUpdateVital)
        {
            switch (vital)
            {
                case Entity.Enum.Ability.Health:
                    this.vital = Vital.Health;
                    break;
                case Entity.Enum.Ability.Stamina:
                    this.vital = Vital.Stamina;
                    break;
                case Entity.Enum.Ability.Mana:
                    this.vital = Vital.Mana;
                    break;
                default:
                    throw new ArgumentException("invalid vital specified");
            }
            this.ranks = ranks;
            this.baseValue = baseValue;
            this.totalInvestment = totalInvestment;
            this.currentValue = currentValue;
            writer.Write(session.UpdateAttributeSequence++);
            writer.Write((uint)this.vital);
            writer.Write(this.ranks);
            writer.Write(this.baseValue);
            writer.Write(this.totalInvestment);
            writer.Write(this.currentValue);
        }
    }
}