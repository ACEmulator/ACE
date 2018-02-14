using System.Collections.Generic;
using System.IO;

using ACE.Common.Extensions;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class CharacterCreateInfo
    {
        public uint Heritage { get; private set; }
        public uint Gender { get; private set; }

        public Appearance Apperance { get; } = new Appearance();

        public int TemplateOption { get; private set; }

        public uint StrengthAbility { get; private set; }
        public uint EnduranceAbility { get; private set; }
        public uint CoordinationAbility { get; private set; }
        public uint QuicknessAbility { get; private set; }
        public uint FocusAbility { get; private set; }
        public uint SelfAbility { get; private set; }

        public uint CharacterSlot { get; private set; }
        public uint ClassId { get; private set; }

        public List<SkillStatus> SkillStatuses = new List<SkillStatus>();

        public string Name { get; private set; }

        public uint StartArea { get; private set; }

        public bool IsAdmin { get; private set; }
        public bool IsEnvoy { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            reader.BaseStream.Position += 4;   /* Unknown constant (1) */

            Heritage    = reader.ReadUInt32();
            Gender      = reader.ReadUInt32();

            Apperance.Unpack(reader);

            TemplateOption = reader.ReadInt32();

            StrengthAbility     = reader.ReadUInt32();
            EnduranceAbility    = reader.ReadUInt32();
            CoordinationAbility = reader.ReadUInt32();
            QuicknessAbility    = reader.ReadUInt32();
            FocusAbility        = reader.ReadUInt32();
            SelfAbility         = reader.ReadUInt32();

            CharacterSlot   = reader.ReadUInt32();
            ClassId         = reader.ReadUInt32();

            uint numOfSkills = reader.ReadUInt32();
            for (uint i = 0; i < numOfSkills; i++)
                SkillStatuses.Add((SkillStatus)reader.ReadUInt32());

            Name = reader.ReadString16L();

            StartArea = reader.ReadUInt32();

            IsAdmin = (reader.ReadUInt32() == 1);
            IsEnvoy = (reader.ReadUInt32() == 1);
        }
    }
}
