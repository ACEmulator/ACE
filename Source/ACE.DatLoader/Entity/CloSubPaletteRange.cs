using System.IO;

namespace ACE.DatLoader.Entity
{
    public class CloSubPaletteRange : IUnpackable
    {
        public uint Offset { get; private set; }
        public uint NumColors { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Offset      = reader.ReadUInt32();
            NumColors   = reader.ReadUInt32();
        }
    }
}
