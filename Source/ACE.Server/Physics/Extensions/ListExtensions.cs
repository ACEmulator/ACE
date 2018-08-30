using System.Collections.Generic;
using System.Linq;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.RollDice(0, n);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static float Product(this IList<float> list)
        {
            var totalProduct = 1.0f;
            foreach (var item in list)
                totalProduct *= item;

            return totalProduct;
        }
    }
}
