using System.IO;
using System.Numerics;

namespace ACE.DatLoader.Entity
{
    public class Sphere : IUnpackable
    {
        public Vector3 Origin { get; private set; }
        public float Radius { get; private set; }

        public void Unpack(BinaryReader reader)
        {
            Origin = reader.ReadVector3();
            Radius = reader.ReadSingle();
        }

        public static Sphere CreateDummySphere()
        {
            var sphere = new Sphere();
            sphere.Origin = Vector3.Zero;
            return sphere;
        }
    }
}
