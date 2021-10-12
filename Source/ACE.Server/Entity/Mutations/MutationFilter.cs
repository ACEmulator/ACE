using System.Collections.Generic;

using ACE.Common;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity.Mutations
{
    public class MutationFilter
    {
        public List<Mutation> Mutations = new List<Mutation>();

        // MutationFilter -> Mutation -> MutationOutcome -> EffectList -> Effect
        
        public bool TryMutate(WorldObject wo, int tier = 1)
        {
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            var mutated = false;

            foreach (var mutation in Mutations)
                mutated |= mutation.TryMutate(wo, tier, rng);

            return mutated;
        }
    }
}
