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
                return GetCellLandblock(qualifiedDID.ID);

            if (qualifiedDID.Type == 2)
                return GetLandblockInfo(qualifiedDID.ID);

            if (qualifiedDID.Type == 3)
                return GetEnvCell(qualifiedDID.ID);

            if (qualifiedDID.Type == 6)
                return GetGfxObj(qualifiedDID.ID);

            if (qualifiedDID.Type == 7)
                return GetSetup(qualifiedDID.ID);

            if (qualifiedDID.Type == 8)
                return GetAnimation(qualifiedDID.ID);

            if (qualifiedDID.Type == 11)
                return GetSurfaceTexture(qualifiedDID.ID);

            if (qualifiedDID.Type == 16)
                return GetEnvironment(qualifiedDID.ID);

            if (qualifiedDID.Type == 42)
                return GetParticleEmitterInfo(qualifiedDID.ID);

            return -1;
        }

        /// <summary>
        /// QualifiedDID Type 1
        /// </summary>
        public static CellLandblock GetCellLandblock(uint id)
        {
            return DatManager.CellDat.ReadFromDat<CellLandblock>(id);
        }

        /// <summary>
        /// QualifiedDID Type 2
        /// </summary>
        public static LandblockInfo GetLandblockInfo(uint id)
        {
            return DatManager.CellDat.ReadFromDat<LandblockInfo>(id);
        }

        /// <summary>
        /// QualifiedDID Type 3
        /// </summary>
        public static EnvCell GetEnvCell(uint id)
        {
            var envCell = DatManager.CellDat.ReadFromDat<DatLoader.FileTypes.EnvCell>(id);

            return new EnvCell(envCell);
        }

        /// <summary>
        /// QualifiedDID Type 6
        /// </summary>
        public static GfxObj GetGfxObj(uint id)
        {
            return DatManager.PortalDat.ReadFromDat<GfxObj>(id);
        }

        /// <summary>
        /// QualifiedDID Type 7
        /// </summary>
        public static SetupModel GetSetup(uint id)
        {
            return DatManager.PortalDat.ReadFromDat<SetupModel>(id);
        }

        /// <summary>
        /// QualifiedDID Type 8
        /// </summary>
        public static DatLoader.FileTypes.Animation GetAnimation(uint id)
        {
            return DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.Animation>(id);
        }

        /// <summary>
        /// QualifiedDID Type 11
        /// </summary>
        public static SurfaceTexture GetSurfaceTexture(uint id)
        {
            return DatManager.PortalDat.ReadFromDat<SurfaceTexture>(id);
        }

        /// <summary>
        /// QualifiedDID Type 16
        /// </summary>
        public static DatLoader.FileTypes.Environment GetEnvironment(uint id)
        {
            return DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.Environment>(id);
        }

        /// <summary>
        /// QualifiedDID Type 42
        /// </summary>
        public static DatLoader.FileTypes.ParticleEmitterInfo GetParticleEmitterInfo(uint id)
        {
            return DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.ParticleEmitterInfo>(id);
        }
    }
}
