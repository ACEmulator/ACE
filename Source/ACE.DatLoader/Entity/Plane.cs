using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class Plane : IUnpackable
    {
        public Position N { get; } = new Position();
        public float D { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            N.ReadFrame(reader);
            D = reader.ReadSingle();
        }
    }
}
