using System.IO;
using System.Numerics;

namespace ACE.Entity
{
    /// <summary>
    /// Vector3 with ACE Serialize
    /// </summary>
    public class AceVector3
    {
        private Vector3 vector;

        public AceVector3(float x, float y, float z)
        {
            vector.X = x; vector.Y = y; vector.Z = z;
        }

        public void Update(float x, float y, float z)
        {
            vector.X = x; vector.Y = y; vector.Z = z;
        }
        
        public Vector3 Forward()
        {
            // todo figure out how to calculate vector forward for AC.
            return vector;
        }

        public Vector3 Get()
        {
            return vector;
        }

        public void Serialize(BinaryWriter payload)
        {
            payload.Write(vector.X);
            payload.Write(vector.Y);
            payload.Write(vector.Z);
        }
    }
}
