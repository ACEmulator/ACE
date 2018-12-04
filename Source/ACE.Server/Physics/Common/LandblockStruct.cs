using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;

using ACE.Entity.Enum;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Server.Physics.Entity;

namespace ACE.Server.Physics.Common
{
    public class LandblockStruct
    {
        public uint ID;
        //public Vector3 VertexLighting;    // RGBColor
        public LandDefs.Direction TransDir;
        public int SideVertexCount;
        public int SidePolyCount;
        public int SideCellCount;
        public LandDefs.WaterType WaterType;
        public List<byte> Height;
        public List<ushort> Terrain;
        public VertexArray VertexArray;
        public List<Polygon> Polygons;
        //public List<int> SurfaceStrips;     // SurfaceTriStrips
        //public int BlockSurfaceIndex;
        public readonly object LandCellMutex = new object();
        public ConcurrentDictionary<int, ObjCell> LandCells;
        public List<bool> SWtoNEcut { get; set; }

        // client-only
        public static List<Vector2> LandUVs;
        public static Dictionary<byte, List<Vector2>> LandUVsRotated;

        public static List<ushort> SurfChar;

        public static List<byte> SW_Corner;
        public static List<byte> SE_Corner;
        public static List<byte> NE_Corner;
        public static List<byte> NW_Corner;

        public static LandSurf LandSurf;

        static LandblockStruct()
        {
            LandUVs = new List<Vector2>(4);
            LandUVs.AddRange(new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
                new Vector2(0, 0)
            });

            LandUVsRotated = new Dictionary<byte, List<Vector2>>();
            LandUVsRotated.Add(0, new List<Vector2>() { LandUVs[0], LandUVs[1], LandUVs[2], LandUVs[3] });
            LandUVsRotated.Add(1, new List<Vector2>() { LandUVs[3], LandUVs[0], LandUVs[1], LandUVs[2] });
            LandUVsRotated.Add(2, new List<Vector2>() { LandUVs[2], LandUVs[3], LandUVs[0], LandUVs[1] });
            LandUVsRotated.Add(3, new List<Vector2>() { LandUVs[1], LandUVs[2], LandUVs[3], LandUVs[0] });

            SurfChar = new List<ushort>()
            {
                0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0,
                1, 1, 1, 1, 1, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0
            };

            SW_Corner = new List<byte>() { 0, 3, 2, 1 };
            SE_Corner = new List<byte>() { 1, 0, 3, 2 };
            NE_Corner = new List<byte>() { 2, 1, 0, 3 };
            NW_Corner = new List<byte>() { 3, 2, 1, 0 };
        }

        public LandblockStruct()
        {
            Init();
        }

        public LandblockStruct(CellLandblock landblock)
        {
            Init();
            Height = landblock.Height;
            Terrain = landblock.Terrain;
            // originally called from LScape.update_block()
            Generate(landblock.Id, 1, LandDefs.Direction.Unknown);
        }

        public Polygon AddPolygon(int polyIdx, int _v0, int _v1, int _v2)
        {
            var polygon = Polygons[polyIdx];
            var v0 = (short)_v0;
            var v1 = (short)_v1;
            var v2 = (short)_v2;

            polygon.Vertices[0] = VertexArray.Vertices[v0];
            polygon.Vertices[1] = VertexArray.Vertices[v1];
            polygon.Vertices[2] = VertexArray.Vertices[v2];

            polygon.VertexIDs[0] = v0;
            polygon.VertexIDs[1] = v1;
            polygon.VertexIDs[2] = v2;

            polygon.make_plane();

            polygon.PosSurface = 0;

            if (polygon.Vertices[0].Origin.Z != 0 || polygon.Vertices[1].Origin.Z != 0 || polygon.Vertices[2].Origin.Z != 0)
                polygon.PosSurface = 1;

            return polygon;
        }

