using System.IO;

namespace ACE.DatLoader.Entity
{
    public class FaceStripCG : IUnpackable
    {
        public uint IconImage { get; private set; }
        public ObjDesc ObjDesc { get; } = new ObjDesc();

        public void Unpack(BinaryReader reader)
        {
            IconImage = reader.ReadUInt32();

            ObjDesc.Unpack(reader);
        }
    }
}
