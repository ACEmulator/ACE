using System;

namespace ACE.Network.GameAction
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameActionAttribute : Attribute
    {
        public GameActionOpcode Opcode { get; }

        public GameActionAttribute(GameActionOpcode opcode)
        {
            Opcode = opcode;
        }
    }
}
