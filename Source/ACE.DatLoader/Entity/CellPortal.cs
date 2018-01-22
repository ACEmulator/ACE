
namespace ACE.DatLoader.Entity
{
    public class CellPortal
    {
        public ushort Flags { get; set; }

        // Not exactly sure what these two are for, but they are from the flags
        public bool ExactMatch => (Flags & 1) != 0;
        public bool PortalSide => (Flags & 2) != 0;

        public ushort EnvironmentId { get; set; }
        public ushort OtherCellId { get; set; }
        public ushort OtherPortalId { get; set; }
    }
}
