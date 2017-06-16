using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.StateMachines
{
    public class StateMachine
    {
        private List<Rule> validRules;
        
        public int CurrentState { get; private set; }

        public void Initialize(List<Rule> states, int startingstate)
        {
            CurrentState = startingstate;
            validRules = states;
        }

        public bool ChangeState(int too)
        {
            bool valid = false;

            if (validRules.Count == 0)
                throw new ApplicationException("StateMachine must be Initialized before it can used");

            // Look up current state machine state and locate its allowed states.
            List<Rule> currule = new List<Rule>();
            currule = validRules.Where(r => r.FromState == CurrentState).ToList();

            if (currule.Count != 1)
                throw new ApplicationException("StateMachine is in a invalid state");

            // found current state, check if we can move from this state to the next.
            if ((currule[0] as Rule).ToStates.Contains(too))
            {
                CurrentState = too;
                valid = true;
            }

            return valid;
        }
    }
}
