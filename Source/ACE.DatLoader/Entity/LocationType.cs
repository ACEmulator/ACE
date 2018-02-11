using System.IO;

namespace ACE.DatLoader.Entity
{
    public class LocationType : IUnpackable
    {
        public uint PartId { get; private set; }
        public Frame Frame { get; } = new Frame();

        public void Unpack(BinaryReader reader)
        {
            PartId = reader.ReadUInt32();
            Frame.Unpack(reader);
        }
    }
}
