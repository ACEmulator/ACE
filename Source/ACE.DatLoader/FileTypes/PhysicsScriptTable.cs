using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x34. 
    /// </summary>
    [DatFileType(DatFileType.PhysicsScriptTable)]
    public class PhysicsScriptTable : IUnpackable
    {
        public uint Id { get; set; }
        public Dictionary<uint, PhysicsScriptTableData> ScriptTable { get; set; } = new Dictionary<uint, PhysicsScriptTableData>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            ScriptTable.Unpack(reader);
        }

        public static PhysicsScriptTable ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.TryGetValue(fileId, out var result))
                return (PhysicsScriptTable)result;

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new PhysicsScriptTable();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }
    }
}
