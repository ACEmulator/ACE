using System.IO;
using System.Numerics;

namespace ACE.Entity
{
    /// <summary>
    /// Vector3 with ACE Serialize
    /// </summary>
    public class AceVector3
    {
        private Vector3 Vector;

        public AceVector3(float x, float y, float z)
        {
            Vector.X = x; Vector.Y = y; Vector.Z = z;
        }

        public void Update(float x, float y, float z)
        {
            Vector.X = x; Vector.Y = y; Vector.Z = z;
        }


        public Vector3 Forward()
        {
            //todo figure out how to calculate vector forward for AC.
            return Vector;
        }

        public Vector3 Get()
        {
            return Vector;
        }

        public void Serialize(BinaryWriter payload)
        {
            payload.Write(Vector.X);
            payload.Write(Vector.Y);
            payload.Write(Vector.Z);
        }
    }
}
