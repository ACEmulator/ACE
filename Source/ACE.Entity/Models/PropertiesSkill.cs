using System;

using ACE.Entity.Enum;

namespace ACE.Entity.Models
{
    public class PropertiesSkill
    {
        public ushort LevelFromPP { get; set; }
        public SkillAdvancementClass SAC { get; set; }
        public uint PP { get; set; }
        public uint InitLevel { get; set; }
        public uint ResistanceAtLastCheck { get; set; }
        public double LastUsedTime { get; set; }

        public PropertiesSkill Clone()
        {
            var result = new PropertiesSkill
            {
                LevelFromPP = LevelFromPP,
                SAC = SAC,
                PP = PP,
                InitLevel = InitLevel,
                ResistanceAtLastCheck = ResistanceAtLastCheck,
                LastUsedTime = LastUsedTime,
            };

            return result;
        }
    }
}
