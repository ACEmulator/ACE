using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x08.
    /// As the name implies this contains surface info for an object. Either texture reference or color and whatever effects applied to it.
    /// </summary>
    public class Surface
    {
        public SurfaceType Type { get; set; }
        public uint OrigTextureId { get; set; }
        public uint OrigPaletteId { get; set; }
        public uint ColorValue { get; set; }
        public float Translucency { get; set; }
        public float Luminosity { get; set; }
        public float Diffuse { get; set; }

        public static Surface ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (Surface)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                Surface obj = new Surface();

                obj.Type = (SurfaceType)datReader.ReadUInt32();

                if (((uint)obj.Type & 6) > 0)
                {
                    obj.OrigTextureId = datReader.ReadUInt32();
                    obj.OrigPaletteId = datReader.ReadUInt32();
                }
                else
                {
                    obj.ColorValue = datReader.ReadUInt32();
                }

                obj.Translucency = datReader.ReadSingle();
                obj.Luminosity = datReader.ReadSingle();
                obj.Diffuse = datReader.ReadSingle();

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = obj;

                return obj;
            }
        }
    }
}