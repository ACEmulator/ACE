using System.IO;

namespace ACE.DatLoader.Entity
{
    public class CellPortal : IUnpackable
    {
        public ushort Bitfield { get; private set; }
        public ushort PolygonId { get; private set; }
        public ushort OtherCellId { get; private set; }
        public ushort OtherPortalId { get; private set; }

        public bool ExactMatch => (Bitfield & 1) != 0;
        public bool PortalSide => (Bitfield & 2) == 0;

        public void Unpack(BinaryReader reader)
        {
            Bitfield        = reader.ReadUInt16();
            PolygonId       = reader.ReadUInt16();
            OtherCellId     = reader.ReadUInt16();
            OtherPortalId   = reader.ReadUInt16();
        }
    }
}
