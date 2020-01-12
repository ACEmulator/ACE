using System.IO;

namespace ACE.DatLoader.Entity
{
    // TODO: refactor to merge with existing TextureMapOverride object
    public class TextureMapChange : IUnpackable
    {
        public byte PartIndex { get; set; }
        public uint OldTexture { get; set; }
        public uint NewTexture { get; set; }

        public void Unpack(BinaryReader reader)
        {
            PartIndex   = reader.ReadByte();
            OldTexture  = reader.ReadAsDataIDOfKnownType(0x05000000);
            NewTexture  = reader.ReadAsDataIDOfKnownType(0x05000000);
        }
    }
}