        public void AdjustPlanes()
        {
            for (var x = 0; x < SidePolyCount; x++)
            {
                for (var y = 0; y < SidePolyCount; y++)
                {
                    for (var polyIdx = 0; polyIdx < 2; polyIdx++)
                        Polygons[(x * SidePolyCount + y) * 2 + polyIdx].make_plane();
                }
            }
        }

        public void CalcCellWater(int x, int y, out bool cellHasWater, out bool cellFullyFlooded)
        {
            cellHasWater = false;
            cellFullyFlooded = true;

            for (var vx = x * LandDefs.VertexPerCell; vx <= LandDefs.VertexPerCell * (x + 1); vx++)
            {
                for (var vy = y * LandDefs.VertexPerCell; vy <= LandDefs.VertexPerCell * (y + 1); vy++)
                {
                    var terrainIdx = vx * SideVertexCount + vy;
                    if (SurfChar[Terrain[terrainIdx] >> 2 & 0x1F] == 1)
                        cellHasWater = true;
                    else
                        cellFullyFlooded = false;
                }
            }
        }

        public void CalcWater()
        {
            var hasWater = false;
            var waterblock = true;

            if (SideCellCount == LandDefs.BlockSide)
            {
                for (var x = 0; x < SideCellCount; x++)
                {
                    for (var y = 0; y < SideCellCount; y++)
                    {
                        bool cellHasWater, cellFullyFlooded;
                        CalcCellWater(x, y, out cellHasWater, out cellFullyFlooded);

                        if (cellHasWater)
                        {
                            hasWater = true;

                            if (cellFullyFlooded)
                                LandCells[x * SideCellCount + y].WaterType = LandDefs.WaterType.EntirelyWater;
                            else
                            {
                                LandCells[x * SideCellCount + y].WaterType = LandDefs.WaterType.PartiallyWater;
                                waterblock = false;
                            }
                        }
                        else
                        {
                            LandCells[x * SideCellCount + y].WaterType = LandDefs.WaterType.NotWater;
                            waterblock = false;
                        }
                    }
                }
            }
            WaterType = hasWater ? waterblock ? LandDefs.WaterType.EntirelyWater : LandDefs.WaterType.PartiallyWater : LandDefs.WaterType.NotWater;
        }

