using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
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
    /// <remarks>
    /// Very special thanks to David Simpson for his early work on reading the cell.dat. Even bigger thanks for his documentation of it!
    /// </remarks>
    [DatFileType(DatFileType.LandBlock)]
    public class CellLandblock : IUnpackable
    {
        public uint Id { get; private set; }

        /// <summary>
        /// Places in the inland sea, for example, are false. Should denote presence of xxxxFFFE (where xxxx is the cell).
        /// </summary>
        public bool HasObjects { get; set; }

        public List<ushort> Terrain { get; } = new List<ushort>();

        /// <summary>
        /// Z value in-game is double this height.
        /// </summary>
        public List<byte> Height { get; } = new List<byte>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            uint hasObjects = reader.ReadUInt32();
            if (hasObjects == 1)
                HasObjects = true;

            // Read in the terrain. 9x9 so 81 records.
            for (int i = 0; i < 81; i++)
            {
                var terrain = reader.ReadUInt16();
                Terrain.Add(terrain);
            }

            // Read in the height. 9x9 so 81 records
            for (int i = 0; i < 81; i++)
            {
                var height = reader.ReadByte();
                Height.Add(height);
            }

            reader.AlignBoundary();
        }

        /// <summary>
        /// Loads the structure of a CellLandblock from the client_cell.dat
        /// </summary>
        /// <param name="landblockId">Either a full int of the landblock or just the short of the cell itself</param>
        public static CellLandblock ReadFromDatUsingLandblock(uint landblockId)
        {
            // Check if landblockId is a full dword. We just need the hiword for the landblockId
            if ((landblockId >> 16) != 0)
                landblockId = landblockId >> 16;

            return ReadFromDatUsingCell((ushort)landblockId);
        }

        /// <summary>
        /// Loads the structure of a CellLandblock from the client_cell.dat
        /// </summary>
        public static CellLandblock ReadFromDatUsingCell(ushort cellId)
        {
            // The file index is CELL + 0xFFFF. e.g. a cell of 1234, the file index would be 0x1234FFFF.
            uint fileId = (uint)((cellId << 16) | 0xFFFF);

            return ReadFromDat(fileId);
        }

        /// <summary>
        /// Loads the structure of a CellLandblock from the client_cell.dat
        /// The file index is CELL + 0xFFFF. e.g. a cell of 1234, the file index would be 0x1234FFFF.
        /// </summary>
        public static CellLandblock ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.CellDat.FileCache.ContainsKey(fileId))
                return (CellLandblock)DatManager.CellDat.FileCache[fileId];

            DatReader datReader = DatManager.CellDat.GetReaderForFile(fileId);

            var obj = new CellLandblock();

            if (datReader != null)
            {
                using (var memoryStream = new MemoryStream(datReader.Buffer))
                using (var reader = new BinaryReader(memoryStream))
                    obj.Unpack(reader);
            }

            // Store this object in the FileCache
            DatManager.CellDat.FileCache[fileId] = obj;

            return obj;
        }

        /// <summary>
        /// Simple class to help calulate the Z point.
        /// </summary>
        private class Point3D
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
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
            uint tileX = (uint)Math.Ceiling(x / 24) - 1; // Subract 1 to 0-index these
            uint tileY = (uint)Math.Ceiling(y / 24) - 1; // Subract 1 to 0-index these

            uint v1 = tileX * 9 + tileY;
            uint v2 = tileX * 9 + tileY + 1;
            uint v3 = (tileX + 1) * 9 + tileY;

            Point3D p1 = new Point3D();
            p1.X = tileX * 24;
            p1.Y = tileY * 24;
            p1.Z = Height[(int)v1] * 2;

            Point3D p2 = new Point3D();
            p2.X = tileX * 24;
            p2.Y = (tileY + 1) * 24;
            p2.Z = Height[(int)v2] * 2;

            Point3D p3 = new Point3D();
            p3.X = (tileX + 1) * 24;
            p3.Y = tileY * 24;
            p3.Z = Height[(int)v3] * 2;

            return GetPointOnPlane(p1, p2, p3, x, y);
        }

        /// <summary>
        /// Note that we only need 3 unique points to calculate our plane.
        /// https://social.msdn.microsoft.com/Forums/en-US/1b32dc40-f84d-4365-a677-b59e49d41eb0/how-to-calculate-a-point-on-a-plane-based-on-a-plane-from-3-points?forum=vbgeneral 
        /// </summary>
        private float GetPointOnPlane(Point3D p1, Point3D p2, Point3D p3, float x, float y)
        {
            Point3D v1 = new Point3D();
            Point3D v2 = new Point3D();
            Point3D abc = new Point3D();

            v1.X = p1.X - p3.X;
            v1.Y = p1.Y - p3.Y;
            v1.Z = p1.Z - p3.Z;

            v2.X = p2.X - p3.X;
            v2.Y = p2.Y - p3.Y;
            v2.Z = p2.Z - p3.Z;

            abc.X = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            abc.Y = (v1.Z * v2.X) - (v1.X * v2.Z);
            abc.Z = (v1.X * v2.Y) - (v1.Y * v2.X);

            float d = (abc.X * p3.X) + (abc.Y * p3.Y) + (abc.Z * p3.Z);

            float z = (d - (abc.X * x) - (abc.Y * y)) / abc.Z;

            return z;
        }
    }
}
