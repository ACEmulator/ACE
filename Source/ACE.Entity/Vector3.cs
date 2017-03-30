using System.IO;
using System.Numerics;

namespace ACE.Entity
{
    public class AceVector3
    {
        private float vx;
        private float vy;
        private float vz;

        public AceVector3(float x, float y, float z)
        {
            vx = x; vy = y; vz = z;
        }

        public void Serialize(BinaryWriter payload)
        {
            payload.Write(vx);
            payload.Write(vy);
            payload.Write(vz);
        }
    }
}
