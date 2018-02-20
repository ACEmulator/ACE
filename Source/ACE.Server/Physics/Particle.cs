using System.Numerics;
using ACE.DatLoader.Entity;

namespace ACE.Server.Physics
{
    public class Particle
    {
        public double Lifespan;
        public double Lifetime;
        public Frame StartFrame;
        public Vector3 Offset;
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;
        public float StartScale;
        public float FinalScale;
        public float StartTrans;
        public float FinalTrans;
    }
}
