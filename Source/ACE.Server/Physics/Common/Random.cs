using System;

namespace ACE.Server.Physics.Common
{
    // probably should be moved outside the physics namespace
    // important class, ensure unit tests pass for this
    public class Random
    {
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
    }
}
