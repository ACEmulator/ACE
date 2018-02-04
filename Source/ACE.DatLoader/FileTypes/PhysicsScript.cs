using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x33. 
    /// </summary>
    [DatFileType(DatFileType.PhysicsScript)]
    public class PhysicsScript : IUnpackable
    {
        public uint Id { get; private set; }
        public List<PhysicsScriptData> ScriptData { get; } = new List<PhysicsScriptData>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            ScriptData.Unpack(reader);
        }

        public static PhysicsScript ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
                return (PhysicsScript)DatManager.PortalDat.FileCache[fileId];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new PhysicsScript();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }
    }
}
