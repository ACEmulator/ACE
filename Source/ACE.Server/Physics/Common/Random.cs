using System;

namespace ACE.Server.Physics.Common
{
    // probably should be moved outside the physics namespace
    // important class, ensure unit tests pass for this
    public class Random
    {
        // multithread support
        [ThreadStatic]
        public static System.Random RNG;

        static Random()
        {
            RNG = new System.Random();
        }

        /// <summary>
        /// Returns a random number between min and max
        /// </summary>
        public static float RollDice(float min, float max)
        {
            // todo: implement exactly the way AC handles it
            // inclusive/exclusive?
            return (float)(RNG.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Returns a random integer between min and max, inclusive
        /// </summary>
        /// <param name="min">The minimum possible value to return</param>
        /// <param name="max">The maximum possible value to return</param>
        public static int RollDice(int min, int max)
        {
            return RNG.Next(min, max + 1);
        }
    }
}
