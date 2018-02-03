using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x0D. 
    /// These are basically pre-fab regions for things like the interior of a dungeon.
    /// </summary>
    [DatFileType(DatFileType.Environment)]
    public class Environment : IUnpackable
    {
        public uint Id { get; set; }
        public Dictionary<uint, CellStruct> Cells { get; set; } = new Dictionary<uint, CellStruct>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32(); // this will match fileId

            Cells.Unpack(reader);
        }

        public static Environment ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
                return (Environment)DatManager.PortalDat.FileCache[fileId];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            Environment environment = new Environment();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                environment.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = environment;

            return environment;
        }
    }
}
