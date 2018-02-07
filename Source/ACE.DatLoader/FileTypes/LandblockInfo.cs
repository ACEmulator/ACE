using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// This reads the extra items in a landblock from the client_cell.dat. This is mostly buildings, but other static/non-interactive objects like tables, lamps, are also included.
    /// CLandBlockInfo is a file designated xxyyFFFE, where xxyy is the landblock.
    /// </summary>
    /// <remarks>
    /// Very special thanks again to David Simpson for his early work on reading the cell.dat. Even bigger thanks for his documentation of it!
    /// </remarks>
    [DatFileType(DatFileType.LandBlockInfo)]
    public class LandblockInfo : IUnpackable
    {
        public uint Id { get; private set; }

        /// <summary>
        /// number of EnvCells in the landblock. This should match up to the unique items in the building stab lists.
        /// </summary>
        public uint NumCells { get; private set; }

        /// <summary>
        /// list of model numbers. 0x01 and 0x01 types and their specific locations
        /// </summary>
        public List<Stab> Objects { get; } = new List<Stab>();

        /// <summary>
        /// As best as I can tell, this only affects whether there is a restriction table or not
        /// </summary>
        public uint PackMask { get; private set; }

        /// <summary>
        /// Buildings and other structures with interior locations in the landblock
        /// </summary>
        public List<BuildInfo> Buildings { get; } = new List<BuildInfo>();

        /// <summary>
        /// The specicic landblock/cell controlled by a specific guid that controls access (e.g. housing barrier)
        /// </summary>
        public Dictionary<uint, uint> RestrictionTables { get; } = new Dictionary<uint, uint>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            NumCells = reader.ReadUInt32();

            Objects.Unpack(reader);

            ushort numBuildings = reader.ReadUInt16();
            PackMask = reader.ReadUInt16();

            Buildings.Unpack(reader, numBuildings);

            if ((PackMask & 1) == 1)
                RestrictionTables.UnpackPackedHashTable(reader);
        }

        /// <summary>
        /// Loads the structure of a CellLandblock from the client_cell.dat
        /// </summary>
        /// <param name="landblockId">Either a full int of the landblock or just the short of the cell itself</param>
        public static LandblockInfo ReadFromDatUsingLandblock(uint landblockId)
        {
            // Check if landblockId is a full dword. We just need the hiword for the landblockId
            if ((landblockId >> 16) != 0)
                landblockId = landblockId >> 16;

            return ReadFromDatUsingCell((ushort)landblockId);
        }

        /// <summary>
        /// Loads the structure of a CellLandblock from the client_cell.dat
        /// </summary>
        public static LandblockInfo ReadFromDatUsingCell(ushort cellId)
        {
            // The file index is CELL + 0xFFFF. e.g. a cell of 1234, the file index would be 0x1234FFFF.
            uint fileId = (uint)((cellId << 16) | 0xFFFE);

            return ReadFromDat(fileId);
        }

        /// <summary>
        /// Loads the structure of a CellLandblock from the client_cell.dat
        /// The file index is CELL + 0xFFFE. e.g. a cell of 1234, the file index would be 0x1234FFFE.
        /// </summary>
        public static LandblockInfo ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.CellDat.FileCache.TryGetValue(fileId, out var result))
                return (LandblockInfo)result;

            DatReader datReader = DatManager.CellDat.GetReaderForFile(fileId);

            var obj = new LandblockInfo();

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
    }
}
