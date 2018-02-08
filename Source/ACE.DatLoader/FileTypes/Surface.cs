using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x08.
    /// As the name implies this contains surface info for an object. Either texture reference or color and whatever effects applied to it.
    /// </summary>
    [DatFileType(DatFileType.Surface)]
    public class Surface : IUnpackable
    {
        public uint Type { get; private set; }
        public uint OrigTextureId { get; private set; }
        public uint OrigPaletteId { get; private set; }
        public uint ColorValue { get; private set; }
        public float Translucency { get; private set; }
        public float Luminosity { get; private set; }
        public float Diffuse { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Type = reader.ReadUInt32();

            if ((Type & 6) != 0)
            {
                OrigTextureId = reader.ReadUInt32();
                OrigPaletteId = reader.ReadUInt32();
            }
            else
            {
                ColorValue = reader.ReadUInt32();
            }

            Translucency    = reader.ReadSingle();
            Luminosity      = reader.ReadSingle();
            Diffuse         = reader.ReadSingle();
        }

        public static Surface ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.TryGetValue(fileId, out var result))
                return (Surface)result;

            DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);

            var obj = new Surface();

            using (var memoryStream = new MemoryStream(datReader.Buffer))
            using (var reader = new BinaryReader(memoryStream))
                obj.Unpack(reader);

            // Store this object in the FileCache
            DatManager.PortalDat.FileCache[fileId] = obj;

            return obj;
        }
    }
}
