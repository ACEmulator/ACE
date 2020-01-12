using System;
using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.SkillTable)]
    public class SkillTable : FileType
    {
        internal const uint FILE_ID = 0x0E000004;

        // Key is the SkillId
        public Dictionary<uint, SkillBase> SkillBaseHash = new Dictionary<uint, SkillBase>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            SkillBaseHash.UnpackPackedHashTable(reader);
        }

        public void AddRetiredSkills()
        {
            SkillBaseHash.Add((int)Skill.Axe, new SkillBase(new SkillFormula(PropertyAttribute.Strength, PropertyAttribute.Coordination, 3)));
            SkillBaseHash.Add((int)Skill.Bow, new SkillBase(new SkillFormula(PropertyAttribute.Coordination, PropertyAttribute.Undef, 2)));
            SkillBaseHash.Add((int)Skill.Crossbow, new SkillBase(new SkillFormula(PropertyAttribute.Coordination, PropertyAttribute.Undef, 2)));
            SkillBaseHash.Add((int)Skill.Dagger, new SkillBase(new SkillFormula(PropertyAttribute.Quickness, PropertyAttribute.Coordination, 3)));
            SkillBaseHash.Add((int)Skill.Mace, new SkillBase(new SkillFormula(PropertyAttribute.Strength, PropertyAttribute.Coordination, 3)));
            SkillBaseHash.Add((int)Skill.Spear, new SkillBase(new SkillFormula(PropertyAttribute.Strength, PropertyAttribute.Coordination, 3)));
            SkillBaseHash.Add((int)Skill.Staff, new SkillBase(new SkillFormula(PropertyAttribute.Strength, PropertyAttribute.Coordination, 3)));
            SkillBaseHash.Add((int)Skill.Sword, new SkillBase(new SkillFormula(PropertyAttribute.Strength, PropertyAttribute.Coordination, 3)));
            SkillBaseHash.Add((int)Skill.ThrownWeapon, new SkillBase(new SkillFormula(PropertyAttribute.Coordination, PropertyAttribute.Undef, 2)));
            SkillBaseHash.Add((int)Skill.UnarmedCombat, new SkillBase(new SkillFormula(PropertyAttribute.Strength, PropertyAttribute.Coordination, 3)));
        }
    }
}
