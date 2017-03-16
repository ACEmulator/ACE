using System;

namespace ACE.Network.GameAction
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GameActionAttribute : Attribute
    {
        public GameActionType Opcode { get; }

        public GameActionAttribute(GameActionType opcode)
        {
            Opcode = opcode;
        }
    }
}
