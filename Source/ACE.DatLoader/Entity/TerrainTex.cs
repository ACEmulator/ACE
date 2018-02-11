using System.IO;

namespace ACE.DatLoader.Entity
{
    public class TerrainTex : IUnpackable
    {
        public uint TexGID { get; private set; }
        public uint TexTiling { get; private set; }
        public uint MaxVertBright { get; private set; }
        public uint MinVertBright { get; private set; }
        public uint MaxVertSaturate { get; private set; }
        public uint MinVertSaturate { get; private set; }
        public uint MaxVertHue { get; private set; }
        public uint MinVertHue { get; private set; }
        public uint DetailTexTiling { get; private set; }
        public uint DetailTexGID { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            TexGID          = reader.ReadUInt32();
            TexTiling       = reader.ReadUInt32();
            MaxVertBright   = reader.ReadUInt32();
            MinVertBright   = reader.ReadUInt32();
            MaxVertSaturate = reader.ReadUInt32();
            MinVertSaturate = reader.ReadUInt32();
            MaxVertHue      = reader.ReadUInt32();
            MinVertHue      = reader.ReadUInt32();
            DetailTexTiling = reader.ReadUInt32();
            DetailTexGID    = reader.ReadUInt32();
        }
    }
}
