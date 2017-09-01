using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.FileTypes
{
    public class Scene
    {
        public uint SceneId { get; set; }
        public List<ObjectDesc> Objects { get; set; } = new List<ObjectDesc>();

        public static Scene ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (Scene)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

                Scene obj = new Scene();

                obj.SceneId = datReader.ReadUInt32();

                uint num_objects = datReader.ReadUInt32();
                for (uint i = 0; i < num_objects; i++)
                    obj.Objects.Add(ObjectDesc.Read(datReader));

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}
