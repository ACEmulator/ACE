using System.Collections.Generic;
using System.IO;
using ACE.Entity.Enum;

namespace ACE.DatLoader.Entity
{
    public class CBldPortal : IUnpackable
    {
        public PortalFlags Flags { get; private set; }

        // Turbine's CBldPortal::UnPack decodes ExactMatch from bit 0 (0x0001),
        // and PortalSide from the inverse of bit 1 (0x0002).
        // The bit behavior is verified; the gameplay meaning of these flags is not.
        public bool ExactMatch => Flags.HasFlag(PortalFlags.ExactMatch);
        public bool PortalSide => !Flags.HasFlag(PortalFlags.PortalSide);

        // Basically the cells that connect both sides of the portal
        public ushort OtherCellId { get; private set; }
        public ushort OtherPortalId { get; private set; }

        /// <summary>
        /// List of cells used in this structure. (Or possibly just those visible through it.)
        /// </summary>
        public List<ushort> StabList { get; } = new List<ushort>();

        public void Unpack(BinaryReader reader)
        {
            Flags = (PortalFlags)reader.ReadUInt16();

            OtherCellId = reader.ReadUInt16();
            OtherPortalId = reader.ReadUInt16();

            ushort num_stabs = reader.ReadUInt16();
            for (var i = 0; i < num_stabs; i++)
                StabList.Add(reader.ReadUInt16());

            reader.AlignBoundary();
        }
    }
}
