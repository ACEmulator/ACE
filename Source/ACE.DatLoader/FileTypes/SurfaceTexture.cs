using ACE.Common;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.DatLoader.FileTypes
{
    [DatFileType(DatFileType.SurfaceTexture)]
    public class SurfaceTexture : FileType
    {
        // The original client identifies ImgTex as SURFACETEXTURE (DB type 0x0B).
        // DBObj::Serialize reads/writes this field for categorized resource types.
        // All examined retail SurfaceTexture records have DataCategory == 0.
        public uint DataCategory { get; private set; }

        // ImgTex::Serialize writes a fixed byte value of 2.
        // RenderTexture uses TextureType.Texture2D for this value.
        // On read, the original ImgTex serializer skips the byte without interpreting it.
        public TextureType TextureType { get; private set; }

        // References to Texture resources (IDs in the 0x06 and 0x07 ranges).
        public List<uint> Textures { get; private set; } = new List<uint>(); // References to Texture (0x06) entries.

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            DataCategory = reader.ReadUInt32();
            TextureType = (TextureType)reader.ReadByte();
            Textures.Unpack(reader);
        }
    }
}
