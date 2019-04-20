using System.IO;

namespace ACE.DatLoader.Entity
{
    public class PortalPoly : IUnpackable
    {
        public short PortalIndex { get; set; }
        public short PolygonId { get; set; }

        public void Unpack(BinaryReader reader)
        {
            PortalIndex = reader.ReadInt16();
            PolygonId   = reader.ReadInt16();
        }
    }
}
