using System;

namespace ACE.Network
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FragmentAttribute : Attribute
    {
        public FragmentOpcode Opcode { get; }

        public FragmentAttribute(FragmentOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
