using System;

using ACE.Network.Enum;

namespace ACE.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GameMessageAttribute : Attribute
    {
        public GameMessageOpcode Opcode { get; }
        public SessionState State { get; }

        public GameMessageAttribute(GameMessageOpcode opcode, SessionState state)
        {
            Opcode = opcode;
            State  = state;
        }
    }
}
