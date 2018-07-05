using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    }
}
