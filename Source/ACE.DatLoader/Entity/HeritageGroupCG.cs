using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class HeritageGroupCG : IUnpackable
    {
        public string Name { get; private set; }
        public uint IconImage { get; private set; }
        public uint SetupID { get; private set; } // Basic character model
        public uint EnvironmentSetupID { get; private set; } // This is the background environment during Character Creation
        public uint AttributeCredits { get; private set; }
        public uint SkillCredits { get; private set; }
        public List<int> PrimaryStartAreas { get; } = new List<int>();
        public List<int> SecondaryStartAreas { get; } = new List<int>();
        public List<SkillCG> Skills { get; } = new List<SkillCG>();
        public List<TemplateCG> Templates { get; } = new List<TemplateCG>();
        public Dictionary<int, SexCG> Genders { get; } = new Dictionary<int, SexCG>();

        public void Unpack(BinaryReader reader)
        {
            Name                = reader.ReadString();
            IconImage           = reader.ReadUInt32();
            SetupID             = reader.ReadUInt32();
            EnvironmentSetupID  = reader.ReadUInt32();
            AttributeCredits    = reader.ReadUInt32();
            SkillCredits        = reader.ReadUInt32();

            PrimaryStartAreas.UnpackSmartArray(reader);
            SecondaryStartAreas.UnpackSmartArray(reader);
            Skills.UnpackSmartArray(reader);
            Templates.UnpackSmartArray(reader);
            reader.BaseStream.Position++; // 0x01 byte here. Not sure what/why, so skip it!
            Genders.UnpackSmartArray(reader);
        }
    }
}
