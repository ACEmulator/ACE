using System.Collections.Generic;
using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public class StaminaCost
    {
        public int Burden;
        public float Stamina;

        public StaminaCost(int burden, float stamina)
        {
            Burden = burden;
            Stamina = stamina;
        }
    }

    public static class StaminaTable
    {
        public static Dictionary<PowerAccuracy, List<StaminaCost>> Costs;

        static StaminaTable()
        {
            BuildTable();
        }

        public static void BuildTable()
        {
            Costs = new Dictionary<PowerAccuracy, List<StaminaCost>>();

            // must be in descending order
            var lowCosts = new List<StaminaCost>();
            lowCosts.Add(new StaminaCost(1600, 1.5f));
            lowCosts.Add(new StaminaCost(1200, 1));
            lowCosts.Add(new StaminaCost(700, 1));

            var midCosts = new List<StaminaCost>();
            midCosts.Add(new StaminaCost(1600, 3));
            midCosts.Add(new StaminaCost(1200, 2));
            midCosts.Add(new StaminaCost(700, 1));

            var highCosts = new List<StaminaCost>();
            highCosts.Add(new StaminaCost(1600, 6));
            highCosts.Add(new StaminaCost(1200, 4));
            highCosts.Add(new StaminaCost(700, 2));

            Costs.Add(PowerAccuracy.Low, lowCosts);
            Costs.Add(PowerAccuracy.Medium, midCosts);
            Costs.Add(PowerAccuracy.High, highCosts);
        }

        public static float GetStaminaCost(PowerAccuracy powerAccuracy, int burden)
        {
            var baseCost = 0.0f;
            var attackCosts = Costs[powerAccuracy];
            foreach (var attackCost in attackCosts)
            {
                if (burden >= attackCost.Burden)
                {
                    var numTimes = burden / attackCost.Burden;
                    baseCost += attackCost.Stamina * numTimes;
                    burden -= attackCost.Burden * numTimes;
                }
            }
            return baseCost;
        }
    }
}
