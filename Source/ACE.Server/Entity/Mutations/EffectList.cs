using System.Collections.Generic;

using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Mutations
{
    public class EffectList
    {
        public float Chance;
        public List<Effect> Effects = new List<Effect>();

        public bool TryMutate(WorldObject wo)
        {
            var mutated = false;

            foreach (var effect in Effects)
                mutated |= effect.TryMutate(wo);      // stop completely on failure?

            return mutated;
        }
    }
}
