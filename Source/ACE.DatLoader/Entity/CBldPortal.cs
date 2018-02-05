using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class CBldPortal : IUnpackable
    {
        public ushort Flags { get; private set; }

        // Not sure what these do. They are both calculated from the flags.
        public bool ExactMatch => (Flags & 1) != 0;
        public bool PortalSide => (Flags & 2) == 0;

        // Basically the cells that connect both sides of the portal
        public ushort OtherCellId { get; private set; }
        public ushort OtherPortalId { get; private set; }

        /// <summary>
        /// List of cells used in this structure. (Or possibly just those visible through it.)
        /// </summary>
        public List<ushort> StabList { get; } = new List<ushort>();

        public void Unpack(BinaryReader reader)
        {
            Flags = reader.ReadUInt16();

            OtherCellId = reader.ReadUInt16();
            OtherPortalId = reader.ReadUInt16();

            ushort num_stabs = reader.ReadUInt16();
            for (var i = 0; i < num_stabs; i++)
                StabList.Add(reader.ReadUInt16());

            reader.AlignBoundary();
        }
    }
}
