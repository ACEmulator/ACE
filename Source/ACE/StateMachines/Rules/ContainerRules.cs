using System.Collections.Generic;
using ACE.StateMachines.Enum;

namespace ACE.StateMachines.Rules
{
    public static class ContainerRules
    {
        private static bool init = false;

        private static int initState = (int)ContainerStates.Unlocked;

        private static readonly List<Rule> RulesStates = new List<Rule>();

        public static int GetInitState()
        {
            return initState;
        }

        public static List<Rule> GetRules()
        {
            if (!init)
                GenerateRules();
            return RulesStates;
        }

        private static void GenerateRules()
        {
            // if locked then we can unlock. . etc etc
            RulesStates.Add(new Rule(
                (int)ContainerStates.Locked,        // from
                (int)ContainerStates.Unlocked));    // allowedto
            RulesStates.Add(new Rule(
                (int)ContainerStates.Unlocked,      // from
                (int)ContainerStates.Locked,        // allowedto
                (int)ContainerStates.InUse));       // allowedto
            RulesStates.Add(new Rule(
                (int)ContainerStates.InUse,         // from
                (int)ContainerStates.Locked,        // allowedto
                (int)ContainerStates.Unlocked));    // allowedto
            init = true;
        }
    }
}
