using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    public class ParticleEmitter
    {
        public int ID;
        public PhysicsObj Parent;
        public int PartIndex;
        public AFrame ParentOffset;
        public PhysicsObj PhysicsObj;
        public ParticleEmitterInfo Info;
        public Particle[] Particles;
        public PhysicsPart[] PartStorage;
        public List<PhysicsPart> Parts;
        public int DegradedOut;
        public float DegradeDistance;
        public double CreationTime;
        public int NumParticles;
        public int TotalEmitted;
        public double LastEmitTime;
        public Vector3 LastEmitOffset;
        public bool Stopped;
        public double LastUpdateTime;

        public ParticleEmitter(PhysicsObj parent)
        {
            Parent = parent;
            PartIndex = -1;
            ParentOffset = new AFrame();
            LastUpdateTime = PhysicsTimer.CurrentTime;
            LastEmitTime = PhysicsTimer.CurrentTime;
        }

        public bool SetParenting(int partIdx, AFrame frame)
        {
            if (PhysicsObj == null || !PhysicsObj.set_parent(Parent, partIdx, frame))
                return false;

            PartIndex = partIdx;
            ParentOffset = new AFrame(frame);
            return true;
        }

        public bool KillParticle(int i)
        {
            if (Particles[i].Lifetime < Particles[i].Lifespan)
                return false;

            PhysicsObj.RemovePartFromShadowCells(Parts[i]);
            Parts[i] = null;
            NumParticles--;
            return true;
        }

        public bool StopEmitter()
        {
            if (!Stopped)
            {
                if (Info.TotalSeconds > 0.0f && CreationTime + Info.TotalSeconds < PhysicsTimer.CurrentTime)
                    Stopped = true;

                if (Info.TotalParticles > 0 && TotalEmitted >= Info.TotalParticles)
                    Stopped = true;
            }

            return Stopped;
        }

        public void RecordParticleEmission()
        {
            NumParticles++;
            TotalEmitted++;

            LastEmitOffset = PhysicsObj.Position.Frame.Origin;
            LastEmitTime = PhysicsTimer.CurrentTime;
        }

        public bool ShouldEmitParticle()
        {
            var offset = Vector3.Zero;

            if (Info.EmitterType == EmitterType.BirthratePerMeter)
                offset = PhysicsObj.Position.Frame.Origin - LastEmitOffset;

            return Info.ShouldEmitParticle(NumParticles, TotalEmitted, offset, LastEmitTime);
        }

        public static ParticleEmitter makeParticleEmitter(PhysicsObj parent)
        {
            if (parent == null)
                return null;

            return new ParticleEmitter(parent);
        }

        public bool SetInfo(ParticleEmitterInfo info)
        {
            // destroy first?
            Info = info;
            if (Info.HWGfxObjID == 0)
            {
                // destroy
                return false;
            }
            PhysicsObj = PhysicsObj.makeParticleObject(Info.MaxParticles, Info.SortingSphere);
            LastEmitOffset = PhysicsObj.Position.Frame.Origin;
            Parts = PhysicsObj.PartArray.Parts;
            PartStorage = new PhysicsPart[Info.MaxParticles];
            for (var i = 0; i < Info.MaxParticles; i++)
                PartStorage[i] = PhysicsPart.MakePhysicsPart(Info.HWGfxObjID);
            // omitted degrade distance
            DegradeDistance = float.MaxValue;
            Particles = new Particle[Info.MaxParticles];
            for (var i = 0; i < Info.MaxParticles; i++)
                Particles[i] = new Particle();  // ??
            return true;
        }

        public bool SetInfo(uint emitterID)
        {
            return SetInfo(new ParticleEmitterInfo(DBObj.GetParticleEmitterInfo(emitterID)));
        }

        public void EmitParticle()
        {
            var nextIdx = GetNextParticleIdx();
            if (nextIdx == -1)
                return;

            Parts[nextIdx] = PartStorage[nextIdx];
            //if (Parts[nextIdx] == null)   // check if index exists?
                //return;

            var firstParticle = Info.TotalParticles == 0 && Info.TotalSeconds == 0.0f;

            var randomOffset = Info.GetRandomOffset();
            var randomA = Info.GetRandomA();
            var randomB = Info.GetRandomB();
            var randomC = Info.GetRandomC();

            Particles[nextIdx].Init(Info, Parent, PartIndex, ParentOffset, Parts[nextIdx], randomOffset, firstParticle, randomA, randomB, randomC);

            PhysicsObj.AddPartToShadowCells(Parts[nextIdx]);

            RecordParticleEmission();
        }

        public int GetNextParticleIdx()
        {
            for (var i = 0; i < Parts.Count; i++)
                if (Parts[i] == null)
                    return i;
            return -1;
        }

        public bool UpdateParticles()
        {
            if (Info == null || PhysicsObj == null)
                return false;

            if (!PhysicsObj.ShouldDrawParticles(DegradeDistance))
            {
                if (DegradedOut != 0)
                {
                    PhysicsObj.SetNoDraw(true);
                    DegradedOut = 1;
                }
                LastUpdateTime = PhysicsTimer.CurrentTime;
                if (Info.TotalParticles > 0 || Info.TotalSeconds > 0.0f)
                {
                    if (Info.MaxParticles > 0)
                    {
                        for (var i = 0; i < Info.MaxParticles; i++)
                        {
                            var particle = Particles[i];
                            if (particle != null)
                            {
                                particle.Lifetime = PhysicsTimer.CurrentTime - particle.Birthtime;
                                KillParticle(i);
                            }
                        }
                    }
                    if (!Stopped)
                    {
                        if (ShouldEmitParticle())
                            RecordParticleEmission();
                        StopEmitter();
                        return true;
                    }
                    else
                        return NumParticles != 0;
                }
                else
                {
                    if (Info.MaxParticles > 0)
                    {
                        for (var i = 0; i < Info.MaxParticles; i++)
                            Particles[i].Birthtime = PhysicsTimer.CurrentTime;
                    }
                    return true;
                }
            }
            else
            {
                if (DegradedOut != 0)
                {
                    DegradedOut = 0;
                    PhysicsObj.SetNoDraw(false);
                }
                if (Info.MaxParticles > 0)
                {
                    for (var i = 0; i < Info.MaxParticles; i++)
                    {
                        AFrame frame = null;
                        var part = Parts[i];
                        if (part != null)
                        {
                            if (Info.IsParentLocal)
                            {
                                if (PartIndex == -1)
                                    frame = Parent.Position.Frame;
                                else
                                    frame = Parent.PartArray.Parts[PartIndex].Pos.Frame;
                            }
                            else
                                frame = Particles[i].StartFrame;

                            var firstParticle = Info.TotalParticles == 0 && Info.TotalSeconds == 0.0f;
                            Particles[i].Update(Info.ParticleType, firstParticle, part, frame);
                            KillParticle(i);
                        }
                    }
                }
                var hasParticles = true;
                if (!Stopped)
                {
                    if (ShouldEmitParticle())
                        EmitParticle();

                    StopEmitter();
                }
                else
                    hasParticles = NumParticles != 0;

                LastUpdateTime = PhysicsTimer.CurrentTime;

                return hasParticles;
            }
        }

        public void InitEnd()
        {
            CreationTime = PhysicsTimer.CurrentTime;

            for (var i = 0; i < Info.TotalParticles; i++)
                EmitParticle();
        }
    }
}
