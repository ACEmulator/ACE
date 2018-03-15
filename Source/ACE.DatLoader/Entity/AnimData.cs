using System.IO;

namespace ACE.DatLoader.Entity
{
    public class AnimData : IUnpackable
    {
        public uint AnimId { get; private set; }
        public uint LowFrame { get; private set; }
        public uint HighFrame { get; private set; }
        /// <summary>
        /// Negative framerates play animation in reverse
        /// </summary>
        public float Framerate { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            AnimId      = reader.ReadUInt32();
            LowFrame    = reader.ReadUInt32();
            HighFrame   = reader.ReadUInt32();
            Framerate   = reader.ReadSingle();
        }
    }
}