        public void ConstructPolygons(uint landblockID)
        {
            var lcoord = LandDefs.blockid_to_lcoord(landblockID).Value;

            var polyNum = 1;
            var seedA = (uint)((int)lcoord.X * LandDefs.VertexPerCell * 214614067);
            var seedB = (uint)((int)lcoord.X * LandDefs.VertexPerCell * 1109124029);
            var vertexCnt = 0;

            for (var x = 0; x < SideCellCount; x++)
            {
                var lcoordX = lcoord.X + x;
                var originY = 1;
                var colVertexCnt = 0;
                var originX = polyNum;

                for (var y = 0; y < SideCellCount; y++)
                {
                    var lcoordY = (int)lcoord.Y + y;
                    var magicB = seedB;
                    var magicA = seedA + 1813693831;

                    for (var i = 0; i < LandDefs.VertexPerCell; i++)
                    {
                        var idxI = vertexCnt + i;
                        var idxJ = colVertexCnt;

                        for (var j = 0; j < LandDefs.VertexPerCell; j++)
                        {
                            var splitDir = (uint)(((int)lcoord.Y * LandDefs.VertexPerCell + idxJ) * magicA - magicB - 1369149221);
                            var polyIdx = (LandDefs.VertexPerCell * i + j) * 2;

                            var cellIdx = x * SideCellCount + y;
                            var vertIdx = idxI * SideVertexCount + idxJ;
                            var firstVertex = (idxI * SidePolyCount + idxJ) * 2;
                            var nextVertIdx = (idxI + 1) * SideVertexCount + idxJ;

                            var lcell = (LandCell)LandCells[cellIdx];

                            if (splitDir * 2.3283064e-10 < 0.5f)
                            {
                                 //  2    1---0
                                 //  | \   \  |
                                 //  |  \   \ |
                                 //  0---1    2

                                SWtoNEcut[cellIdx] = false;

                                lcell.Polygons[polyIdx] = AddPolygon(firstVertex, vertIdx, nextVertIdx, vertIdx + 1);
                                lcell.Polygons[polyIdx + 1] = AddPolygon(firstVertex + 1, nextVertIdx + 1, vertIdx + 1, nextVertIdx);
                            }
                            else
                            {
                                //     2   2---1
                                //    / |  |  /
                                //   /  |  | /
                                //  0---1  0

                                SWtoNEcut[cellIdx] = true;

                                lcell.Polygons[polyIdx] = AddPolygon(firstVertex, vertIdx, nextVertIdx, nextVertIdx + 1);
                                lcell.Polygons[polyIdx + 1] = AddPolygon(firstVertex + 1, vertIdx, nextVertIdx + 1, vertIdx + 1);
                            }
                            idxJ++;
                        }
                        magicA += 214614067;
                        magicB += 1109124029;
                    }

                    var cellID = (uint)LandDefs.lcoord_to_gid(lcoordX, lcoordY);
                    var landCell = LandCells[x * SideCellCount + y];
                    landCell.ID = cellID;
                    landCell.Pos.ObjCellID = cellID;
                    landCell.Pos.Frame.Origin.X = originX * LandDefs.HalfSquareLength;
                    landCell.Pos.Frame.Origin.Y = originY * LandDefs.HalfSquareLength;

                    originY += 2;
                    colVertexCnt += LandDefs.VertexPerCell;
                }
                vertexCnt += LandDefs.VertexPerCell;
                seedB += 1109124029 * (uint)LandDefs.VertexPerCell;
                seedA += 214614067 * (uint)LandDefs.VertexPerCell;
                polyNum += 2;
            }
        }

        public void ConstructUVs(uint landblockID)
        {
            for (uint x = 0; x < SidePolyCount; x++)
            {
                for (uint y = 0; y < SidePolyCount; y++)
                {
                    bool singleTextureCell = false;
                    uint surfNum = 0;
                    var rotation = LandDefs.Rotation.Rot0;
                    GetCellRotation(landblockID, x, y, ref singleTextureCell, ref surfNum, ref rotation);

                    var idx = (int)(2 * (y + x * SidePolyCount));
                    if (singleTextureCell)
                    {
                        Polygons[idx].Stippling = StipplingType.Both;
                        Polygons[idx + 1].Stippling = StipplingType.Both;
                    }
                    var vType = VertexArray.Type;

                    if (SWtoNEcut[idx / 2])
                    {
                        if (vType == VertexType.CSWVertexType)
                        {
                            Polygons[idx].PosUVIndices = new List<byte>();
                            Polygons[idx].PosUVIndices.Add(SW_Corner[(int)rotation]);
                            Polygons[idx].PosUVIndices.Add(SE_Corner[(int)rotation]);
                            Polygons[idx].PosUVIndices.Add(NE_Corner[(int)rotation]);

                            Polygons[idx + 1].PosUVIndices = new List<byte>();
                            Polygons[idx + 1].PosUVIndices.Add(SW_Corner[(int)rotation]);
                            Polygons[idx + 1].PosUVIndices.Add(NE_Corner[(int)rotation]);
                            Polygons[idx + 1].PosUVIndices.Add(NW_Corner[(int)rotation]);
                        }
                    } else
                    {
                        if (vType == VertexType.CSWVertexType)
                        {
                            Polygons[idx].PosUVIndices = new List<byte>();
                            Polygons[idx].PosUVIndices.Add(SW_Corner[(int)rotation]);
                            Polygons[idx].PosUVIndices.Add(SE_Corner[(int)rotation]);
                            Polygons[idx].PosUVIndices.Add(NW_Corner[(int)rotation]);

                            Polygons[idx + 1].PosUVIndices = new List<byte>();
                            Polygons[idx + 1].PosUVIndices.Add(NE_Corner[(int)rotation]);
                            Polygons[idx + 1].PosUVIndices.Add(NW_Corner[(int)rotation]);
                            Polygons[idx + 1].PosUVIndices.Add(SE_Corner[(int)rotation]);
                        }
                    }
                    Polygons[idx].PosSurface = (short)surfNum;
                    Polygons[idx + 1].PosSurface = (short)surfNum;
                }
            }
        }

