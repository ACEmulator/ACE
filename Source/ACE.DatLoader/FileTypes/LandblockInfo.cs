using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// This reads the extra items in a landblock from the client_cell.dat. This is mostly buildings, but other static/non-interactive objects like tables, lamps, are also included.
    /// CLandBlockInfo is a file designated xxyyFFFE, where xxyy is the landblock.
    /// <para />
    /// The fileId is CELL + 0xFFFE. e.g. a cell of 1234, the file index would be 0x1234FFFE.
    /// </summary>
    /// <remarks>
    /// Very special thanks again to David Simpson for his early work on reading the cell.dat. Even bigger thanks for his documentation of it!
    /// </remarks>
    [DatFileType(DatFileType.LandBlockInfo)]
    public class LandblockInfo : FileType
    {
        /// <summary>
        /// number of EnvCells in the landblock. This should match up to the unique items in the building stab lists.
        /// </summary>
        public uint NumCells { get; private set; }

        /// <summary>
        /// list of model numbers. 0x01 and 0x02 types and their specific locations
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
        /// The specific landblock/cell controlled by a specific guid that controls access (e.g. housing barrier)
        /// </summary>
        public Dictionary<uint, uint> RestrictionTables { get; } = new Dictionary<uint, uint>();

        public override void Unpack(BinaryReader reader)
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
    }
}
