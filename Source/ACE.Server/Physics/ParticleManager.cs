using System.Collections.Generic;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics
{
    public class ParticleManager
    {
        public int NextEmitterId;
        public HashSet<ParticleEmitterInfo> ParticleTable;

        public bool CreateBlockingParticleEmitter(PhysicsObj obj, int emitterInfoID, int partIdx, AFrame offset, int emitterID)
        {
            return false;
        }

        public bool CreateParticleEmitter(PhysicsObj obj, int emitterInfoID, int partIdx, AFrame offset, int emitterID)
        {
            return false;
        }

        public int GetNumEmitters()
        {
            return ParticleTable.Count;
        }

        public void UpdateParticles()
        {

        }

        public bool StopParticleEmitter(int emitterID)
        {
            return false;
        }

        public bool DestroyParticleEmitter(int emitterID)
        {
            return false;
        }
    }
}
