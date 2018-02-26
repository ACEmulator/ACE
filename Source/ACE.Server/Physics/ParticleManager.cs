using System.Collections.Generic;

namespace ACE.Server.Physics
{
    public class ParticleManager
    {
        public int NextEmitterId;
        public HashSet<ParticleEmitterInfo> ParticleTable;

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
    }
}
