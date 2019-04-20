using System.IO;

namespace ACE.DatLoader.Entity
{
    public class RoadAlphaMap : IUnpackable
    {
        public uint RCode { get; private set; }
        public uint RoadTexGID { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            RCode       = reader.ReadUInt32();
            RoadTexGID  = reader.ReadUInt32();
        }
    }
}
