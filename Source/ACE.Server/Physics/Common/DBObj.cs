using System;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;

namespace ACE.Server.Physics.Common
{
    public class DBObj
    {
        public static Object Get(QualifiedDataID qualifiedDID)
        {
            // TODO: map to ACE datloaders
            // return static or mutable?
            if (qualifiedDID.Type == 1)
            {
                var landblock = DatManager.CellDat.ReadFromDat<CellLandblock>(qualifiedDID.ID);
                return landblock;
            }
            else if (qualifiedDID.Type == 2)
            {
                var landblockInfo = DatManager.CellDat.ReadFromDat<LandblockInfo>(qualifiedDID.ID);
                return landblockInfo;
            }
            else if (qualifiedDID.Type == 3)
            {
                var envCell = DatManager.CellDat.ReadFromDat<DatLoader.FileTypes.EnvCell>(qualifiedDID.ID);
                return new EnvCell(envCell);
            }
            else if (qualifiedDID.Type == 6)
            {
                var gfxObj = DatManager.PortalDat.ReadFromDat<GfxObj>(qualifiedDID.ID);
                return gfxObj;
            }
            else if (qualifiedDID.Type == 7)
            {
                var setupModel = DatManager.PortalDat.ReadFromDat<SetupModel>(qualifiedDID.ID);
                return new Setup(setupModel);
            }
            else if (qualifiedDID.Type == 8)
            {
                var animation = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.Animation>(qualifiedDID.ID);
                return animation;
            }
            return -1;
        }
    }
}
