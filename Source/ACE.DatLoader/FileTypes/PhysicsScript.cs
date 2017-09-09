using ACE.DatLoader.Entity;
using System.Collections.Generic;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x33. 
    /// </summary>
    public class PhysicsScript
    {
        public uint Id { get; set; }
        public List<PhysicsScriptData> ScriptData { get; set; } = new List<PhysicsScriptData>();

        public static PhysicsScript ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (PhysicsScript)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                PhysicsScript obj = new PhysicsScript();
                obj.Id = datReader.ReadUInt32();

                uint num_script_data = datReader.ReadUInt32();
                for (uint i = 0; i < num_script_data; i++)
                    obj.ScriptData.Add(PhysicsScriptData.Read(datReader));

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}
