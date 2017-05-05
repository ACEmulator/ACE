namespace ACE.DatLoader.Entity
{
    public class CellPortal
    {
        public ushort flags { get; set; }

        // Not exactly sure what these two are for, but they are from the flags
        public byte ExactMatch { get; set; }
        public byte PortalSide { get; set; }

        public ushort EnvironmentId { get; set; }
        public ushort OtherCellId { get; set; }
        public ushort OtherPortalId { get; set; }
    }
}
