using System;

using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateSkill : GameMessage
    {
        public GameMessagePrivateUpdateSkill(Session session, Skill skill, SkillStatus status, uint ranks, uint baseValue, uint totalInvestment) : base(GameMessageOpcode.PrivateUpdateSkill)
        {
            // TODO We shouldn't be passing session. Insetad, we should pass the value after session.UpdateSkillSequence++.
            // TODO Why is baseValue being passed to this function even though it's not used?

            Writer.Write(session.UpdateSkillSequence++);
            Writer.Write((uint)skill);
            Writer.Write(Convert.ToUInt16(ranks));
            Writer.Write(Convert.ToUInt16(1)); // no clue, but this makes it work.
            Writer.Write((uint)status);
            Writer.Write(totalInvestment);

            // not sure what's in these, but anything in the first DWORD gets added to your current skill value - augmentations perhaps?
            Writer.Write(0u);
            Writer.Write(0u);
            Writer.Write(0u);
            Writer.Write(0u);
        }
    }
}
