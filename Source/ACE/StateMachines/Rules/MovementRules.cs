using System.Collections.Generic;
using ACE.StateMachines.Enum;

namespace ACE.StateMachines.Rules
{
    public static class MovementRules
    {
        private static bool init;

        private static MovementStates initstate = MovementStates.Idle;

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
            Rulesstates.Add(new Rule(
                (int)MovementStates.Idle,         // from
                (int)MovementStates.Moving));     // allowedto
            Rulesstates.Add(new Rule(
                (int)MovementStates.Moving,       // from
                (int)MovementStates.Arrived,      // allowedto
                (int)MovementStates.Abandoned));  // allowedto
            Rulesstates.Add(new Rule(
               (int)MovementStates.Arrived,       // from
               (int)MovementStates.Idle));        // allowedto
            Rulesstates.Add(new Rule(
               (int)MovementStates.Abandoned,     // from
               (int)MovementStates.Idle));        // allowedto
            init = true;
        }
    }
}
