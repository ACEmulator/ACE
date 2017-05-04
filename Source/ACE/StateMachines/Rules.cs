using System.Collections.Generic;

namespace ACE.StateMachines
{
    public class Rule
    {
        public int FromState;
        public List<int> ToStates = new List<int>();

        public Rule(int fromstate, params int[] tostates)
        {
            FromState = fromstate;
            foreach (int i in tostates)
                ToStates.Add(i);
        }
    }
}
