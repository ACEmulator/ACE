using System.Collections.Generic;

namespace ACE.Server.Entity
{
    public class Probability
    {
        /// <summary>
        /// Returns the probability of none of the events occurring
        /// from a list of chances
        /// </summary>
        public static float GetProbabilityNone(List<float> chances)
        {
            var probability = 1.0f;

            foreach (var chance in chances)
                probability *= 1.0f - chance;

            return probability;
        }

        /// <summary>
        /// Returns the probability of any of the events occurring
        /// from a list of chances
        /// </summary>
        public static float GetProbabilityAny(List<float> chances)
        {
            return 1.0f - GetProbabilityNone(chances);
        }
    }
}
