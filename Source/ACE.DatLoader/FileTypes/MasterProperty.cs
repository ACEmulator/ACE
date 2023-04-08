using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.MasterProperty)]
    public class MasterProperty : FileType
    {
        internal const uint FILE_ID = 0x39000001;

        public Dictionary<uint, string> m_emapper { get; } = new Dictionary<uint, string>();
        public Dictionary<uint, BasePropertyDesc> m_properties { get; } = new Dictionary<uint, BasePropertyDesc>();

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            reader.ReadUInt32(); // 0, Unknown
            reader.ReadUInt32(); // 0, Unknown

            reader.ReadByte(); // = 5, Unknown (Bucket?)

            uint numEnums = reader.ReadCompressedUInt32();
            for(var i = 0; i < numEnums; i++)
            {
                var key = reader.ReadUInt32();
                var value = reader.ReadPString(1);
                m_emapper.Add(key, value);
            }

            reader.ReadByte(); // = 6, Unknown (Bucket?)

            m_properties.UnpackSmartArray(reader);
        }
    }
}
