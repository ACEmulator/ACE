﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.StateMachines
{
    public class StateMachine
    {
        private List<Rule> validrules;
        public int CurrentState { get; private set; }

        public void Initialize(List<Rule> states, int startingstate)
        {
            CurrentState = startingstate;
            validrules = states;
        }

        public bool ChangeState(int too)
        {
            bool valid = false;

            if (validrules.Count == 0)
                throw new ApplicationException("StateMachine must be Initialized before it can used");

                // Look up current state machine state and locate its allowed states.
                List<Rule> currule = new List<Rule>();
                currule = validrules.Where(r => r.FromState == CurrentState).ToList();

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
