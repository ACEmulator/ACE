using System.Collections.Generic;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics
{
    public class ParticleManager
    {
        public int NextEmitterId;
        public Dictionary<int, ParticleEmitter> ParticleTable;

        public ParticleManager()
        {
            ParticleTable = new Dictionary<int, ParticleEmitter>();
        }

        public int CreateBlockingParticleEmitter(PhysicsObj obj, uint emitterInfoID, int partIdx, AFrame offset, int emitterID)
        {
            if (emitterID != 0)
            {
                if (ParticleTable.ContainsKey(emitterID))
                    return 0;
            }
            return CreateBlockingParticleEmitter(obj, emitterInfoID, partIdx, offset, emitterID);
        }

        public int CreateParticleEmitter(PhysicsObj obj, uint emitterInfoID, int partIdx, AFrame offset, int emitterID)
        {
            if (emitterID != 0)
                ParticleTable.Remove(emitterID);

            var emitter = ParticleEmitter.makeParticleEmitter(obj);

            if (!emitter.SetInfo(emitterInfoID) || !emitter.SetParenting(partIdx, offset))
                return 0;

            emitter.InitEnd();

            var currentID = emitterID;
            if (emitterID == 0)
                currentID = NextEmitterId++;

            emitter.ID = currentID;
            ParticleTable.Add(currentID, emitter);
            return currentID;
        }

        public int GetNumEmitters()
        {
            return ParticleTable.Count;
        }

        public void UpdateParticles()
        {
            var remove = new List<int>();

            foreach (var emitter in ParticleTable)
            {
                if (!emitter.Value.UpdateParticles())
                    remove.Add(emitter.Key);
            }

            foreach (var emitter in remove)
                ParticleTable.Remove(emitter);
        }

        public bool StopParticleEmitter(int emitterID)
        {
            if (emitterID == 0 || !ParticleTable.TryGetValue(emitterID, out var emitter))
                return false;

            emitter.Stopped = true;
            return true;
        }

        public bool DestroyParticleEmitter(int emitterID)
        {
            if (emitterID == 0)
                return false;

            return ParticleTable.Remove(emitterID);
        }
    }
}
