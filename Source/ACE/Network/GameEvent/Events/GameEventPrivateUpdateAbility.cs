namespace ACE.Network.GameEvent
{
    public class GameEventPrivateUpdateAbility : GameEventPacket
    {
        private Network.Ability networkAbility;
        private uint ranks;
        private uint baseValue;
        private uint totalInvestment;

        public override GameEventOpcode Opcode { get { return GameEventOpcode.PrivateUpdateAttribute; } }

        public GameEventPrivateUpdateAbility(Session session, Entity.Enum.Ability ability, uint ranks, uint baseValue, uint totalInvestment) 
            : base(session)
        {
            switch (ability)
            {
                case Entity.Enum.Ability.Strength:
                    networkAbility = Ability.Strength;
                    break;
                case Entity.Enum.Ability.Endurance:
                    networkAbility = Ability.Endurance;
                    break;
                case Entity.Enum.Ability.Coordination:
                    networkAbility = Ability.Coordination;
                    break;
                case Entity.Enum.Ability.Quickness:
                    networkAbility = Ability.Quickness;
                    break;
                case Entity.Enum.Ability.Focus:
                    networkAbility = Ability.Focus;
                    break;
                case Entity.Enum.Ability.Self:
                    networkAbility = Ability.Self;
                    break;
            }

            this.ranks = ranks;
            this.baseValue = baseValue;
            this.totalInvestment = totalInvestment;
        }

        protected override void WriteEventBody()
        {
            fragment.Payload.Write(session.UpdateAttributeSequence++);
            fragment.Payload.Write((uint)networkAbility);
            fragment.Payload.Write(this.ranks);
            fragment.Payload.Write(this.baseValue);
            fragment.Payload.Write(this.totalInvestment);
        }
    }
}
