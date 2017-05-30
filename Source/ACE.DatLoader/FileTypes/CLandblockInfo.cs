using ACE.Entity;
using System.Collections.Generic;
using ACE.DatLoader.Entity;

/// <summary>
/// This reads the extra items in a landblock from the client_cell.dat. This is mostly buildings, but other static/non-interactive objects like tables, lamps, are also included.
/// CLandBlockInfo is a file designated xxyyFFFE, where xxyy is the landblock.
/// </summary>
/// <remarks>
/// Very special thanks again to David Simpson for his early work on reading the cell.dat. Even bigger thanks for his documentation of it!
/// </remarks>

namespace ACE.DatLoader.FileTypes
{
    public class CLandblockInfo
    {
        // number of EnvCells in the landblock. This should match up to the unique items in the building stab lists.
        public uint NumCells { get; set; } 

        // list of model numbers. 0x01 and 0x01 types.
        public List<uint> ObjectIds { get; set; } = new List<uint>(); 

        // specific locations of the above models.
        public List<Position> ObjectFrames { get; set; } = new List<Position>(); 

        // As best as I can tell, this only affects whether there is a restriction table or not
        public uint PackMask { get; set; } 

        // Buildings and other structures with interior locations in the landblock
        public List<BuildInfo> Buildings { get; set; } = new List<BuildInfo>(); 

        // Possibly part of a a packed word? Or just padding for alignment.
        public ushort UnknownShort { get; set; } 

        // The specicic landblock/cell controlled by a specific guid that controls access (e.g. housing barrier)
        public List<RestrictionTable> Restriction_table { get; set; } = new List<RestrictionTable>(); 

        public static CLandblockInfo ReadFromDat(uint landblockId)
        {
            // Check if landblockId is a full dword. We just need the hiword for the landblockId
            if ((landblockId >> 16) != 0)
                landblockId = landblockId >> 16;

            // The file index is CELL + 0xFFFF. e.g. a cell of 1234, the file index would be 0x1234FFFE.
            uint landblockFileIndex = (landblockId << 16) + 0xFFFE;

            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.CellDat.FileCache.ContainsKey(landblockFileIndex))
            {
                return (CLandblockInfo)DatManager.CellDat.FileCache[landblockFileIndex];
            }
            else
            {
                CLandblockInfo c = new CLandblockInfo();
                if (DatManager.CellDat.AllFiles.ContainsKey(landblockFileIndex))
                {
                    DatReader datReader = DatManager.CellDat.GetReaderForFile(landblockFileIndex);

                    uint file_id = datReader.ReadUInt32();
                    c.NumCells = datReader.ReadUInt32();

                    uint num_objects = datReader.ReadUInt32();
                    for (uint i = 0; i < num_objects; i++)
                    {
                        c.ObjectIds.Add(datReader.ReadUInt32());

                        Position objPosition = new Position();
                        objPosition.PositionX = datReader.ReadSingle();
                        objPosition.PositionY = datReader.ReadSingle();
                        objPosition.PositionZ = datReader.ReadSingle();
                        objPosition.RotationW = datReader.ReadSingle();
                        objPosition.RotationX = datReader.ReadSingle();
                        objPosition.RotationY = datReader.ReadSingle();
                        objPosition.RotationZ = datReader.ReadSingle();
                        c.ObjectFrames.Add(objPosition);
                    }

                    ushort num_buildings = datReader.ReadUInt16();
                    c.PackMask = datReader.ReadUInt16();

                    for (uint i = 0; i < num_buildings; i++)
                    {
                        BuildInfo b = new BuildInfo();
                        b.ModelId = datReader.ReadUInt32();

                        // position
                        b.Frame.PositionX = datReader.ReadSingle();
                        b.Frame.PositionY = datReader.ReadSingle();
                        b.Frame.PositionZ = datReader.ReadSingle();
                        b.Frame.RotationW = datReader.ReadSingle();
                        b.Frame.RotationX = datReader.ReadSingle();
                        b.Frame.RotationY = datReader.ReadSingle();
                        b.Frame.RotationZ = datReader.ReadSingle();

                        b.NumLeaves = datReader.ReadUInt32();
                        uint num_portals = datReader.ReadUInt32();

                        for (uint j = 0; j < num_portals; j++)
                        {
                            CBldPortal cbp = new CBldPortal();
                            cbp.Flags = datReader.ReadUInt16();
                            cbp.ExactMatch = (ushort)(cbp.Flags & 1);
                            cbp.PortalSide = (ushort)((cbp.Flags >> 1) & 1);

                            cbp.OtherCellId = (landblockId << 16) + datReader.ReadUInt16();
                            cbp.OtherPortalId = (landblockId << 16) + datReader.ReadUInt16();

                            ushort num_stabs = datReader.ReadUInt16();
                            for (ushort k = 0; k < num_stabs; k++)
                                cbp.StabList.Add((landblockId << 16) + datReader.ReadUInt16());

                            datReader.AlignBoundary();
                            b.Portals.Add(cbp);
                        }

                        c.Buildings.Add(b);
                    }

                    if ((c.PackMask & 1) == 1)
                    {
                        ushort num_restiction_table = datReader.ReadUInt16();
                        c.UnknownShort = datReader.ReadUInt16(); // seems to always be 0x0008 ... packed val?
                        for (ushort i = 0; i < num_restiction_table; i++)
                        {
                            RestrictionTable r = new RestrictionTable();
                            r.Landblock = datReader.ReadUInt32();
                            r.Iid = datReader.ReadUInt32();
                            c.Restriction_table.Add(r);
                        }
                    }

                    // Store this object in the FileCache
                    DatManager.CellDat.FileCache[landblockFileIndex] = c;
                }

                return c;
            }
        }
    }
}
