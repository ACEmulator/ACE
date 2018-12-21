namespace ACE.Common
{
    // important class, ensure unit tests pass for this
    public static class Random
    {
        /// <summary>
        /// Returns a random number between min and max
        /// </summary>
        public static float RollDice(float min, float max)
        {
            // todo: implement exactly the way AC handles it
            // inclusive/exclusive?
            return (float)(new System.Random().NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Returns a random integer between min and max, inclusive
        /// </summary>
        /// <param name="min">The minimum possible value to return</param>
        /// <param name="max">The maximum possible value to return</param>
        public static int RollDice(int min, int max)
        {
            return new System.Random().Next(min, max + 1);
        }

        public static uint RollDice(uint min, uint max)
        {
            return (uint)new System.Random().Next((int)min, (int)(max + 1));
        }
    }
}
