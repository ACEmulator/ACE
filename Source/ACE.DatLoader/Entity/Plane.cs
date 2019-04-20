using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    public class Plane : IUnpackable
    {
        public Vector3 N { get; private set; }
        public float D { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            N = reader.ReadVector3();
            D = reader.ReadSingle();
        }

        public System.Numerics.Plane ToNumerics()
        {
            return new System.Numerics.Plane(N, D);
        }
    }
}
