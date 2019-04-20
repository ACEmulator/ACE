using System;
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessagePrivateUpdateSkill : GameMessage
    {
        public GameMessagePrivateUpdateSkill(WorldObject worldObject, CreatureSkill skill)
            : base(GameMessageOpcode.PrivateUpdateSkill, GameMessageGroup.UIQueue)
        {
            UpdateSkill(worldObject, skill.Skill, skill.AdvancementClass, skill.Ranks, skill.InitLevel, skill.ExperienceSpent);
        }

        public GameMessagePrivateUpdateSkill(WorldObject worldObject, Skill skill, SkillAdvancementClass status, uint ranks, uint bonus, uint totalInvestment)
            : base(GameMessageOpcode.PrivateUpdateSkill, GameMessageGroup.UIQueue)
        {
            // TODO: deprecate
            UpdateSkill(worldObject, skill, status, ranks, bonus, totalInvestment);
        }

        public void UpdateSkill(WorldObject worldObject, Skill skill, SkillAdvancementClass status, uint ranks, uint bonus, uint totalInvestment)
        {
            Writer.Write(worldObject.Sequences.GetNextSequence(Sequence.SequenceType.UpdateSkill, skill));

            ushort adjustPP = 1;            // If this is not 0, it appears to trigger the initLevel to be treated as extra XP applied to the skill
            uint resistanceOfLastCheck = 0; // last use difficulty;
            double lastUsedTime = 0;        // time skill was last used;

            Writer.Write((uint)skill);
            Writer.Write(Convert.ToUInt16(ranks));
            Writer.Write(adjustPP);
            Writer.Write((uint)status);
            Writer.Write(totalInvestment);

            Writer.Write(bonus);            // starting point for advancement of the skill (eg bonus points)
            Writer.Write(resistanceOfLastCheck);
            Writer.Write(lastUsedTime);
        }
    }
}
