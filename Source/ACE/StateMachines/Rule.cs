using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
