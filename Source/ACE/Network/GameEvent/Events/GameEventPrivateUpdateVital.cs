using System;

using ACE.Network.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventPrivateUpdateVital : GameEventPacket
    {
        private Vital vital;
        private uint baseValue;
        private uint ranks;
        private uint totalInvestment;
        private uint currentValue;

        public override GameEventOpcode Opcode { get { return GameEventOpcode.PrivateUpdateVital; } }

        public GameEventPrivateUpdateVital(Session session, Entity.Enum.Ability vital, uint ranks, uint baseValue, uint totalInvestment, uint currentValue) 
            : base(session)
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
        }

        protected override void WriteEventBody()
        {
            fragment.Payload.Write(session.UpdateAttributeSequence++);
            fragment.Payload.Write((uint)vital);
            fragment.Payload.Write(this.ranks);
            fragment.Payload.Write(this.baseValue);
            fragment.Payload.Write(this.totalInvestment);
            fragment.Payload.Write(this.currentValue);
        }
    }
}