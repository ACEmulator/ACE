using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class LocationType : IUnpackable
    {
        public uint PartId { get; private set; }
        public Position Frame { get; } = new Position();

        public void Unpack(BinaryReader reader)
        {
            PartId = reader.ReadUInt32();
            Frame.Read(reader);
        }
    }
}
