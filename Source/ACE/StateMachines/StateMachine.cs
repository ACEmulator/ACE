using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.StateMachines
{
    public class StateMachine
    {
        private List<Rule> validRules;
        public int CurrentState { get; private set; }

        public void Initialise(List<Rule> states, int startingState)
        {
            CurrentState = startingState;
            validRules = states;
        }

        public bool ChangeState(int too)
        {
            bool valid = false;

            if (validRules.Count == 0)
                throw new ApplicationException("StateMachine must be initialised before it can be used");

            // Look up current state machine state and locate its allowed states.
            List<Rule> curRule = new List<Rule>();
            curRule = validRules.Where(r => r.FromState == CurrentState).ToList();

            if (curRule.Count != 1)
                throw new ApplicationException("StateMachine is in an invalid state");

            // found current state, check if we can move from this state to the next.
            if ((curRule[0] as Rule).ToStates.Contains(too))
            {
                CurrentState = too;
                valid = true;
            }

            return valid;
        }
    }
}
