using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.StringTable)]
    public class StringTable : FileType
    {
        public uint Language { get; private set; } // This should always be 1 for English

        public byte Unknown { get; private set; }

        public List<StringTableData> StringTableData { get; } = new List<StringTableData>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            Language = reader.ReadUInt32();

            Unknown = reader.ReadByte();

            StringTableData.Unpack(reader, true);
        }
    }
}
