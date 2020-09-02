using System.Collections.Generic;
using System.Linq;

using ACE.Common;

namespace ACE.Server.Factories.Entity
{
    public class ChanceTable<T>
    {
        private readonly List<(T result, float chance)> table = new List<(T, float)>();

        public void Add(T result, float chance)
        {
            table.Add((result, chance));
        }

        public T Roll()
        {
            var total = 0.0f;

            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            foreach (var entry in table)
            {
                total += entry.chance;

                if (rng < total)
                    return entry.result;
            }
            return table.Last().result;
        }
    }
}
