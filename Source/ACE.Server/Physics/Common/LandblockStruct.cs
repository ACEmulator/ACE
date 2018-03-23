using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader.FileTypes;

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
        public List<int> SurfaceStrips;     // SurfaceTriStrips
        public int BlockSurfaceIndex;
        public Dictionary<int, LandCell> LandCells;
        public List<bool> SWtoNEcut;

        public static List<VertexUV> LandUVs;
        public static List<ushort> SurfChar;

        static LandblockStruct()
        {
            LandUVs = new List<VertexUV>(4);
            LandUVs.AddRange(new List<VertexUV>()
            {
                new VertexUV(0, 1),
                new VertexUV(1, 1),
                new VertexUV(1, 0),
                new VertexUV(0, 0)
            });

            SurfChar = new List<ushort>()
            {
                0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0,
                1, 1, 1, 1, 1, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0
            };
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

            for (var vx = x * LandDefs.VertexPerCell; vx < LandDefs.VertexPerCell * (x + 1); vx++)
            {
                for (var vy = y * LandDefs.VertexPerCell; vy < LandDefs.VertexPerCell * (y + 1); vy++)
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
                            var firstVertex = idxI * SidePolyCount + idxJ;
                            var nextVertIdx = (idxI + 1) * SideVertexCount + idxJ;

                            if (splitDir * 2.3283064e-10 < 0.5f)
                            {
                                SWtoNEcut[polyIdx] = false;

                                LandCells[cellIdx].Polygons[polyIdx] = AddPolygon(firstVertex * 2, vertIdx, nextVertIdx, vertIdx + 1);
                                LandCells[cellIdx].Polygons[polyIdx + 1] = AddPolygon(firstVertex * 2 + 1, nextVertIdx + 1, vertIdx + 1, nextVertIdx);
                            }
                            else
                            {
                                SWtoNEcut[polyIdx] = true;

                                LandCells[cellIdx].Polygons[polyIdx] = AddPolygon(firstVertex * 2, vertIdx, nextVertIdx, nextVertIdx + 1);
                                LandCells[cellIdx].Polygons[polyIdx + 1] = AddPolygon(firstVertex * 2 + 1, vertIdx, nextVertIdx + 1, vertIdx + 1);
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
            // texture coords for rendering omitted
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
            if (VertexArray.Type == 1)
                VertexArray.DeleteUVs();    // unneeded?

            if (LandCells != null)
            {
                RemoveSurfaces();
                LandCells = null;
            }

            Polygons = null;

            VertexArray.DestroyVertex();

            SWtoNEcut = null;

            SurfaceStrips = null;
            // vertexlighting
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
                ConstructPolygons(landblockID);

            CalcWater();

            return cellRegen;
        }

        public void GetCellRotation(uint landblockID, uint x, uint y, ref int uvSet, ref int texIdx)
        {
            // only for rotating texture coords?
        }

        public void Init()
        {
            TransDir = LandDefs.Direction.Unknown;
            WaterType = LandDefs.WaterType.NotWater;
            BlockSurfaceIndex = -1;

            // init for landcell
            LandCells = new Dictionary<int, LandCell>();
            for (uint i = 1; i <= 64; i++) LandCells.Add((int)i, new LandCell((i)));
        }

        public void InitPVArrays()
        {
            var numSquares = SidePolyCount * SidePolyCount;
            var numVerts = SideVertexCount * SideVertexCount;
            var numCells = SideCellCount * SideCellCount;

            VertexArray = new VertexArray(numVerts, 1);
            for (ushort i = 0; i < numVerts; i++)
                VertexArray.Vertices.Add(new Vertex(i, LandUVs));

            var numPolys = numSquares * 2;
            Polygons = new List<Polygon>(numPolys);
            for (var i = 0; i < numPolys; i++)
                Polygons.Add(new Polygon(i, 3, CullMode.Clockwise));

            SWtoNEcut = new List<bool>(numSquares);
            for (var i = 0; i < numSquares; i++)
                SWtoNEcut.Add(false);

            LandCells = new Dictionary<int, LandCell>(numCells);
            for (uint i = 0; i < numCells; i++)
                LandCells.Add((int)i, new LandCell((ID & LandDefs.BlockMask) + i));

            // omitted lighting
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
            var cellID = blockCellID & 0xFFFF;
            var cellOffset = cellID - 1;
            uint cellX = 0, cellY = cellID;

            if (LandDefs.inbound_valid_cellid(cellID) && cellID < 0x100)
            {
                cellX = cellOffset / 8;
                cellY = cellOffset & 7;
            }
            uint terrainIdx;
            uint surfOffset = 0;
            // missing fpu instructions
            if (point.X <= point.Y)
            {
                terrainIdx = (cellY + 8 * cellX + cellX + 1);
                if (point.X == point.Y)
                    terrainIdx--;
            }
            else if (point.X >= point.Y)
                terrainIdx = cellY + 8 * (cellX + 1) + cellX + 1;
            else  // ?
            {
                terrainIdx = 8 * cellX + 10;
                surfOffset = (cellX + cellY) * 2;
            }
            var column = Terrain[(int)terrainIdx & 0xFF];
            var hasWater = SurfChar[column + (int)surfOffset& 0x7F];
            if (hasWater != 0)
            {
                if (hasWater == 1)
                    return 0.44999999f;
                else
                    return 0;
            }
            else
                return 0.1f;
        }
    }
}
