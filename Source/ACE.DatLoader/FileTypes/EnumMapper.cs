using ACE.Entity.Enum;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.EnumMapper)]
    public class EnumMapper : FileType
    {
        public uint BaseEnumMap { get; private set; }   // m_base_emp_did
        public NumberingType NumberingType { get; private set; }
        public Dictionary<uint, string> IdToStringMap { get; private set; } = new Dictionary<uint, string>();    // m_id_to_string_map

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            BaseEnumMap = reader.ReadUInt32();

            NumberingType = (NumberingType)reader.ReadByte();

            uint num_enums = reader.ReadCompressedUInt32();
            for (var i = 0; i < num_enums; i++)
                IdToStringMap.Add(reader.ReadUInt32(), reader.ReadPString(1));
        }
    }
}
