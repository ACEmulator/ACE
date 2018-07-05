using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.NameFilterTable)]
    public class NameFilterTable : FileType
    {
        internal const uint FILE_ID = 0x0E000020;

        // Key is a list of a WCIDs that are "bad" and should not exist. The value is always 1 (could be a bool?)
        public Dictionary<uint, NameFilterLanguageData> LanguageData = new Dictionary<uint, NameFilterLanguageData>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            ushort totalObjects = reader.ReadByte();
            reader.ReadByte(); // table size
            for (var i = 0; i < totalObjects; i++)
            {
                uint key = reader.ReadUInt32();
                NameFilterLanguageData val = new NameFilterLanguageData();
                val.Unpack(reader);
                LanguageData.Add(key, val);
            }
        }
    }
}
