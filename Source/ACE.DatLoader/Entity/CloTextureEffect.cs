using System.IO;

namespace ACE.DatLoader.Entity
{
    public class CloTextureEffect : IUnpackable
    {
        /// <summary>
        /// Texture portal.dat 0x05000000
        /// </summary>
        public uint OldTexture { get; private set; }

        /// <summary>
        /// Texture portal.dat 0x05000000
        /// </summary>
        public uint NewTexture { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            OldTexture = reader.ReadUInt32();
            NewTexture = reader.ReadUInt32();
        }
    }
}
