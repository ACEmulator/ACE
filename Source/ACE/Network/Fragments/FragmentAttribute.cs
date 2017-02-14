using System;

namespace ACE.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FragmentAttribute : Attribute
    {
        public GameMessageOpcode Opcode { get; }
        public SessionState State { get; }

        public FragmentAttribute(GameMessageOpcode opcode, SessionState state)
        {
            Opcode = opcode;
            State  = state;
        }
    }
}
