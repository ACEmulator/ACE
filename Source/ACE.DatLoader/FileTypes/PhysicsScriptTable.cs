using ACE.DatLoader.Entity;
using System.Collections.Generic;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x34. 
    /// </summary>
    public class PhysicsScriptTable
    {
        public uint Id { get; set; }
        public Dictionary<uint, PhysicsScriptTableData> ScriptTable { get; set; } = new Dictionary<uint, PhysicsScriptTableData>();

        public static PhysicsScriptTable ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (PhysicsScriptTable)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                PhysicsScriptTable obj = new PhysicsScriptTable();
                obj.Id = datReader.ReadUInt32();

                uint script_table_length = datReader.ReadUInt32();
                for (uint i = 0; i < script_table_length; i++)
                {
                    uint key = datReader.ReadUInt32();
                    PhysicsScriptTableData value = PhysicsScriptTableData.Read(datReader);
                    obj.ScriptTable.Add(key, value);
                }

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}