using System.IO;

namespace ACE.DatLoader
{
    /// <summary>
    /// DiskFileInfo_t in the client
    /// </summary>
    public class DatDatabaseHeader : IUnpackable
    {
        public uint FileType { get; private set; }
        public uint BlockSize { get; private set; }
        public uint FileSize { get; private set; }
        public DatDatabaseType DataSet { get; private set; }
        public uint DataSubset { get; private set; }

        public uint FreeHead { get; private set; }
        public uint FreeTail { get; private set; }
        public uint FreeCount { get; private set; }
        public uint BTree { get; private set; }

        public uint NewLRU { get; private set; }
        public uint OldLRU { get; private set; }
        public bool UseLRU { get; private set; }

        public uint MasterMapID { get; private set; }

        public uint EnginePackVersion { get; private set; }
        public uint GamePackVersion { get; private set; }
        public byte[] VersionMajor { get; private set; } = new byte[16];
        public uint VersionMinor { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            FileType    = reader.ReadUInt32();
            BlockSize   = reader.ReadUInt32();
            FileSize    = reader.ReadUInt32();
            DataSet     = (DatDatabaseType)reader.ReadUInt32();
            DataSubset  = reader.ReadUInt32();

            FreeHead    = reader.ReadUInt32();
            FreeTail    = reader.ReadUInt32();
            FreeCount   = reader.ReadUInt32();
            BTree       = reader.ReadUInt32();

            NewLRU      = reader.ReadUInt32();
            OldLRU      = reader.ReadUInt32();
            UseLRU      = (reader.ReadUInt32() == 1);

            MasterMapID = reader.ReadUInt32();

            EnginePackVersion   = reader.ReadUInt32();
            GamePackVersion     = reader.ReadUInt32();
            VersionMajor        = reader.ReadBytes(16);
            VersionMinor        = reader.ReadUInt32();
        }
    }
}
