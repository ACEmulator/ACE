using System.Collections.Generic;

namespace ACE.DatLoader.Entity
{
    public class CBldPortal
    {
        public ushort Flags { get; set; }

        // Not sure what these do. They are both calculated from the flags.
        public bool ExactMatch => (Flags & 1) != 0;
        public bool PortalSide => (Flags & 2) != 0;

        // Basically the cells that connect both sides of the portal
        public uint OtherCellId { get; set; }
        public uint OtherPortalId { get; set; }

        // List of cells used in this structure. (Or possibly just those visible through it.)
        public List<uint> StabList { get; set; } = new List<uint>();
    }
}
