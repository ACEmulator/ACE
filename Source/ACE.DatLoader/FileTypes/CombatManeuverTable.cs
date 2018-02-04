using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x30. 
    /// </summary>
    [DatFileType(DatFileType.CombatTable)]
    public class CombatManeuverTable : IUnpackable
    {
        public uint Id { get; private set; }
        public List<CombatManeuver> CMT { get; } = new List<CombatManeuver>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32(); // This should always equal the fileId

            CMT.Unpack(reader);
        }

        public static CombatManeuverTable ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
                return (CombatManeuverTable)DatManager.PortalDat.FileCache[fileId];

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new CombatManeuverTable();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }
    }
}
