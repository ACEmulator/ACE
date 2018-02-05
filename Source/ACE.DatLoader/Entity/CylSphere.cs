using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    public class CylSphere : IUnpackable
    {
        public Vector3 Origin { get; private set; }
        public float Radius { get; private set; }
        public float Height { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var z = reader.ReadSingle();
            Origin = new Vector3(x, y, z);

            Radius = reader.ReadSingle();
            Height = reader.ReadSingle();
        }
    }
}
