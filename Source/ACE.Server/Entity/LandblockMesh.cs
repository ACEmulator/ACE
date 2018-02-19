using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;

namespace ACE.Server.Entity
{
    /// <summary>
    /// The polygonal landblock mesh
    /// </summary>
    public class LandblockMesh: Mesh
    {
        public LandblockId LandblockId;

        /// <summary>
        /// The heights of the landblock cell vertices
        /// </summary>
        public float[,] VertexHeights;

        /// <summary>
        /// A landblock has this many cells squared
        /// </summary>
        public static readonly int CellDim = 8;

        /// <summary>
        /// A landblock is this unit size squared
        /// </summary>
        public static readonly int LandblockSize = 192;

        /// <summary>
        /// A landblock cell is this unit size squared
        /// </summary>
        public static readonly int CellSize = LandblockSize / CellDim;

        /// <summary>
        /// A landblock has this many vertices squared
        /// </summary>
        public static readonly int VertexDim = CellDim + 1;

        /// <summary>
        /// LandHeightTable mapping non-linear heights
        /// </summary>
        public static RegionDesc RegionDesc;

        /// <summary>
        /// Static constructor
        /// </summary>
        static LandblockMesh()
        {
            // load the region file from portal.dat
            RegionDesc = DatManager.PortalDat.ReadFromDat<RegionDesc>(0x13000000);
        }

        /// <summary>
        /// Constructs a new mesh for a landblock
        /// </summary>
        public LandblockMesh(LandblockId id)
        {
            LandblockId = id;

            BuildVertices();
            BuildTriangles();
        }

        /// <summary>
        /// Builds the vertices for the landblock cells
        /// </summary>
        public void BuildVertices()
        {
            VertexHeights = GetVertexHeights();
            LoadVertices(VertexHeights);
        }

        /// <summary>
        /// Reads the heights for each vertex in the landblock cells
        /// </summary>
        /// <returns>The vertex heights for the landblock cells</returns>
        public float[,] GetVertexHeights()
        {
            // The vertex heights in the cell database are stored in bytes,
            // which map to offsets in the land height table from the region file in the portal database.

            var cellLandblock = DatManager.CellDat.ReadFromDat<CellLandblock>(LandblockId.Raw | 0xFFFF);

            var heights = new float[VertexDim, VertexDim];

            for (int x = 0; x < VertexDim; x++)
            {
                for (int y = 0; y < VertexDim; y++)
                {
                    heights[x, y] = RegionDesc.LandDefs.LandHeightTable[cellLandblock.Height[x * VertexDim + y]];
                }
            }
            return heights;
        }

        /// <summary>
        /// Loads the vertices for a landblock mesh
        /// </summary>
        /// <param name="height">The height of each vertex in the landblock cells</param>
        public void LoadVertices(float[,] height)
        {
            var xSize = height.GetLength(0);
            var ySize = height.GetLength(1);

            Vertices = new List<Vector3>(xSize * ySize);

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    Vertices.Add(new Vector3(x * CellSize, y * CellSize, height[x, y]));
                }
            }
        }

        /// <summary>
        /// Generates the triangles from the mesh vertices
        /// </summary>
        /// <returns>The list of triangles generated for the landblock mesh</returns>
        public void BuildTriangles()
        {
            Triangles = new List<Triangle>();

            for (int x = 0; x < CellDim; x++)
            {
                for (int y = 0; y < CellDim; y++)
                {
                    int lowerLeft = x + y * VertexDim;
                    int lowerRight = (x + 1) + y * VertexDim;
                    int topLeft = x + (y + 1) * VertexDim;
                    int topRight = (x + 1) + (y + 1) * VertexDim;

                    // determine where to draw the split line
                    if (GetSplitDir(LandblockId, x, y))
                    {
                        // clockwise winding order
                        Triangles.Add(new Triangle(topLeft, lowerRight, lowerLeft));
                        Triangles.Add(new Triangle(topLeft, topRight, lowerRight));
                    }
                    else
                    {
                        Triangles.Add(new Triangle(topRight, lowerRight, lowerLeft));
                        Triangles.Add(new Triangle(topRight, lowerLeft, topLeft));
                    }
                }
            }
        }

        /// <summary>
        /// Determines the split line direction
        /// for a cell triangulation
        /// </summary>
        /// <param name="id">A reference to the landblock ID</param>
        /// <param name="cellX">The horizontal cell position within the landblock</param>
        /// <param name="cellY">The vertical cell position within the landblock</param>
        /// <returns>TRUE if NW-SE split, FALSE if NE-SW split</returns>
        public bool GetSplitDir(LandblockId id, int cellX, int cellY)
        {
            // get the global tile offsets
            var x = (id.LandblockX * 8) + cellX;
            var y = (id.LandblockY * 8) + cellY;

            // Thanks to https://github.com/deregtd/AC2D for this bit
            var dw = x * y * 0x0CCAC033 - x * 0x421BE3BD + y * 0x6C1AC587 - 0x519B8F25;
            return (dw & 0x80000000) == 0;
        }

        /// <summary>
        /// Returns the shared line between 2 triangles
        /// </summary>
        public Line2 GetSplitter(List<Triangle> triangles)
        {
            if (triangles[0].Indices[1] == triangles[1].Indices[2])
                return new Line2(Vertices[triangles[0].Indices[0]], Vertices[triangles[0].Indices[1]]);
            else
                return new Line2(Vertices[triangles[0].Indices[0]], Vertices[triangles[0].Indices[2]]);
        }

        /// <summary>
        /// Returns the triangle containing a pair of x,y coordinates
        /// </summary>
        public Triangle GetTriangle(Vector2 point)
        {
            // find the cell which contains these coordinates
            Vector2 cellOffset = GetCell(point);

            // get the triangles for this cell
            var cellTriangles = GetCellTriangles(cellOffset);

            // return the triangle containing this point
            if (cellTriangles[0].Contains(point, Vertices))
                return cellTriangles[0];
            else
                return cellTriangles[1];
        }

        /// <summary>
        /// Given a pair of 2D coordinates within a landblock,
        /// Returns the cell that contains these coordinates
        /// </summary>
        /// <param name="point">The 2D coordinates within the landblock</param>
        /// <returns>The cell that contains these coordinates</returns>
        public static Vector2 GetCell(Vector2 point)
        {
            var cellX = (float)Math.Floor(point.X / CellSize);
            var cellY = (float)Math.Floor(point.Y / CellSize);

            if (cellX < 0) cellX = 0;
            if (cellY < 0) cellY = 0;

            if (cellX >= CellDim) cellX = CellDim - 1;
            if (cellY >= CellDim) cellY = CellDim - 1;

            return new Vector2(cellX, cellY);
        }

        /// <summary>
        /// Returns the 2 triangles for a cell
        /// </summary>
        public List<Triangle> GetCellTriangles(Vector2 cellOffset)
        {
            var offset = (int)((cellOffset.Y * CellDim + cellOffset.X) * 2);
            // TODO: ensure within bounds
            return new List<Triangle>() { Triangles[offset], Triangles[offset + 1] };
        }

        /// <summary>
        /// Returns the z height coordinate for a 2D position within a landblock
        /// </summary>
        public float GetZ(Vector2 point)
        {
            // find the triangle that contains this point
            var triangle = GetTriangle(point);

            // calculate the z coordinate at x,y
            // for the plane defined by this triangle
            return triangle.GetZ(Vertices, point);
        }
    }
}
