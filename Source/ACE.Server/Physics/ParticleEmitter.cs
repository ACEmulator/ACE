using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader.Entity;

namespace ACE.Server.Physics
{
    public class ParticleEmitter
    {
        public int ID;
        public PhysicsObj Parent;
        public int PartIndex;
        public Frame ParentOffset;
        public PhysicsObj PhysObj;
        public ParticleEmitterInfo Info;
        public List<Particle> Particles;
        public List<PhysicsPart> PartStorage;
        public List<PhysicsPart> Parts;
        public int DegradedOut;
        public float DegradeDistance;
        public double CreationTime;
        public int NumParticles;
        public int TotalEmitted;
        public double LastEmitTime;
        public Vector3 LastEmitOffset;
        public int Stopped;
        public double LastUpdateTime;
    }
}