        public void ConstructVertices()
        {
            var cellScale = LandDefs.BlockSide / SideCellCount;
            var cellSize = LandDefs.BlockLength / SidePolyCount;

            for (var x = 0; x < SideVertexCount; x++)
            {
                var cellX = x * cellSize;

                for (var y = 0; y < SideVertexCount; y++)
                {
                    var cellY = y * cellSize;
                    var vertex_idx = x * SideVertexCount + y;

                    var vertex = VertexArray.Vertices[vertex_idx];
                    var zHeight = LandDefs.LandHeightTable[Height[vertex_idx]];

                    vertex.Origin = new Vector3(cellX, cellY, zHeight * cellScale);
                }
            }
        }

        public void Destroy()
        {
            // omitted delete UVs

            if (LandCells != null)
            {
                RemoveSurfaces();
                LandCells = null;
            }
            VertexArray.Vertices = null;
            Polygons = null;
            SWtoNEcut = null;
            //SurfaceStrips = null;

            // omitted vertex lighting
        }

        public bool FSplitNESW(int x, int y)
        {
            var split = x * y * 0xCCAC033 - x * 0x421BE3BD + (y * 0x6C1AC587 - 0x519B8F25) / 2147483648;
            return split % 2 != 0;
        }

        public bool Generate(uint landblockID, int cellScale, LandDefs.Direction transAdj)
        {
            var cellWidth = LandDefs.BlockSide / cellScale;

            if (cellWidth == SideCellCount && TransDir == transAdj)
                return false;

            var cellRegen = false;

            if (cellWidth != SideCellCount)
            {
                cellRegen = true;

                if (SideCellCount > 0)
                    Destroy();

                SideCellCount = cellWidth;

                SideVertexCount = SideCellCount * LandDefs.VertexPerCell + 1;
                SidePolyCount = SideCellCount * LandDefs.VertexPerCell;

                InitPVArrays();
            }

            TransDir = transAdj;
            ConstructVertices();

            if (TransDir != LandDefs.Direction.Inside && SideCellCount > 1 && SideCellCount < LandDefs.BlockSide)
                TransAdjust();

            if (!cellRegen)
                AdjustPlanes();
            else
            {
                ConstructPolygons(landblockID);

                if (!PhysicsEngine.Instance.Server)
                    ConstructUVs(landblockID);      // client mode only
            }

            CalcWater();

            FinalizePVArrays();

            return cellRegen;
        }

