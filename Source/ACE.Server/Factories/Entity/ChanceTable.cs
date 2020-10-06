using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;

namespace ACE.Server.Factories.Entity
{
    public class ChanceTable<T> : List<(T result, float chance)>
    {
        public T Roll(float qualityMod = 0.0f)
        {
            var total = 0.0f;

            //var rng = ThreadSafeRandom.NextIntervalMax(qualityMod);
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            foreach (var entry in this)
            {
                total += entry.chance;

                if (rng < total && total >= qualityMod)
                    return entry.result;
            }

            //Console.WriteLine($"Rolled {rng}, everything >= {total}");

            return this.Last(i => i.chance > 0).result;
        }
    }
}
