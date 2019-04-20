using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class TemplateCG : IUnpackable
    {
        public string Name { get; private set; }
        public uint IconImage { get; private set; }
        public uint Title { get; private set; }
        public uint Strength { get; private set; }
        public uint Endurance { get; private set; }
        public uint Coordination { get; private set; }
        public uint Quickness { get; private set; }
        public uint Focus { get; private set; }
        public uint Self { get; private set; }
        public List<uint> NormalSkillsList { get; } = new List<uint>();
        public List<uint> PrimarySkillsList { get; } = new List<uint>();

        public void Unpack(BinaryReader reader)
        {
            Name            = reader.ReadString();
            IconImage       = reader.ReadUInt32();
            Title           = reader.ReadUInt32();
            // Attributes
            Strength        = reader.ReadUInt32();
            Endurance       = reader.ReadUInt32();
            Coordination    = reader.ReadUInt32();
            Quickness       = reader.ReadUInt32();
            Focus           = reader.ReadUInt32();
            Self            = reader.ReadUInt32();

            NormalSkillsList.UnpackSmartArray(reader);
            PrimarySkillsList.UnpackSmartArray(reader);
        }
    }
}
