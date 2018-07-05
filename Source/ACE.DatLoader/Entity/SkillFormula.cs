using System.IO;

namespace ACE.DatLoader.Entity
{
    public class SkillFormula : IUnpackable
    {
        public uint W;
        public uint X;
        public uint Y;
        public uint Z;
        public uint Attr1;
        public uint Attr2;

        public void Unpack(BinaryReader reader)
        {
            W = reader.ReadUInt32();
            X = reader.ReadUInt32();
            Y = reader.ReadUInt32();
            Z = reader.ReadUInt32();

            Attr1 = reader.ReadUInt32();
            Attr2 = reader.ReadUInt32();
        }
    }
}
