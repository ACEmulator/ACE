using ACE.Entity;
using System;

namespace ACE.Command
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandHandlerAttribute : Attribute
    {
        public string Command { get; }
        public AccessLevel Access { get; }
        public CommandHandlerFlag Flags { get; }
        public int ParameterCount { get; }

        public CommandHandlerAttribute(string command, AccessLevel access, CommandHandlerFlag flags = CommandHandlerFlag.None, int parameterCount = -1)
        {
            Command        = command;
            Access         = access;
            Flags          = flags;
            ParameterCount = parameterCount;
        }
    }
}
