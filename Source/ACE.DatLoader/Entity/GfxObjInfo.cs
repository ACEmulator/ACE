using System.IO;

namespace ACE.DatLoader.Entity
{
    public class GfxObjInfo : IUnpackable
    {
        public uint Id { get; private set; }
        public uint DegradeMode { get; private set; }
        public float MinDist { get; private set; }
        public float IdealDist { get; private set; }
        public float MaxDist { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Id          = reader.ReadUInt32();
            DegradeMode = reader.ReadUInt32();
            MinDist     = reader.ReadSingle();
            IdealDist   = reader.ReadSingle();
            MaxDist     = reader.ReadSingle();
        }
    }
}
