using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    public class Plane : IUnpackable
    {
        public Vector3 N { get; private set; } = new Vector3();
        public float D { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            var z = reader.ReadSingle();
            N = new Vector3(x, y, z);

            D = reader.ReadSingle();
        }
    }
}
