using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x08.
    /// As the name implies this contains surface info for an object. Either texture reference or color and whatever effects applied to it.
    /// </summary>
    [DatFileType(DatFileType.Surface)]
    public class Surface : FileType
    {
        public uint OrigTextureId { get; private set; }
        public uint OrigPaletteId { get; private set; }
        public uint ColorValue { get; private set; }
        public float Translucency { get; private set; }
        public float Luminosity { get; private set; }
        public float Diffuse { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            if ((Id & 6) != 0)
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
    }
}
