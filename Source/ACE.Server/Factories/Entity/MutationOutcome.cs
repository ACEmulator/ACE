using System.Collections.Generic;

using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Entity
{
    public class MutationOutcome
    {
        public List<EffectList> EffectLists = new List<EffectList>();

        public bool TryMutate(WorldObject wo, double rng)
        {
            foreach (var effectList in EffectLists)
            {
                if (rng < effectList.Chance)
                    return effectList.TryMutate(wo);
            }
            return false;
        }
    }
}
