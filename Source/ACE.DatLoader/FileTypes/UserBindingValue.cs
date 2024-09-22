using System.IO;

namespace ACE.DatLoader.Entity
{
    public class UserBindingValue : IUnpackable
    {
        public uint ActionClass { get; private set; }

        // String hash from the StringTable
        public uint ActionName { get; private set; }

        // String hash from the StringTable
        public uint Description { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            ActionClass = reader.ReadUInt32();
            ActionName = reader.ReadUInt32();
            Description = reader.ReadUInt32();
        }
    }
}
