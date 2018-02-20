using System.Numerics;

namespace ACE.Server.Physics
{
    public class ParticleEmitterInfo
    {
        public int EmitterType;
        public int ParticleType;
        public int IsParentLocal;
        public int GfxObjID;
        public int HWGfxObjID;
        public double Birthrate;
        public int MaxParticles;
        public int InitialParticles;
        public int TotalParticles;
        public double TotalSeconds;
        public double LifeSpanRand;
        public double LifeSpan;
        public Sphere SortingSphere;
        public Vector3 OffsetDir;
        public float MinOffset;
        public float MaxOffset;
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;
        public float MinA;
        public float MaxA;
        public float MinB;
        public float MaxB;
        public float MinC;
        public float MaxC;
        public float ScaleRand;
        public float StartScale;
        public float FinalScale;
        public float TransRand;
        public float StartTrans;
        public float FinalTrans;
    }
}
