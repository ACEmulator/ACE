using System.IO;

namespace ACE.DatLoader.Entity
{
    public class SkyObjectReplace : IUnpackable
    {
        public uint ObjectIndex { get; private set; }
        public uint GFXObjId { get; private set; }
        public float Rotate { get; private set; }
        public float Transparent { get; private set; }
        public float Luminosity { get; private set; }
        public float MaxBright { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            ObjectIndex = reader.ReadUInt32();
            GFXObjId    = reader.ReadUInt32();
            Rotate      = reader.ReadSingle();
            Transparent = reader.ReadSingle();
            Luminosity  = reader.ReadSingle();
            MaxBright   = reader.ReadSingle();

            reader.AlignBoundary();
        }
    }
}
