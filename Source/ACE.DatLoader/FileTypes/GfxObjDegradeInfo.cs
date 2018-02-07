using System.Collections.Generic;
using System.IO;

using ACE.DatLoader.Entity;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x11. 
    /// Contains info on what objects to display at what distance to help with render performance (e.g. low-poly very far away, but high-poly when close)
    /// </summary>
    [DatFileType(DatFileType.DegradeInfo)]
    public class GfxObjDegradeInfo : IUnpackable
    {
        public uint Id { get; private set; }
        public List<GfxObjInfo> Degrades { get; } = new List<GfxObjInfo>();

        public void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            Degrades.Unpack(reader);
        }

        public static GfxObjDegradeInfo ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.TryGetValue(fileId, out var result))
                return (GfxObjDegradeInfo)result;

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new GfxObjDegradeInfo();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }
    }
}
