using System.Collections.Generic;
using ACE.StateMachines.Enum;

namespace ACE.StateMachines.Rules
{
    public static class ContainerRules
    {
        private static bool init = false;

        private static ContainerStates initstate = ContainerStates.Unlocked;

        private static readonly List<Rule> Rulesstates = new List<Rule>();

        public static int GetInitState()
        {
            return (int)initstate;
        }

        public static List<Rule> GetRules()
        {
            if (!init)
                GenerateRules();
            return Rulesstates;
        }

        private static void GenerateRules()
        {
            // if locked then we can unlock. . etc etc
            Rulesstates.Add(new Rule(
                (int)ContainerStates.Locked,        // from
                (int)ContainerStates.Unlocked));    // allowedto
            Rulesstates.Add(new Rule(
                (int)ContainerStates.Unlocked,      // from
                (int)ContainerStates.Locked,        // allowedto
                (int)ContainerStates.InUse));       // allowedto
            Rulesstates.Add(new Rule(
                (int)ContainerStates.InUse,         // from
                (int)ContainerStates.Locked,        // allowedto
                (int)ContainerStates.Unlocked));    // allowedto
            init = true;
        }
    }
}
