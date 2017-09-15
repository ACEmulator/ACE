using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0D. 
    /// These are basically pre-fab regions for things like the interior of a dungeon.
    /// </summary>
    public class Environment
    {
        public uint Id { get; set; }
        public Dictionary<uint, CellStruct> Cells { get; set; } = new Dictionary<uint, CellStruct>();

        public static Environment ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (Environment)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                Environment obj = new Environment();

                obj.Id = datReader.ReadUInt32(); // this will match fileId

                uint numCells = datReader.ReadUInt32();

                for (uint i = 0; i < numCells; i++)
                {
                    uint cellstuctId = datReader.ReadUInt32();
                    obj.Cells.Add(cellstuctId, CellStruct.Read(datReader));
                }

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}
