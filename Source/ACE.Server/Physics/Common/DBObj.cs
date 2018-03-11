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
            if (qualifiedDID.Type == 6)
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
