using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Common
{
    public class TexMerge
    {
        // mega thanks to parad0x for tremendous help with this class!

        public DatLoader.Entity.TexMerge _texMerge;

        public uint BaseTexSize;
        public List<TerrainAlphaMap> CornerTerrainMaps;
        public List<TerrainAlphaMap> SideTerrainMaps;
        public List<RoadAlphaMap> RoadMaps;
        public List<TMTerrainDesc> TerrainDesc;

        public static Dictionary<ImageScaleType, int> ImageShift;

        public static byte[] TexData;

        static TexMerge()
        {
            ImageShift = new Dictionary<ImageScaleType, int>();   // size 5?
            ImageShift.Add(ImageScaleType.Full, 0);     // guessed, verify
            ImageShift.Add(ImageScaleType.Half, 1);
            ImageShift.Add(ImageScaleType.Quarter, 2);
            ImageShift.Add(ImageScaleType.Eighth, 3);
        }

        public TexMerge(DatLoader.Entity.TexMerge texMerge)
        {
            _texMerge = texMerge;

            BaseTexSize = _texMerge.BaseTexSize;

            CornerTerrainMaps = new List<TerrainAlphaMap>();
            foreach (var cornerTerrainMap in _texMerge.CornerTerrainMaps)
                CornerTerrainMaps.Add(new TerrainAlphaMap(cornerTerrainMap));

            SideTerrainMaps = new List<TerrainAlphaMap>();
            foreach (var sideTerrainMap in _texMerge.SideTerrainMaps)
                SideTerrainMaps.Add(new TerrainAlphaMap(sideTerrainMap));

            RoadMaps = new List<RoadAlphaMap>();
            foreach (var roadAlphaMap in _texMerge.RoadMaps)
                RoadMaps.Add(new RoadAlphaMap(roadAlphaMap));

            TerrainDesc = new List<TMTerrainDesc>();
            foreach (var terrainDesc in _texMerge.TerrainDesc)
                TerrainDesc.Add(new TMTerrainDesc(terrainDesc));
        }

        public Surface MakeNewSurface(uint pCode, int size)
        {
            var texSize = BaseTexSize >> ImageShift[ImgTex.LandTextureScale] / size;
            if (texSize < ImgTex.MinTexSize)
                texSize = ImgTex.MinTexSize;

            var surface = Surface.MakeCustomSurface(SurfaceHandler.TexMerge);
            if (surface != null)
            {
                // vfptr
                var texInfo = BuildTexture(pCode, texSize);
                texInfo.PostInit();

                if (texInfo != null)
                {
                    //var landscapeTex = ImgTex.CreateLScapeTexture(TexData, texSize, texSize);
                    //if (landscapeTex != null)
                        //surface.UseTextureMap(landscapeTex, SurfaceHandler.TexMerge);

                    surface.Info = texInfo;
                }
            }
            return surface;
        }

        public TextureMergeInfo BuildTexture(uint pcode, uint texSize)
        {
            var terrainTex = GetTerrain(pcode, out var tcode);

            // possible tcode values:
            // 0,1,2,3,4,6,8
            //Console.WriteLine("tcodes: " + String.Join(",", tcode));

            bool all_road = false;
            var rcode = GetRoadCode(pcode, ref all_road);
            var roadTex = GetTerrainTex(LandDefs.TerrainType.RoadType);

            var result = new TextureMergeInfo();
            result.TCodes = tcode;

            if (all_road)
            {
                //CopyAndTile(TexData, texSize, roadTex);
                result.TerrainBase = roadTex;
                return result;
            }

            result.TerrainBase = terrainTex[0];

            //if (TexData == null)
                //TexData = new byte[4 * BaseTexSize * BaseTexSize];

            for (var i = 0; i < 3; i++)
            {
                if (tcode[i] == 0) break;

                var terrainAlpha = FindTerrainAlpha(pcode, tcode[i], out var rot, out var alphaIdx);
                if (terrainAlpha == null) return null;

                //Console.WriteLine($"FindTerrainAlpha({tcode[i]}, {rot}, {alphaIdx})");

                result.TerrainOverlays[i] = terrainTex[i + 1];
                result.TerrainRotations[i] = rot;
                result.TerrainAlphaOverlays[i] = terrainAlpha;
                result.TerrainAlphaIndices[i] = alphaIdx;

                //Merge(TexData, texSize, terrainAlpha.Texture, rot, terrainTex[i]);  // * 4 / + 1?
            }

            if (roadTex != null)
            {
                for (var i = 0; i < 2; i++)
                {
                    if (rcode[i] == 0) break;

                    var roadAlpha = FindRoadAlpha(pcode, rcode[i], out var rot, out var alphaIdx);

                    result.RoadRotations[i] = rot;
                    result.RoadAlphaIndices[i] = alphaIdx;
                    result.RoadAlphaOverlays[i] = roadAlpha;
                    result.RoadOverlay = roadTex;

                    //Merge(TexData, texSize, roadAlpha.Texture, rot, roadTex);
                }
            }
            return result;
        }

        public TerrainTex GetTerrainTex(LandDefs.TerrainType t1)
        {
            var numTextures = TerrainDesc.Count;
            if (numTextures == 0)
                return null;

            for (var i = 0; i < numTextures; i++)
            {
                var terrainDesc = TerrainDesc[i];
                if (t1 == terrainDesc.TerrainType)
                    return terrainDesc.TerrainTex;
            }
            return TerrainDesc[0].TerrainTex;
        }

        public List<LandDefs.TerrainType> GetTerrainCodes(uint pcode)
        {
            var pcodes = new List<LandDefs.TerrainType>();

            pcodes.Add((LandDefs.TerrainType)((pcode >> 15) & 0x1F));
            pcodes.Add((LandDefs.TerrainType)((pcode >> 10) & 0x1F));
            pcodes.Add((LandDefs.TerrainType)((pcode >> 5) & 0x1F));
            pcodes.Add((LandDefs.TerrainType)(pcode & 0x1F));

            return pcodes;
        }

        public List<TerrainTex> GetTerrain(uint pcode, out List<uint> tcode)
        {
            tcode = Enumerable.Repeat(0u, 3).ToList();

            var pcodes = GetTerrainCodes(pcode);

            for (var i = 0; i < 4; i++)
            {
                for (var j = i + 1; j < 4; j++)
                {
                    if (pcodes[i] == pcodes[j])
                        return BuildTCodes(pcodes, tcode, i);
                }
            }
            var terrainTex = new List<TerrainTex>(4);

            for (var i = 0; i < 4; i++)
                terrainTex.Add(GetTerrainTex(pcodes[i]));

            // tcodes: 2, 4, 8
            for (var i = 0; i < 3; i++)
                tcode[i] = (uint)(1 << (i + 1));

            return terrainTex;
        }

        public List<TerrainTex> BuildTCodes(List<LandDefs.TerrainType> pcodes, List<uint> tcode, int i)
        {
            // some output combinations:
            // 1,0,0
            // 2,0,0
            // 4,0,0
            // 6,0,0
            // 8,0,0
            // 12,0,0
            // 2,8,0

            var terrainTex = Enumerable.Repeat(new TerrainTex(), 3).ToList();

            LandDefs.TerrainType t1 = pcodes[i];
            LandDefs.TerrainType t2 = 0;

            terrainTex[0] = GetTerrainTex(t1);

            for (var k = 0; k < 4; k++)
            {
                if (t1 == pcodes[k])
                    continue;

                if (tcode[0] == 0)
                {
                    tcode[0] = (uint)(1 << k);
                    t2 = pcodes[k];

                    terrainTex[1] = GetTerrainTex(t2);
                }
                else
                {
                    if (t2 == pcodes[k] && tcode[0] == (1 << (k - 1)))
                    {
                        tcode[0] += (uint)(1 << k);
                    }
                    else
                    {
                        terrainTex[2] = GetTerrainTex(pcodes[k]);
                        tcode[1] = (uint)(1 << k);
                    }
                    break;
                }
            }
            return terrainTex;
        }

        public List<uint> GetRoadCode(uint pcode, ref bool all_road)
        {
            var rcode = Enumerable.Repeat(0u, 2).ToList();

            uint mask = 0;
            if ((pcode & 0xC000000) != 0)   // upper left (0)
                mask = 1;
            if ((pcode & 0x3000000) != 0)   // upper right (1)
                mask |= 2;
            if ((pcode & 0xC00000) != 0)    // bottom right (2)
                mask |= 4;
            if ((pcode & 0x300000) != 0)    // bottom left (3)
                mask |= 8;

            all_road = false;
            rcode[0] = 0;
            rcode[1] = 0;

            switch (mask)
            {
                case 0xF:       // 0 + 1 + 2 + 3
                    all_road = true;
                    break;
                case 0xE:       // 1 + 2 + 3
                    rcode[0] = 6;
                    rcode[1] = 12;
                    break;
                case 0xD:       // 0 + 2 + 3
                    rcode[0] = 9;
                    rcode[1] = 12;
                    break;
                case 0xB:       // 0 + 1 + 3
                    rcode[0] = 9;
                    rcode[1] = 3;
                    break;
                case 0x7:       // 0 + 1 + 2
                    rcode[0] = 3;
                    rcode[1] = 6;
                    break;
                case 0x0:
                    break;
                default:        // 1-6, 8, 9, A, C       
                    rcode[0] = mask;
                    break;
            }
            return rcode;
        }

        public TerrainAlphaMap FindTerrainAlpha(uint pcode, uint tcode, out LandDefs.Rotation rot, out int alphaIdx)
        {
            List<TerrainAlphaMap> terrainMaps = null;
            var baseIdx = 0;

            // corner tcodes - sw / se / ne / nw
            if (tcode != 1 && tcode != 2 && tcode != 4 && tcode != 8)
            {
                baseIdx = 4;
                terrainMaps = SideTerrainMaps;   // common tcode: 9
            }
            else
                terrainMaps = CornerTerrainMaps; // common tcode: 8

            var numTerrains = terrainMaps.Count;

            var prng = (int)Math.Floor((1379576222 * pcode - 1372186442) * 2.3283064e-10 * numTerrains);
            if (prng >= numTerrains)
                prng = 0;

            var alpha = terrainMaps[prng];
            alphaIdx = baseIdx + prng;

            rot = LandDefs.Rotation.Rot0;

            var i = 0;
            var alphaCode = alpha.TCode;
            while (alphaCode != tcode)
            {
                // corners: 8 -> 1 -> 2 -> 4
                // sides: 9 -> 3 -> 6 -> 12
                // west / south / east / north?
                alphaCode *= 2;
                if (alphaCode >= 16)
                    alphaCode -= 15;
                if (++i >= 4)
                    return null;
            }
            rot = (LandDefs.Rotation)i;

            if (alpha.Texture == null && alpha.TexGID != 0)
            {
                var qdid = new QualifiedDataID(11, alpha.TexGID);
                alpha.Texture = new ImgTex(DBObj.GetSurfaceTexture(alpha.TexGID));
            }
            return alpha;
        }

        public RoadAlphaMap FindRoadAlpha(uint pcode, uint rcode, out LandDefs.Rotation rot, out int alphaIdx)
        {
            rot = LandDefs.Rotation.Rot0;
            alphaIdx = -1;

            var numRoadMaps = RoadMaps.Count;
            if (numRoadMaps == 0)
                return null;

            var prng = (int)Math.Floor((1379576222 * pcode - 1372186442) * 2.3283064e-10 * numRoadMaps);

            for (var i = 0; i < numRoadMaps; i++)
            {
                var idx = (i + prng) % numRoadMaps;
                var alpha = RoadMaps[idx];
                rot = LandDefs.Rotation.Rot0;
                var alphaCode = alpha.RCode;
                alphaIdx = 5 + idx;

                for (var j = 0; j < 4; j++)
                {
                    if (alphaCode == rcode)
                    {
                        rot = (LandDefs.Rotation)j;
                        if (alpha.Texture == null && alpha.RoadTexGID != 0)
                            alpha.Texture = new ImgTex(DBObj.GetSurfaceTexture(alpha.RoadTexGID));

                        return alpha;
                    }
                    alphaCode *= 2;
                    if (alphaCode >= 16)
                        alphaCode -= 15;
                }
            }
            alphaIdx = -1;
            return null;
        }

        public bool CopyAndTile(byte[] data, uint texSize, TerrainTex terrainTex)
        {
            var baseTexture = terrainTex.BaseTexture;

            if (baseTexture != null)
            {
                ImgTex.CopyCSI(data, texSize, texSize, baseTexture, terrainTex.TexTiling);
                return true;
            }

            if (terrainTex.TexGID != 0) // stru_841760
            {
                var qdid = new QualifiedDataID(11, terrainTex.TexGID);

                var surfaceTexture = DBObj.GetSurfaceTexture(terrainTex.TexGID);
                terrainTex.BaseTexture = new ImgTex(surfaceTexture);
            }
            baseTexture = terrainTex.BaseTexture;
            
            if (baseTexture != null)
            {
                ImgTex.CopyCSI(data, texSize, texSize, baseTexture, terrainTex.TexTiling);
                return true;
            }
            else
            {
                ImgTex.CopyCSI(data, texSize, texSize, null, 1);
                return false;
            }
        }

        public static bool Merge(byte[] data, uint texSize, ImgTex texture, LandDefs.Rotation rot, TerrainTex terrainTex)
        {
            if (texture == null)
                return false;

            var baseTexture = terrainTex.BaseTexture;
            if (baseTexture != null)
            {
                ImgTex.MergeTexture(data, texSize, texSize, baseTexture, terrainTex.TexTiling, texture, rot);
                return true;
            }
            else if (terrainTex.InitEnd())
            {
                ImgTex.MergeTexture(data, texSize, texSize, terrainTex.BaseTexture, terrainTex.TexTiling, texture, rot);
                return true;
            }
            return false;
        }
    }
}
