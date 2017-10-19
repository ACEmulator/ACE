using ACE.DatLoader.Entity;
using ACE.Entity;
using System.Collections.Generic;

/// <summary>
/// This reads an "indoor" cell from the client_cell.dat. This is mostly dungeons, but can also be a building interior.
/// An EnvCell is designated by starting 0x0100 (whereas all landcells are in the 0x0001 - 0x003E range.
/// </summary>
/// <remarks>
/// Very special thanks again to David Simpson for his early work on reading the cell.dat. Even bigger thanks for his documentation of it!
/// </remarks>
namespace ACE.DatLoader.FileTypes
{
    public class EnvCell
    {
        public uint CellId { get; set; }
        public uint Bitfield { get; set; }

        // 0x08000000 textures (which contains degrade/quality info to reference the specific 0x06000000 graphics)
        public List<uint> Textures { get; set; } = new List<uint>();
        
        // the 0x0D000000 model of the pre-fab dungeon block
        public uint EnvironmentId { get; set; } 

        public ushort CellStructure { get; set; }
        public Position Position { get; set; }
        public List<CellPortal> CellPortals { get; set; } = new List<CellPortal>();
        public List<ushort> VisibleBlocks { get; set; } = new List<ushort>();
        public List<Stab> StabList { get; set; } = new List<Stab>(); // List of objects in the cell and their positions

        /// <summary>
        /// Load the EnvCell (Dungeon/Interior Block) from the client_cell.dat
        /// </summary>
        /// <param name="landblockId">The full int32/dword landblock value as reported by the @loc command (e.g. 0x12345678)</param>
        public static EnvCell ReadFromDat(uint landblockId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.CellDat.FileCache.ContainsKey(landblockId))
            {
                return (EnvCell)DatManager.CellDat.FileCache[landblockId];
            }
            else
            {
                EnvCell c = new EnvCell();

                if (DatManager.CellDat.AllFiles.ContainsKey(landblockId))
                {
                    DatReader datReader = DatManager.CellDat.GetReaderForFile(landblockId);
                    c.CellId = datReader.ReadUInt32();
                    c.Bitfield = datReader.ReadUInt32();
                    datReader.Offset += 4; // Skip ahead 4 bytes, because this is the CellId. Again. Twice.

                    byte numTextures = datReader.ReadByte();

                    // Note that "portal" in this context does not refer to the swirly pink/purple thing, its basically connecting cells
                    byte numPortals = datReader.ReadByte();
                    
                    // I believe this is what cells can be seen from this one. So the engine knows what else it needs to load/draw.
                    ushort numVisibleBlocks = datReader.ReadUInt16();

                    // Read what textures are used in this cell
                    for (uint i = 0; i < numTextures; i++)
                    {
                        c.Textures.Add(0x08000000u + datReader.ReadUInt16()); // these are stored in the dat as short vals, so we'll make them a full dword
                    }

                    c.EnvironmentId = (0x0D000000u + datReader.ReadUInt16());
                    c.CellStructure = datReader.ReadUInt16();

                    c.Position = new Position();
                    c.Position.LandblockId = new LandblockId(landblockId);
                    c.Position.PositionX = datReader.ReadSingle();
                    c.Position.PositionY = datReader.ReadSingle();
                    c.Position.PositionZ = datReader.ReadSingle();
                    c.Position.RotationW = datReader.ReadSingle();
                    c.Position.RotationX = datReader.ReadSingle();
                    c.Position.RotationY = datReader.ReadSingle();
                    c.Position.RotationZ = datReader.ReadSingle();

                    for (uint i = 0; i < numPortals; i++)
                    {
                        CellPortal cp = new CellPortal();
                        cp.Flags = datReader.ReadUInt16();
                        cp.EnvironmentId = datReader.ReadUInt16();
                        cp.OtherCellId = datReader.ReadUInt16();
                        cp.OtherPortalId = datReader.ReadUInt16();
                        cp.ExactMatch = (byte)(cp.Flags & 1);
                        cp.PortalSide = (byte)((cp.Flags >> 1) & 1);
                        c.CellPortals.Add(cp);
                    }

                    for (uint i = 0; i < numVisibleBlocks; i++)
                    {
                        c.VisibleBlocks.Add(datReader.ReadUInt16());
                    }

                    uint numObjects = datReader.ReadUInt32();
                    for (uint i = 0; i < numObjects; i++)
                    {
                        Stab s = new Stab();
                        s.Model = datReader.ReadUInt32();
                        s.Position.LandblockId = new LandblockId(landblockId);
                        s.Position.PositionX = datReader.ReadSingle();
                        s.Position.PositionY = datReader.ReadSingle();
                        s.Position.PositionZ = datReader.ReadSingle();
                        s.Position.RotationW = datReader.ReadSingle();
                        s.Position.RotationX = datReader.ReadSingle();
                        s.Position.RotationY = datReader.ReadSingle();
                        s.Position.RotationZ = datReader.ReadSingle();
                        c.StabList.Add(s);
                    }
                }

                // Store this object in the FileCache
                DatManager.CellDat.FileCache[landblockId] = c;
                return c;
            }
        }
    }
}
