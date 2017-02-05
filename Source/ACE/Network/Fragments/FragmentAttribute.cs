using System;

namespace ACE.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FragmentAttribute : Attribute
    {
        public FragmentOpcode Opcode { get; }
        public SessionState State { get; }

        public FragmentAttribute(FragmentOpcode opcode, SessionState state)
        {
            Opcode = opcode;
            State  = state;
        }
    }
}
