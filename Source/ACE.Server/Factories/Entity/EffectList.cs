using System.Collections.Generic;

using ACE.Server.WorldObjects;

namespace ACE.Server.Factories.Entity
{
    public class EffectList
    {
        public float Chance;
        public List<Effect> Effects = new List<Effect>();

        public bool TryMutate(WorldObject wo)
        {
            var success = true;

            foreach (var effect in Effects)
                success &= effect.TryMutate(wo);      // stop completely on failure?

            return success;
        }
    }
}
