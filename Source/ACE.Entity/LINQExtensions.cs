using System.Collections.Generic;

namespace ACE.Entity
{
    public static class LINQExtensions
    {
        public static ulong Sum(this IEnumerable<ulong> values)
        {
            ulong sum = 0;

            foreach (var value in values)
                sum += value;

            return sum;
        }
    }
}
