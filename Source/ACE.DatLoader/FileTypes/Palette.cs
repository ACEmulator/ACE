using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x04. 
    /// </summary>
    public class Palette
    {
        public uint Id { get; set; }

        // Color data is stored in ARGB format (Alpha, Red, Green, Blue--each are two bytes long)
        public List<uint> Colors { get; set; } 

        public static Palette ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (Palette)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                Palette obj = new Palette();
                obj.Id = datReader.ReadUInt32();

                uint num_colors = datReader.ReadUInt32();
                for (uint i = 0; i < num_colors; i++)
                    obj.Colors.Add(datReader.ReadUInt32());

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}
