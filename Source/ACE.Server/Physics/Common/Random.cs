using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ACE.Server.Physics.Common
{
    public sealed class LazyRandom
    {
        private static readonly Lazy<LazyRandom> lazy =
            new Lazy<LazyRandom>(() => new LazyRandom());

        public static LazyRandom Instance { get { return lazy.Value; } }

        private LazyRandom()
        {
            Seed = Guid.NewGuid().GetHashCode();
        }
        private int Seed = 0;
        private volatile ConcurrentDictionary<int, System.Random> RNGs = new ConcurrentDictionary<int, System.Random>();

        public System.Random RNG
        {
            get
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                if (RNGs.ContainsKey(threadId))
                    return RNGs[threadId];
                else
                {
                    var seed = Interlocked.Add(ref Seed, 1); // possible to overflow here, but incredibly unlikely for current ACE architecture.
                    return RNGs.AddOrUpdate(threadId, new System.Random(seed), (a, b) => b);
                }
            }
        }
    }

    // probably should be moved outside the physics namespace
    // important class, ensure unit tests pass for this
    public class Random
    {
        /// <summary>
        /// Returns a random number between min and max
        /// </summary>
        public static float RollDice(float min, float max)
        {
            // todo: implement exactly the way AC handles it
            // inclusive/exclusive?
            return (float)(LazyRandom.Instance.RNG.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Returns a random integer between min and max, inclusive
        /// </summary>
        /// <param name="min">The minimum possible value to return</param>
        /// <param name="max">The maximum possible value to return</param>
        public static int RollDice(int min, int max)
        {
            return LazyRandom.Instance.RNG.Next(min, max + 1);
        }

        public static uint RollDice(uint min, uint max)
        {
            return (uint)LazyRandom.Instance.RNG.Next((int)min, (int)(max + 1));
        }
    }
}
