using System.Text;

namespace ACE
{
    // important class, ensure unit tests pass for this
    public static class ThreadSafeRandom
    {
        private static readonly object randomMutex = new object();
        private static readonly System.Random random = new System.Random();
        /// <summary>
        /// Returns a random number between min and max
        /// </summary>
        public static float Next(float min, float max)
        {
            // todo: implement exactly the way AC handles it
            // inclusive/exclusive?
            lock (randomMutex)
            {
                return (float)(random.NextDouble() * (max - min) + min);
            }
        }

        /// <summary>
        /// Returns a random integer between min and max, inclusive
        /// </summary>
        /// <param name="min">The minimum possible value to return</param>
        /// <param name="max">The maximum possible value to return</param>
        public static int Next(int min, int max)
        {
            lock (randomMutex)
            {
                return random.Next(min, max + 1);
            }
        }

        public static uint Next(uint min, uint max)
        {
            lock (randomMutex)
            {
                return (uint)random.Next((int)min, (int)(max + 1));
            }
        }

        public static string NextString(string charSelection, int stringLength)
        {
            StringBuilder product = new StringBuilder();
            lock (randomMutex)
            {
                for (int i = 0; i < stringLength; i++)
                {
                    product.Append(charSelection[random.Next(0, charSelection.Length)]);
                }
            }
            return product.ToString();
        }
    }
}
