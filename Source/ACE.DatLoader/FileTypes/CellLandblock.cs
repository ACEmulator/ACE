using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

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
    /// <para />
    /// The fileId is CELL + 0xFFFF. e.g. a cell of 1234, the file index would be 0x1234FFFF.
    /// </summary>
    /// <remarks>
    /// Very special thanks to David Simpson for his early work on reading the cell.dat. Even bigger thanks for his documentation of it!
    /// </remarks>
    [DatFileType(DatFileType.LandBlock)]
    public class CellLandblock : FileType
    {
        /// <summary>
        /// Places in the inland sea, for example, are false. Should denote presence of xxxxFFFE (where xxxx is the cell).
        /// </summary>
        public bool HasObjects { get; set; }

        public List<ushort> Terrain { get; } = new List<ushort>();

        public static ushort TerrainMask_Road = 0x3;
        public static ushort TerrainMask_Type = 0x7C;
        public static ushort TerrainMask_Scenery = 0XF800;

        public static byte TerrainShift_Road = 0;
        public static byte TerrainShift_Type = 2;
        public static byte TerrainShift_Scenery = 11;

        /// <summary>
        /// Z value in-game is double this height.
        /// </summary>
        public List<byte> Height { get; } = new List<byte>();

        public override void Unpack(BinaryReader reader)
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

        public static ushort GetRoad(ushort terrain)
        {
            return GetTerrain(terrain, TerrainMask_Road, TerrainShift_Road);
        }

        public static ushort GetType(ushort terrain)
        {
            return GetTerrain(terrain, TerrainMask_Type, TerrainShift_Type);
        }

        public static ushort GetScenery(ushort terrain)
        {
            return GetTerrain(terrain, TerrainMask_Scenery, TerrainShift_Scenery);
        }

        public static ushort GetTerrain(ushort terrain, ushort mask, byte shift)
        {
            return (ushort)((terrain & mask) >> shift);
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

            var p1 = new Vector3();
            p1.X = tileX * 24;
            p1.Y = tileY * 24;
            p1.Z = Height[(int)v1] * 2;

            var p2 = new Vector3();
            p2.X = tileX * 24;
            p2.Y = (tileY + 1) * 24;
            p2.Z = Height[(int)v2] * 2;

            var p3 = new Vector3();
            p3.X = (tileX + 1) * 24;
            p3.Y = tileY * 24;
            p3.Z = Height[(int)v3] * 2;

            return GetPointOnPlane(p1, p2, p3, x, y);
        }

        /// <summary>
        /// Note that we only need 3 unique points to calculate our plane.
        /// https://social.msdn.microsoft.com/Forums/en-US/1b32dc40-f84d-4365-a677-b59e49d41eb0/how-to-calculate-a-point-on-a-plane-based-on-a-plane-from-3-points?forum=vbgeneral 
        /// </summary>
        private float GetPointOnPlane(Vector3 p1, Vector3 p2, Vector3 p3, float x, float y)
        {
            var v1 = new Vector3();
            var v2 = new Vector3();
            var abc = new Vector3();

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
