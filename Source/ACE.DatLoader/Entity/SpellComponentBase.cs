using System.IO;

namespace ACE.DatLoader.Entity
{
    public class SpellComponentBase : IUnpackable
    {
        public string Name { get; private set; }
        public uint Category { get; private set; }
        public uint Icon { get; private set; }
        public uint Type { get; private set; }
        public uint Gesture { get; private set; }
        public float Time { get; private set; }
        public string Text { get; private set; }
        public float CDM { get; private set; } // Unsure what this is

        public void Unpack(BinaryReader reader)
        {
            Name        = reader.ReadObfuscatedString();
            reader.AlignBoundary();
            Category    = reader.ReadUInt32();
            Icon        = reader.ReadUInt32();
            Type        = reader.ReadUInt32();
            Gesture     = reader.ReadUInt32();
            Time        = reader.ReadSingle();
            Text        = reader.ReadObfuscatedString();
            reader.AlignBoundary();
            CDM         = reader.ReadSingle();
        }
    }
}
