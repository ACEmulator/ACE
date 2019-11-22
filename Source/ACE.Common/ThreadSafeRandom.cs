using System;

namespace ACE.Common
{
    // important class, ensure unit tests pass for this
    // todo: implement exactly the way AC handles it.. which we'll never know unless we get original source code
    public static class ThreadSafeRandom
    {
        private static readonly object randomMutex = new object();
        private static readonly Random random = new Random();

        /// <summary>
        /// Returns a random floating-point number between min and max, inclusive
        /// </summary>
        /// <param name="min">The minimum possible value to return</param>
        /// <param name="max">The maximum possible value to return</param>
        public static float Next(float min, float max)
        {
            lock (randomMutex)
                return (float)(random.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Returns a random integer between min and max, inclusive
        /// </summary>
        /// <param name="min">The minimum possible value to return</param>
        /// <param name="max">The maximum possible value to return</param>
        public static int Next(int min, int max)
        {
            lock (randomMutex)
                return random.Next(min, max + 1);
        }
    }
}