        public void GetCellRotation(uint landblockID, uint x, uint y, ref bool singleTextureCell, ref uint surfNum, ref LandDefs.Rotation rotation)
        {
            var lcoord = LandDefs.blockid_to_lcoord(landblockID);

            var globalCellX = (int)(lcoord.Value.X + x);
            var globalCellY = (int)(lcoord.Value.Y + y);

            // no palette shift

            // SW / SE / NE / NW
            var i = (int)(LandDefs.VertexDim * x + y);
            var terrain = Terrain[i];
            var t1 = (terrain & 0x7F) >> 2;
            var r1 = terrain & 3;
            var j = (int)(LandDefs.VertexDim * (x + 1) + y);
            var terrain2 = Terrain[j];
            var t2 = (terrain2 & 0x7F) >> 2;
            var r2 = terrain2 & 3;
            var terrain3 = Terrain[j + 1];
            var t3 = (terrain3 & 0x7F) >> 2;
            var r3 = terrain3 & 3;
            var terrain4 = Terrain[i + 1];
            var t4 = (terrain4 & 0x7F) >> 2;
            var r4 = terrain4 & 3;

            /*Console.WriteLine($"LandblockStruct.GetCellRotation({landblockID:X8}, x:{x}, y:{y})");
            Console.WriteLine($"I1: {i}, I2: {j}, I3: {j+1}, I4: {i+1}");
            if (r1 != 0 || r2 != 0 || r3 != 0 || r4 != 0)
                Console.WriteLine($"R1: {r1}, R2: {r2}, R3: {r3}, R4: {r4}");
            Console.WriteLine($"T1: {(LandDefs.TerrainType)t1}, T2: {(LandDefs.TerrainType)t2}, T3: {(LandDefs.TerrainType)t3}, T4: {(LandDefs.TerrainType)t4}");*/

            var palCodes = new List<uint>();

            palCodes.Add(GetPalCode(r1, r2, r3, r4, t1, t2, t3, t4));   // 0
            //palCodes.Add(GetPalCode(r2, r3, r4, r1, t2, t3, t4, t1));   // 270
            //palCodes.Add(GetPalCode(r3, r4, r1, r2, t3, t4, t1, t2));   // 180
            //palCodes.Add(GetPalCode(r4, r1, r2, r3, t4, t1, t2, t3));   // 90

            var singleRoadCell = r1 == r2 && r1 == r3 && r1 == r4;
            var singleTypeCell = t1 == t2 && t1 == t3 && t1 == t4;

            singleTextureCell = r1 != 0 ? singleRoadCell : singleRoadCell && singleTypeCell;

            var regionDesc = DatManager.PortalDat.RegionDesc;
            var minimizePal = true;

            LandSurf.Instance.SelectTerrain(globalCellX, globalCellY, ref surfNum, ref rotation, palCodes, 1, minimizePal);
        }

        public static uint GetPalCode(int r1, int r2, int r3, int r4, int t1, int t2, int t3, int t4)
        {
            var terrainBits = t1 << 15 | t2 << 10 | t3 << 5 | t4;
            var roadBits = r1 << 26 | r2 << 24 | r3 << 22 | r4 << 20;

            // tex_size = 1 or 4, only used for palette shift (unused?)
            var sizeBits = 1 << 28;

            return (uint)(sizeBits | roadBits | terrainBits);

            //return (uint)(sizeBits + t4 + 32 * (t3 + 32 * (t2 + 32 * (t1 + 32 * (r4 + 4 * (r3 + 4 * (r2 + 4 * r1)))))));
        }

        public void Init()
        {
            TransDir = LandDefs.Direction.Unknown;
            WaterType = LandDefs.WaterType.NotWater;
            //BlockSurfaceIndex = -1;

            // init for landcell
            LandCells = new ConcurrentDictionary<int, ObjCell>();
            for (uint i = 1; i <= 64; i++) LandCells.TryAdd((int)i, new LandCell((i)));
        }

        /// <summary>
        /// Initialize arrays for vertices and polygons
        /// </summary>
        public void InitPVArrays()
        {
            var numSquares = SidePolyCount * SidePolyCount;
            var numVerts = SideVertexCount * SideVertexCount;
            var numCells = SideCellCount * SideCellCount;

            VertexArray = new VertexArray(VertexType.CSWVertexType, numVerts);
            for (var i = 0; i < numVerts; i++)
                VertexArray.Vertices.Add(new Vertex((ushort)i));

            var numPolys = numSquares * 2;
            Polygons = new List<Polygon>(numPolys);
            for (var i = 0; i < numPolys; i++)
                Polygons.Add(new Polygon(i, 3, CullMode.Landblock));

            SWtoNEcut = new List<bool>(numSquares);
            for (var i = 0; i < numSquares; i++)
                SWtoNEcut.Add(false);

            LandCells = new ConcurrentDictionary<int, ObjCell>(1, numCells);
            for (uint i = 0; i < numCells; i++)
                LandCells.TryAdd((int)i, new LandCell((ID & LandDefs.BlockMask) + i));
        }

