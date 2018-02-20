using System.Collections.Generic;

namespace ACE.Server.Physics
{
    public class ParticleManager
    {
        public int NextEmitterId;
        public HashSet<ParticleEmitterInfo> ParticleTable;
    }
}
