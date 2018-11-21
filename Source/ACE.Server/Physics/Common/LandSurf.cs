using System.Collections.Generic;
using ACE.DatLoader;

namespace ACE.Server.Physics.Common
{
    public class LandSurf
    {
        public static LandSurf Instance;
        public static DatLoader.Entity.LandSurf _landSurf;

        public PalShift PalShift;
        public TexMerge TexMerge;
        public Dictionary<uint, SurfInfo> SurfInfo;
        public uint NumLSurf;
        public Dictionary<uint, Surface> LSurf;
        public uint NumUniqueSurfaces;
        public uint NumBlockSurfs;
        public List<Surface> BlockSurface;
        public byte CurrentTex;

        static LandSurf()
        {
            var regionDesc = DatManager.PortalDat.RegionDesc;
            var terrainDesc = regionDesc.TerrainInfo;
            Instance = new LandSurf(terrainDesc.LandSurfaces);
        }

        public LandSurf(DatLoader.Entity.LandSurf landSurf)
        {
            _landSurf = landSurf;

            TexMerge = new TexMerge(_landSurf.TexMerge);
            SurfInfo = new Dictionary<uint, SurfInfo>();
            LSurf = new Dictionary<uint, Surface>();
        }

        public bool SelectTerrain(int x, int y, ref uint surfNum, ref LandDefs.Rotation rot, List<uint> palCodes, int size, bool minimizePal)
        {
            var palIDs = new List<uint>();   // ??

            if (PalShift == null && TexMerge == null)
                return false;

            if (PalShift != null)
            {
                var rotIdx = (int)rot;
                var selectRotResult = PalShift.SelectRot(x, y, ref rot, palCodes, size, palIDs[0] /* fixme */, ref rotIdx /* verify */, minimizePal);
                if (SurfInfo.TryGetValue(selectRotResult, out var surfInfo))
                {
                    surfInfo.LCellCount++;
                    surfNum = surfInfo.SurfNum;
                    return true;
                }
                else
                {
                    // PalShift.MakeNewSurface()
                    // return AddNewSurface()
                    return false;
                }
            }

            if (TexMerge != null)
            {
                var palCode = palCodes[0];
                rot = LandDefs.Rotation.Rot0;

                if (SurfInfo.TryGetValue(palCode, out var surfInfo))
                {
                    surfInfo.LCellCount++;
                    surfNum = surfInfo.SurfNum;
                    return true;
                }
                else
                {
                    var surface = TexMerge.MakeNewSurface(palCode, size);
                    return AddNewSurface(surface, palCode, ref surfNum);
                }
            }

            return false;
        }

        public bool AddNewSurface(Surface surface, uint pcode, ref uint surfNum)
        {
            var surfInfo = new SurfInfo();  // 0x10, 16

            surfInfo.Surface = surface;
            surfInfo.PalCode = pcode;
            surfInfo.LCellCount++;

            surfNum = GetNextFree();
            surfInfo.SurfNum = surfNum;

            LSurf.Add(surfNum, surface);
            SurfInfo.Add(pcode, surfInfo);

            NumUniqueSurfaces++;

            return true;
        }

        public uint NextSurfNum;

        public uint GetNextFree()
        {
            return NextSurfNum++;
        }

        public Surface GetLandSurface(uint surf_id)
        {
            LSurf.TryGetValue(surf_id, out var surface);
            return surface;
        }
    }
}
