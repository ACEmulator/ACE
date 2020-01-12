using System;
using System.Numerics;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    public class Particle
    {
        // union?
        public double Birthtime { get => LastUpdateTime; set => LastUpdateTime = value; }
        public double LastUpdateTime;

        public double Lifespan;
        public double Lifetime;
        public AFrame StartFrame;
        public Vector3 Offset;
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;
        public float StartScale;
        public float FinalScale;
        public float StartTrans;
        public float FinalTrans;
        public PhysicsObj Owner;

        public bool Init(ParticleEmitterInfo info, PhysicsObj parent, int partIdx, AFrame pFrame, PhysicsPart part, Vector3 _offset, bool persistent, Vector3 a, Vector3 b, Vector3 c)
        {
            var currentTime = PhysicsTimer.CurrentTime;

            LastUpdateTime = currentTime;
            Birthtime = currentTime;
            Lifetime = 0;

            Lifespan = info.GetRandomLifespan();

            if (partIdx == -1)
                StartFrame = new AFrame(parent.Position.Frame);
            else
                StartFrame = new AFrame(parent.PartArray.Parts[partIdx].Pos.Frame);

            Offset = StartFrame.LocalToGlobalVec(pFrame.Origin + _offset);

            switch (info.ParticleType)
            {
                case ParticleType.Still:
                    break;
                case ParticleType.LocalVelocity:
                    A = StartFrame.LocalToGlobalVec(a);
                    break;
                case ParticleType.ParabolicLVGA:
                    B = StartFrame.LocalToGlobalVec(b);
                    break;
                case ParticleType.ParabolicLVGAGR:
                    C = StartFrame.LocalToGlobalVec(c);
                    break;
                case ParticleType.Swarm:
                    A = StartFrame.LocalToGlobalVec(a);
                    B = b;
                    C = c;
                    break;
                case ParticleType.Explode:
                    A = a;
                    B = b;

                    var ra = ThreadSafeRandom.Next(-(float)Math.PI, (float)Math.PI);
                    var po = ThreadSafeRandom.Next(-(float)Math.PI, (float)Math.PI);
                    var rb = Math.Cos(po);

                    C.X = (float)(Math.Cos(ra) * c.X * rb);
                    C.Y = (float)(Math.Sin(ra) * c.Y * rb);
                    C.Z = (float)(Math.Sin(po) * c.Z * rb);

                    if (Vec.NormalizeCheckSmall(ref C))
                        C = Vector3.Zero;

                    break;
                case ParticleType.Implode:
                    A = a;
                    B = b;
                    Offset *= c;
                    C = Offset;
                    break;
                case ParticleType.ParabolicLVLA:
                    A = StartFrame.LocalToGlobalVec(a);
                    B = StartFrame.LocalToGlobalVec(b);
                    break;
                case ParticleType.ParabolicLVLALR:
                    C = StartFrame.LocalToGlobalVec(c);
                    break;
                case ParticleType.ParabolicGVGA:
                    B = b;
                    break;
                case ParticleType.ParabolicGVGAGR:
                    C = c;
                    break;
                case ParticleType.GlobalVelocity:
                    A = a;
                    break;
                default:
                    A = a;
                    B = b;
                    C = c;
                    break;
            }

            StartScale = info.StartScale;
            FinalScale = info.FinalScale;
            StartTrans = info.StartTrans;
            FinalTrans = info.FinalTrans;

            part.GfxObjScale = new Vector3(StartScale, StartScale, StartScale);
            part.SetTranslucency(StartTrans);

            Update(info.ParticleType, persistent, part, pFrame);

            return false;
        }

        public void Update(ParticleType particleType, bool persistent, PhysicsPart part, AFrame parent)
        {
            var currentTime = PhysicsTimer.CurrentTime;
            var elapsedTime = currentTime - LastUpdateTime;

            if (persistent)
            {
                Lifetime += elapsedTime;
                LastUpdateTime = currentTime;
            }
            else
                Lifetime = elapsedTime;

            var lifetime = (float)Lifetime;

            switch (particleType)
            {
                case ParticleType.Still:
                    part.Pos.Frame.Origin = parent.Origin + Offset;
                    break;
                case ParticleType.LocalVelocity:
                case ParticleType.GlobalVelocity:
                    part.Pos.Frame.Origin = (lifetime * A) + parent.Origin + Offset;
                    break;
                case ParticleType.ParabolicLVGA:
                case ParticleType.ParabolicLVLA:
                case ParticleType.ParabolicGVGA:
                    part.Pos.Frame.Origin += (lifetime * lifetime * B / 2.0f) + (lifetime * A) + Offset;
                    break;
                case ParticleType.ParabolicLVGAGR:
                case ParticleType.ParabolicLVLALR:
                case ParticleType.ParabolicGVGAGR:
                    part.Pos.Frame = new AFrame(parent);
                    part.Pos.Frame.Origin += (lifetime * lifetime * B / 2.0f) + (lifetime * A) + Offset;
                    part.Pos.Frame.Rotate(lifetime * C);
                    break;
                case ParticleType.Swarm:
                    var swarm = (lifetime * A) + C + parent.Origin + Offset;
                    part.Pos.Frame.Origin.X = (float)Math.Cos(lifetime * B.X) + swarm.X;
                    part.Pos.Frame.Origin.Y = (float)Math.Sin(lifetime * B.Y) + swarm.Y;
                    part.Pos.Frame.Origin.Z = (float)Math.Cos(lifetime * B.Z) + swarm.Z;
                    break;
                case ParticleType.Explode:
                    part.Pos.Frame.Origin = (lifetime * B + C * A.X) * lifetime + Offset + parent.Origin;
                    break;
                case ParticleType.Implode:
                    part.Pos.Frame.Origin = ((float)Math.Cos(A.X * lifetime) * C) + (lifetime * lifetime * B) + parent.Origin + Offset;
                    break;
            }

            var interval = Math.Min(Lifetime / Lifespan, 1.0f);

            var currentScale = StartScale + (FinalScale - StartScale) * interval;
            var currentTrans = StartTrans + (FinalTrans - StartTrans) * interval;

            part.GfxObjScale = new Vector3((float)currentScale);
            part.SetTranslucency((float)currentTrans);
        }
    }
}
