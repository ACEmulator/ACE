using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class LightInfo : IUnpackable
    {
        public Position ViewerspaceLocation { get; } = new Position();
        public uint Color { get; private set; } // _RGB Color. Red is bytes 3-4, Green is bytes 5-6, Blue is bytes 7-8. Bytes 1-2 are always FF (?)
        public float Intensity { get; private set; }
        public float Falloff { get; private set; }
        public float ConeAngle { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            ViewerspaceLocation.ReadFrame(reader);

            Color       = reader.ReadUInt32();
            Intensity   = reader.ReadSingle();
            Falloff     = reader.ReadSingle();
            ConeAngle   = reader.ReadSingle();
        }
    }
}
