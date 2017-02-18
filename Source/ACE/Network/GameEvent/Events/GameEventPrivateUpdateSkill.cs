using System;

using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventPrivateUpdateSkill : GameEventPacket
    {
        private Skill skill;
        private SkillStatus status;
        private uint ranks;
        private uint baseValue;
        private uint totalInvestment;

        public override GameEventOpcode Opcode { get { return GameEventOpcode.PrivateUpdateSkill; } }

        public GameEventPrivateUpdateSkill(Session session, Skill skill, SkillStatus status, uint ranks, uint baseValue, uint totalInvestment) 
            : base(session)
        {
            this.skill = skill;
            this.status = status;
            this.ranks = ranks;
            this.baseValue = baseValue;
            this.totalInvestment = totalInvestment;
        }

        protected override void WriteEventBody()
        {
            fragment.Payload.Write(session.UpdateSkillSequence++);
            fragment.Payload.Write((uint)skill);
            fragment.Payload.Write(Convert.ToUInt16(this.ranks));
            fragment.Payload.Write(Convert.ToUInt16(1)); // no clue, but this makes it work.
            fragment.Payload.Write((uint)status);
            fragment.Payload.Write(this.totalInvestment);

            // not sure what's in these, but anything in the first DWORD gets added to your current skill value - augmentations perhaps?
            fragment.Payload.Write(0u);
            fragment.Payload.Write(0u);
            fragment.Payload.Write(0u);
            fragment.Payload.Write(0u);
        }
    }
}
