using System;
using System.Numerics;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.Physics
{
    public class ParticleEmitterInfo
    {
        public DatLoader.FileTypes.ParticleEmitterInfo _info;

        public EmitterType EmitterType;
        public ParticleType ParticleType;
        public bool IsParentLocal;
        public uint GfxObjID;
        public uint HWGfxObjID;
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
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;
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

        public ParticleEmitterInfo()
        {
            // dat object type 32?
            SortingSphere = new Sphere();
            SortingSphere.Center = Vector3.Zero;

            OffsetDir = Vector3.Zero;
            A = B = C = Vector3.Zero;
            MinA = MinB = MinC = 1.0f;
            MaxA = MaxB = MaxC = 1.0f;
            StartScale = 1.0f;
            FinalScale = 1.0f;
        }

        public ParticleEmitterInfo(DatLoader.FileTypes.ParticleEmitterInfo info)
        {
            _info = info;
            EmitterType = info.EmitterType;
            ParticleType = info.ParticleType;
            GfxObjID = info.GfxObjId;
            HWGfxObjID = info.HwGfxObjId;
            Birthrate = info.Birthrate;
            MaxParticles = info.MaxParticles;
            InitialParticles = info.InitialParticles;
            TotalParticles = info.TotalParticles;
            TotalSeconds = info.TotalSeconds;
            LifeSpanRand = info.LifespanRand;
            LifeSpan = info.Lifespan;
            //SortingSphere?
            OffsetDir = info.OffsetDir;
            MinOffset = info.MinOffset;
            MaxOffset = info.MaxOffset;
            A = info.A;
            B = info.B;
            C = info.C;
            MinA = info.MinA;
            MaxA = info.MaxA;
            MinB = info.MinB;
            MaxB = info.MaxB;
            MinC = info.MinC;
            MaxC = info.MaxC;
            ScaleRand = info.ScaleRand;
            StartScale = info.StartScale;
            FinalScale = info.FinalScale;
            TransRand = info.TransRand;
            StartTrans = info.StartTrans;
            FinalTrans = info.FinalTrans;
            IsParentLocal = info.IsParentLocal != 0;

            InitEnd();
        }

        public double GetRandomStartScale()
        {
            var result = (float)ThreadSafeRandom.Next(-1.0f, 1.0f) * ScaleRand + StartScale;
            result = result.Clamp(0.1f, 10.0f);

            return result;
        }

        public double GetRandomFinalScale()
        {
            var result = (float)ThreadSafeRandom.Next(-1.0f, 1.0f) * ScaleRand * FinalScale;
            result = result.Clamp(0.1f, 10.0f);

            return result;
        }

        public double GetRandomStartTrans()
        {
            var result = (float)ThreadSafeRandom.Next(-1.0f, 1.0f) * TransRand * StartTrans;
            result = result.Clamp(0.0f, 1.0f);

            return result;
        }

        public double GetRandomFinalTrans()
        {
            var result = (float)ThreadSafeRandom.Next(-1.0f, 1.0f) * TransRand * FinalTrans;
            result = result.Clamp(0.0f, 1.0f);

            return result;
        }

        public double GetRandomLifespan()
        {
            var result = ThreadSafeRandom.Next(-1.0f, 1.0f) * LifeSpanRand + LifeSpan;
            result = Math.Max(0.0f, result);

            return result;
        }

        public int GetDBOType()
        {
            return 42;
        }

        public void InitEnd()
        {
            var maxOffset = MaxOffset;
            var velocityRadius = MaxA * LifeSpan;
            if (maxOffset <= velocityRadius)
                maxOffset = (float)velocityRadius;
            SortingSphere = new Sphere();
            SortingSphere.Radius = maxOffset;
            SortingSphere.Center = Vector3.Zero;
        }

        public bool ShouldEmitParticle(int numParticles, int totalEmitted, Vector3 emitterOffset, double lastEmitTime)
        {
            if ((TotalParticles <= 0 || totalEmitted < TotalParticles) && numParticles < MaxParticles)
            {
                if (EmitterType == EmitterType.BirthratePerSec)
                {
                    if (PhysicsTimer.CurrentTime - lastEmitTime > Birthrate)
                        return true;
                }
                else if (EmitterType == EmitterType.BirthratePerMeter)
                {
                    if (lastEmitTime < emitterOffset.LengthSquared())   // verify
                        return true;
                }
            }
            return false;
        }

        public Vector3 GetRandomOffset()
        {
            var rng = new Vector3(
                (float)ThreadSafeRandom.Next(-1.0f, 1.0f),
                (float)ThreadSafeRandom.Next(-1.0f, 1.0f),
                (float)ThreadSafeRandom.Next(-1.0f, 1.0f));

            var randomAngle = rng - OffsetDir * Vector3.Dot(OffsetDir, rng);

            if (Vec.NormalizeCheckSmall(ref randomAngle))
                return Vector3.Zero;

            var scaled = randomAngle * ((MaxOffset - MinOffset) + MinOffset) * (float)ThreadSafeRandom.Next(0.0f, 1.0f);
            return scaled;
        }

        public Vector3 GetRandomA()
        {
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            var magnitude = (MaxA - MinA) * rng + MinA;

            return A * (float)magnitude;
        }

        public Vector3 GetRandomB()
        {
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            var magnitude = (MaxB - MinB) * rng + MinB;

            return B * (float)magnitude;
        }

        public Vector3 GetRandomC()
        {
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
            var magnitude = (MaxC - MinC) * rng + MinC;

            return C * (float)magnitude;
        }
    }
}
