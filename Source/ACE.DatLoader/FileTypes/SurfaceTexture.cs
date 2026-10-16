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
        // These fields use the same serialized layout as RenderTexture.
        // DataCategory is zero in the retail SurfaceTexture records examined;
        // its broader meaning has not been independently established.
        public uint DataCategory { get; private set; }

        // The examined retail SurfaceTexture records use TextureType.Texture2D (2).
        public TextureType TextureType { get; private set; }

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
