using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x11. 
    /// Contains info on what objects to display at what distance to help with render performance (e.g. low-poly very far away, but high-poly when close)
    /// </summary>
    public class GfxObjDegradeInfo
    {
        public uint Id { get; set; }
        public List<GfxObjInfo> Degrades { get; set; } = new List<GfxObjInfo>();

        public static GfxObjDegradeInfo ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (GfxObjDegradeInfo)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                GfxObjDegradeInfo obj = new GfxObjDegradeInfo();

                obj.Id = datReader.ReadUInt32();

                uint num_degrades = datReader.ReadUInt32();
                for (uint i = 0; i < num_degrades; i++)
                    obj.Degrades.Add(GfxObjInfo.Read(datReader));

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}