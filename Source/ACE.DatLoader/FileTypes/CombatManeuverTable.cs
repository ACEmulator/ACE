using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x30. 
    /// </summary>
    public class CombatManeuverTable
    {
        public uint Id { get; set; }
        public List<CombatManeuver> CMT { get; set; } = new List<CombatManeuver>();

        public static CombatManeuverTable ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (CombatManeuverTable)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                CombatManeuverTable obj = new CombatManeuverTable();

                obj.Id = datReader.ReadUInt32(); // This should always equal the fileId

                uint num_combat_maneuvers = datReader.ReadUInt32();
                for (uint i = 0; i < num_combat_maneuvers; i++)
                    obj.CMT.Add(CombatManeuver.Read(datReader));

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}