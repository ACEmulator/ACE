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
    }
}
