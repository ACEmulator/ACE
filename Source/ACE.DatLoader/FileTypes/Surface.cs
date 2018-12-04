using System.IO;
using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x08.
    /// As the name implies this contains surface info for an object. Either texture reference or color and whatever effects applied to it.
    /// </summary>
    [DatFileType(DatFileType.Surface)]
    public class Surface : FileType
    {
        public SurfaceType Type { get; private set; }
        public uint OrigTextureId { get; private set; }
        public uint OrigPaletteId { get; private set; }
        public uint ColorValue { get; private set; }
        public float Translucency { get; private set; }
        public float Luminosity { get; private set; }
        public float Diffuse { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            Type = (SurfaceType)reader.ReadUInt32();

            if (Type.HasFlag(SurfaceType.Base1Image) || Type.HasFlag(SurfaceType.Base1ClipMap))
            {
                // image or clipmap
                OrigTextureId = reader.ReadUInt32();
                OrigPaletteId = reader.ReadUInt32();
            }
            else
            {
                // solid color
                ColorValue = reader.ReadUInt32();
            }

            Translucency    = reader.ReadSingle();
            Luminosity      = reader.ReadSingle();
            Diffuse         = reader.ReadSingle();
        }
    }
}
