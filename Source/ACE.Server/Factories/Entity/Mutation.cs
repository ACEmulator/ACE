using System.Collections.Generic;

using ACE.Common;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Entity
{
    public class Mutation
    {
        public List<float> Chances = new List<float>();

        public List<MutationOutcome> Outcomes = new List<MutationOutcome>();

        public bool TryMutate(WorldObject wo, int tier, double rng)
        {
            // if at least 6 tiers are defined,
            // if we are rolling for a higher tier,
            // fall back on highest tier?
            if (Chances.Count >= 6 && tier >= Chances.Count)
                tier = Chances.Count - 1;

            // does it pass the roll to mutate for the tier?
            if (tier < 0 || tier >= Chances.Count)
                return false;

            if (rng >= Chances[tier])
                return false;

            // roll again to select the mutations
            rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            var success = true;
            foreach (var outcome in Outcomes)
                success &= outcome.TryMutate(wo, rng);

            return success;
        }
    }
}
