using System;
using System.Collections.Generic;

/// <summary>
/// Very special thanks to David Simpson for his early work on reading the cell.dat. Even bigger thanks for his documentation of it!
/// </summary>
namespace ACE.DatLoader.FileTypes
{
    public class CellLandblock
    {
        /// <summary>
        /// A landblock is divided into 8 x 8 tiles, which means 9 x 9 vertices reporesenting those tiles. 
        /// (Draw a grid of 9x9 dots; connect those dots to form squares; you'll have 8x8 squares)
        /// It is also divided in 192x192 units (this is the x and the y)
        /// 
        /// 0,0 is the bottom left corner of the landblock. 
        /// 
        /// Height 0-9 is Western most edge. 10-18 is S-to-N strip just to the East. And so on.
        /// </summary>

        // Places in the inland sea, for example, are false. Should denote presence of xxxxFFFE (where xxxx is the cell).
        private bool HasObjects { get; set; }

        private List<uint> Terrain { get; set; } = new List<uint>();

        // Z value in-game is double this height.
        private List<ushort> Height { get; set; } = new List<ushort>();

        /// <summary>
        /// Simple class to help calulate the Z point.
        /// TODO: Convert to AceVector3 after proof of concept complete.
        /// </summary>
        private class Point3d
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }

        /// <summary>
        /// Loads the structure of a CellLandblock from the client_cell.dat
        /// </summary>
        /// <param name="landblockId">Either a full int of the landblock or just the short of the cell itself</param>
        /// <returns></returns>
        public static CellLandblock ReadFromDat(uint landblockId)
        {
            var c = new CellLandblock();

            // Check if landblockId is a full dword. We just need the hiword for the landblockId
            if ((landblockId >> 16) != 0)
                landblockId = landblockId >> 16;

            // The file index is CELL + 0xFFFF. e.g. a cell of 1234, the file index would be 0x1234FFFF.
            var landblockFileIndex = (landblockId << 16) + 0xFFFF;

            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.CellDat.FileCache.ContainsKey(landblockFileIndex))
            {
                return (CellLandblock)DatManager.CellDat.FileCache[landblockFileIndex];
            }
            else
            {
                if (DatManager.CellDat.AllFiles.ContainsKey(landblockFileIndex))
                {
                    var datReader = DatManager.CellDat.GetReaderForFile(landblockFileIndex);
                    var cellId = datReader.ReadUInt32();

                    var hasObjects = datReader.ReadUInt32();
                    if (hasObjects == 1)
                        c.HasObjects = true;

                    // Read in the terrain. 9x9 so 81 records.
                    for (var i = 0; i < 81; i++)
                    {
                        uint terrain = datReader.ReadUInt16();
                        c.Terrain.Add(terrain);
                    }
                    // Read in the height. 9x9 so 81 records
                    for (var i = 0; i < 81; i++)
                    {
                        ushort height = datReader.ReadByte();
                        c.Height.Add(height);
                    }
                }

                // Store this object in the FileCache
                DatManager.CellDat.FileCache[landblockFileIndex] = c;

                return c;
            }
        }

        /// <summary>
        /// Calculates the z value on the CellLandblock plane at coordinate x,y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The Z value for a given X/Y in the CellLandblock</returns>
        public float GetZ(float x, float y)
        {
            // Find the exact tile in the 8x8 square grid. The cell is 192x192, so each tile is 24x24
            var tileX = (uint)Math.Ceiling(x / 24) - 1; // Subract 1 to 0-index these
            var tileY = (uint)Math.Ceiling(y / 24) - 1; // Subract 1 to 0-index these

            var v1 = tileX * 9 + tileY;
            var v2 = tileX * 9 + tileY + 1;
            var v3 = (tileX + 1) * 9 + tileY;

            var p1 = new Point3d();
            p1.X = tileX * 24;
            p1.Y = tileY * 24;
            p1.Z = Height[(int)v1] * 2;

            var p2 = new Point3d();
            p2.X = tileX * 24;
            p2.Y = (tileY + 1) * 24;
            p2.Z = Height[(int)v2] * 2;

            var p3 = new Point3d();
            p3.X = (tileX + 1) * 24;
            p3.Y = tileY * 24;
            p3.Z = Height[(int)v3] * 2;

            var z = GetPointOnPlane(p1, p2, p3, x, y);
            return z;
        }

        /// <summary>
        /// Note that we only need 3 unique points to calculate our plane.
        /// https://social.msdn.microsoft.com/Forums/en-US/1b32dc40-f84d-4365-a677-b59e49d41eb0/how-to-calculate-a-point-on-a-plane-based-on-a-plane-from-3-points?forum=vbgeneral 
        /// </summary>
        private float GetPointOnPlane(Point3d p1, Point3d p2, Point3d p3, float x, float y)
        {
            var v1 = new Point3d();
            var v2 = new Point3d();
            var abc = new Point3d();

            v1.X = p1.X - p3.X;
            v1.Y = p1.Y - p3.Y;
            v1.Z = p1.Z - p3.Z;

            v2.X = p2.X - p3.X;
            v2.Y = p2.Y - p3.Y;
            v2.Z = p2.Z - p3.Z;

            abc.X = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            abc.Y = (v1.Z * v2.X) - (v1.X * v2.Z);
            abc.Z = (v1.X * v2.Y) - (v1.Y * v2.X);

            var d = (abc.X * p3.X) + (abc.Y * p3.Y) + (abc.Z * p3.Z);

            var z = (d - (abc.X * x) - (abc.Y * y)) / abc.Z;

            return z;
        }
    }
}
