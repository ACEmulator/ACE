using ACE.Entity.Enum;

namespace ACE.Server.Physics.Common
{
    public class BldPortal
    {
        public PortalFlags Flags;
        public bool ExactMatch;
        public bool PortalSide;
        public ushort OtherCellId;
        public ushort OtherPortalId;

        public BldPortal() { }

        public BldPortal(DatLoader.Entity.CBldPortal bldPortal)
        {
            Flags = bldPortal.Flags;
            ExactMatch = bldPortal.ExactMatch;
            PortalSide = bldPortal.PortalSide;
            OtherCellId = bldPortal.OtherCellId;
            OtherPortalId = bldPortal.OtherPortalId;
        }

        public EnvCell GetOtherCell(uint landblockID)
        {
            var blockCellID = landblockID & 0xFFFF0000 | OtherCellId;

            return (EnvCell)LScape.get_landcell(blockCellID);
        }

    }
}
