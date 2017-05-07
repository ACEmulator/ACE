using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.StateMachines
{
    public static class MonsterRules
    {
        private static bool init = false;

        private static MonsterStates initState = MonsterStates.Idle;

        private static List<Rule> ruleStates = new List<Rule>();

        public static int GetInitState()
        {
            return (int)initState;
        }

        public static List<Rule> GetRules()
        {
            if (!init)
                GenerateRules();
            return ruleStates;
        }

        private static void GenerateRules()
        {
            ruleStates.Add(new Rule(
                (int)MonsterStates.Idle,            // from
                (int)MonsterStates.SensePlayer,     // allowedto
                (int)MonsterStates.UnderAttack));   // allowedto
            ruleStates.Add(new Rule(
                (int)MonsterStates.SensePlayer,     // from
                (int)MonsterStates.EnterCombat));   // allowedto
            ruleStates.Add(new Rule(
                (int)MonsterStates.UnderAttack,     // from
                (int)MonsterStates.EnterCombat));   // allowedto
            ruleStates.Add(new Rule(
                (int)MonsterStates.EnterCombat,     // from
                (int)MonsterStates.MoveToPlayer,    // allowedto
                (int)MonsterStates.AttackPlayer));  // allowedto
            ruleStates.Add(new Rule(
                (int)MonsterStates.MoveToPlayer,    // from
                (int)MonsterStates.ExitCombat,      // allowedto
                (int)MonsterStates.AttackPlayer));  // allowedto
            ruleStates.Add(new Rule(
                (int)MonsterStates.AttackPlayer,    // from
                (int)MonsterStates.ExitCombat));    // allowedto
            ruleStates.Add(new Rule(
                (int)MonsterStates.ExitCombat,      // from
                (int)MonsterStates.ReturnToSpawn)); // allowedto
            ruleStates.Add(new Rule(
                (int)MonsterStates.ReturnToSpawn,   // from
                (int)MonsterStates.Idle));          // allowedto
            init = true;
        }
    }
}