        /// <summary>
        /// Finalize arrays for vertices and polygons
        /// by linking to cached versions
        /// </summary>
        public void FinalizePVArrays()
        {
            if (VertexCache.Enabled)
            {
                for (var i = 0; i < VertexArray.Vertices.Count; i++)
                    VertexArray.Vertices[i] = VertexCache.Get(VertexArray.Vertices[i]);
            }

            if (PolygonCache.Enabled)
            {
                for (var i = 0; i < Polygons.Count; i++)
                    Polygons[i] = PolygonCache.Get(Polygons[i]);
            }
        }

        public void RemoveSurfaces()
        {
            // calls LandSurf::RemoveSurface()
            // possibly only for rendering textures?
        }

        public void TransAdjust()
        {
            // refactor me..
            if (TransDir == LandDefs.Direction.North || TransDir == LandDefs.Direction.NorthWest || TransDir == LandDefs.Direction.NorthEast)
            {
                for (var i = 1; i < SidePolyCount; i += 2)
                {
                    var v0 = VertexArray.Vertices[((i - 1) * SideVertexCount) + SidePolyCount];
                    var v1 = VertexArray.Vertices[((i + 1) * SideVertexCount) + SidePolyCount];
                    var v2 = VertexArray.Vertices[(i * SideVertexCount) + SidePolyCount];

                    v2.Origin.Z = (v0.Origin.Z + v1.Origin.Z) / 2;
                }
            }
            if (TransDir == LandDefs.Direction.West || TransDir == LandDefs.Direction.NorthWest || TransDir == LandDefs.Direction.SouthWest)
            {
                for (var i = 1; i < SidePolyCount; i += 2)
                {
                    var v0 = VertexArray.Vertices[i - 1];
                    var v1 = VertexArray.Vertices[i + 1];
                    var v2 = VertexArray.Vertices[i];

                    v2.Origin.Z = (v0.Origin.Z + v1.Origin.Z) / 2;
                }
            }
            if (TransDir == LandDefs.Direction.South || TransDir == LandDefs.Direction.SouthWest || TransDir == LandDefs.Direction.SouthEast)
            {
                for (var i = 1; i < SidePolyCount; i += 2)
                {
                    var v0 = VertexArray.Vertices[(i - 1) * SideVertexCount];
                    var v1 = VertexArray.Vertices[(i + 1) * SideVertexCount];
                    var v2 = VertexArray.Vertices[i * SideVertexCount];

                    v2.Origin.Z = (v0.Origin.Z + v1.Origin.Z) / 2;
                }
            }
            if (TransDir == LandDefs.Direction.East || TransDir == LandDefs.Direction.NorthEast || TransDir == LandDefs.Direction.SouthEast)
            {
                for (var i = 1; i < SidePolyCount; i += 2)
                {
                    var v0 = VertexArray.Vertices[SideVertexCount * SidePolyCount + i - 1];
                    var v1 = VertexArray.Vertices[SideVertexCount * SidePolyCount + i + 1];
                    var v2 = VertexArray.Vertices[SideVertexCount * SidePolyCount + i];

                    v2.Origin.Z = (v0.Origin.Z + v1.Origin.Z) / 2;
                }
            }

            if (SideCellCount != LandDefs.BlockSide / 2)
                return;

            if (TransDir == LandDefs.Direction.North)
            {
                for (int i = 1, j = 4; i < SidePolyCount; i += 2, j += 4)
                {
                    var vertex = VertexArray.Vertices[i * SideVertexCount];

                    var height0 = LandDefs.LandHeightTable[Height[(j - 1) * SideVertexCount]];
                    var height1 = LandDefs.LandHeightTable[Height[j * SideVertexCount]];
                    var height2 = LandDefs.LandHeightTable[Height[(j - 3) * SideVertexCount]];
                    var height3 = LandDefs.LandHeightTable[Height[(j - 4) * SideVertexCount]];

                    vertex.Origin.Z = Math.Min(vertex.Origin.Z, height2 * 2 - height3);
                    vertex.Origin.Z = Math.Min(vertex.Origin.Z, height0 * 2 - height1);
                }
            }

            if (TransDir == LandDefs.Direction.South)
            {
                for (int i = 1, j = 4; i < SidePolyCount; i += 2, j += 4)
                {
                    var vertex = VertexArray.Vertices[i * SideVertexCount + SidePolyCount];

                    var height0 = LandDefs.LandHeightTable[Height[(j - 1) * SideVertexCount + SideVertexCount - 1]];
                    var height1 = LandDefs.LandHeightTable[Height[j * SideVertexCount + SideVertexCount - 1]];
                    var height2 = LandDefs.LandHeightTable[Height[(j - 3) * SideVertexCount + SideVertexCount - 1]];
                    var height3 = LandDefs.LandHeightTable[Height[(j - 4) * SideVertexCount + SideVertexCount - 1]];

                    vertex.Origin.Z = Math.Min(vertex.Origin.Z, height2 * 2 - height3);
                    vertex.Origin.Z = Math.Min(vertex.Origin.Z, height0 * 2 - height1);
                }
            }

            if (TransDir == LandDefs.Direction.East)
            {
                for (int i = 1; i < SidePolyCount; i += 2)
                {
                    var vertex = VertexArray.Vertices[i];

                    var height0 = LandDefs.LandHeightTable[Height[i * 2 + 1]];
                    var height1 = LandDefs.LandHeightTable[Height[i * 2 + 2]];
                    var height2 = LandDefs.LandHeightTable[Height[i * 2 - 1]];
                    var height3 = LandDefs.LandHeightTable[Height[i * 2 - 2]];

                    vertex.Origin.Z = Math.Min(vertex.Origin.Z, height2 * 2 - height3);
                    vertex.Origin.Z = Math.Min(vertex.Origin.Z, height0 * 2 - height1);
                }
            }

            if (TransDir == LandDefs.Direction.West)
            {
                for (int i = 1; i < SidePolyCount; i += 2)
                {
                    var vertex = VertexArray.Vertices[i + SideVertexCount * SideVertexCount];

                    var height0 = LandDefs.LandHeightTable[Height[SideVertexCount * (SideVertexCount - 1) + i * 2 + 1]];
                    var height1 = LandDefs.LandHeightTable[Height[SideVertexCount * (SideVertexCount - 1) + i * 2 + 2]];
                    var height2 = LandDefs.LandHeightTable[Height[SideVertexCount * (SideVertexCount - 1) + i * 2 - 1]];
                    var height3 = LandDefs.LandHeightTable[Height[SideVertexCount * (SideVertexCount - 1) + i * 2 - 2]];

                    vertex.Origin.Z = Math.Min(vertex.Origin.Z, height2 * 2 - height3);
                    vertex.Origin.Z = Math.Min(vertex.Origin.Z, height0 * 2 - height1);
                }
            }
        }

        public float calc_water_depth(uint blockCellID, Vector3 point)
        {
            var cellID = ((int)blockCellID & 0xFFFF) - 1;

            var cellX = cellID / 8;
            var cellY = cellID % 8;

            var terrainIdx = cellX * LandDefs.VertexDim + cellY;

            if (point.X % 24.0f >= 12.0f)
                terrainIdx += 9;

            if (point.Y % 24.0f >= 12.0f)
                terrainIdx++;

            var terrain = Terrain[terrainIdx];
            var surfCharIdx = terrain >> 2 & 0x1F;

            var has_water = SurfChar[surfCharIdx];

            if (has_water != 0)
                return 0.44999999f;
            else
                return 0.1f;
        }
    }
}
