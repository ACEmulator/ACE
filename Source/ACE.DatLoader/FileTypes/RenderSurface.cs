using ACE.Entity.Enum;
using System;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.RenderSurface)]
    public class RenderSurface : FileType
    {
        public int Unknown { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SurfacePixelFormat Format { get; set; }
        public int Length { get; set; }
        public byte[] Data { get; set; }
        public uint? DefaultPaletteId { get; set; }

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            Unknown = reader.ReadInt32();
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            Format = (SurfacePixelFormat)reader.ReadUInt32();
            Length = reader.ReadInt32();

            Data = reader.ReadBytes(Length);

            switch (Format)
            {
                case SurfacePixelFormat.PFID_INDEX16:
                case SurfacePixelFormat.PFID_P8:
                    DefaultPaletteId = reader.ReadUInt32();
                    break;
                default:
                    DefaultPaletteId = null;
                    break;
            }
        }
    }
}
