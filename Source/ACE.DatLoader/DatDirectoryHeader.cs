using System.IO;

namespace ACE.DatLoader
{
    public class DatDirectoryHeader : IUnpackable
    {
        internal static readonly uint ObjectSize = ((sizeof(uint) * 0x3E) + sizeof(uint) + (DatFile.ObjectSize * 0x3D));


        public uint[] Branches { get; } = new uint[0x3E];
        public uint EntryCount { get; private set; }
        public DatFile[] Entries { get; } = new DatFile[0x3D];

        public void Unpack(BinaryReader reader)
        {
            for (int i = 0; i < Branches.Length; i++)
                Branches[i] = reader.ReadUInt32();

            EntryCount = reader.ReadUInt32();

            for (int i = 0; i < Entries.Length; i++)
            {
                Entries[i] = new DatFile();
                Entries[i].Unpack(reader);
            }
        }
    }
}
