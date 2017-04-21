using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.StateMachines
{
    public static class ContainerRules
    {
        private static bool init = false;

        private static ContainerStates initstate = ContainerStates.Unlocked;

        private static List<Rule> rulesstates = new List<Rule>();

        public static int GetInitState()
        {
            return (int)initstate;
        }

        public static List<Rule> GetRules()
        {
            if (!init)
                GenerateRules();
            return rulesstates;
        }

        private static void GenerateRules()
        {
            // if locked then we can unlock. . etc etc
            rulesstates.Add(new Rule(
                (int)ContainerStates.Locked,        // from
                (int)ContainerStates.Unlocked));    // allowedto
            rulesstates.Add(new Rule(
                (int)ContainerStates.Unlocked,      // from
                (int)ContainerStates.Locked,        // allowedto
                (int)ContainerStates.InUse));       // allowedto
            rulesstates.Add(new Rule(
                (int)ContainerStates.InUse,         // from
                (int)ContainerStates.Locked,        // allowedto
                (int)ContainerStates.Unlocked));    // allowedto
            init = true;
        }
    }
}
