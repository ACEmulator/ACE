using System;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network.GameMessages
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
