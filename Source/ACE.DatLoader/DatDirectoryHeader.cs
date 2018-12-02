using System.IO;

namespace ACE.DatLoader
{
    public class DatDirectoryHeader : IUnpackable
    {
        internal static readonly uint ObjectSize = ((sizeof(uint) * 0x3E) + sizeof(uint) + (DatFile.ObjectSize * 0x3D));

        public uint[] Branches { get; } = new uint[0x3E];
        public uint EntryCount { get; private set; }
        public DatFile[] Entries { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            for (int i = 0; i < Branches.Length; i++)
                Branches[i] = reader.ReadUInt32();

            EntryCount = reader.ReadUInt32();

            Entries = new DatFile[EntryCount];

            for (int i = 0; i < EntryCount; i++)
            {
                Entries[i] = new DatFile();
                Entries[i].Unpack(reader);
            }
        }
    }
}
