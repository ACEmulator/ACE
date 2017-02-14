using ACE.Entity.Enum;
using System;

namespace ACE.Network
{
    public class GameMessagePrivateUpdateSkill : GameMessage
    {
        private Skill skill;
        private SkillStatus status;
        private uint ranks;
        private uint baseValue;
        private uint totalInvestment;

        public GameMessagePrivateUpdateSkill(Session session, Skill skill, SkillStatus status, uint ranks, uint baseValue, uint totalInvestment) 
            : base(session, GameMessageOpcode.PrivateUpdateSkill)
        {
            this.skill = skill;
            this.status = status;
            this.ranks = ranks;
            this.baseValue = baseValue;
            this.totalInvestment = totalInvestment;
        }

        protected override void WriteBody()
        {
            writer.Write(session.UpdateSkillSequence++);
            writer.Write((uint)skill);
            writer.Write(Convert.ToUInt16(this.ranks));
            writer.Write(Convert.ToUInt16(1)); // no clue, but this makes it work.
            writer.Write((uint)status);
            writer.Write(this.totalInvestment);

            // not sure what's in these, but anything in the first DWORD gets added to your current skill value - augmentations perhaps?
            writer.Write(0u);
            writer.Write(0u);
            writer.Write(0u);
            writer.Write(0u);
        }
    }
}
