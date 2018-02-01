using System.IO;

using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class Sphere : IUnpackable
    {
        public AceVector3 Origin { get; private set; } = new AceVector3(0, 0, 0);
        public float Radius { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Origin = new AceVector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Radius = reader.ReadSingle();
        }
    }
}
