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
            if (qualifiedDID.Type == 2)
            {
                var landblockInfo = DatManager.CellDat.ReadFromDat<LandblockInfo>(qualifiedDID.ID);
                return landblockInfo;
            }
            if (qualifiedDID.Type == 3)
            {
                var envCell = DatManager.CellDat.ReadFromDat<DatLoader.FileTypes.EnvCell>(qualifiedDID.ID);
                return new EnvCell(envCell);
            }
            if (qualifiedDID.Type == 6)
            {
                var gfxObj = DatManager.PortalDat.ReadFromDat<GfxObj>(qualifiedDID.ID);
                return gfxObj;
            }
            if (qualifiedDID.Type == 7)
            {
                var setupModel = DatManager.PortalDat.ReadFromDat<SetupModel>(qualifiedDID.ID);
                return new Setup(setupModel);
            }
            if (qualifiedDID.Type == 8)
            {
                var animation = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.Animation>(qualifiedDID.ID);
                return animation;
            }
            if (qualifiedDID.Type == 16)
            {
                var environment = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.Environment>(qualifiedDID.ID);
                return environment;
            }
            return -1;
        }

        /// <summary>
        /// QualifiedDID Type 6
        /// </summary>
        public static GfxObj GetGfxObj(uint id)
        {
            return DatManager.PortalDat.ReadFromDat<GfxObj>(id);
        }
    }
}
