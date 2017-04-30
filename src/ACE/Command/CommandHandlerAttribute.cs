using System;

using ACE.Entity.Enum;

namespace ACE.Command
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandHandlerAttribute : Attribute
    {
        public string Command { get; }

        public AccessLevel Access { get; }

        public CommandHandlerFlag Flags { get; }

        public int ParameterCount { get; }

        public string Description { get; }

        public string Usage { get; }

        public CommandHandlerAttribute(string command, AccessLevel access, CommandHandlerFlag flags = CommandHandlerFlag.None, int parameterCount = -1, string description = "", string usage = "")
        {
            Command        = command;
            Access         = access;
            Flags          = flags;
            ParameterCount = parameterCount;
            Description    = description;
            Usage          = usage;
        }

        public CommandHandlerAttribute(string command, AccessLevel access, CommandHandlerFlag flags = CommandHandlerFlag.None, string description = "", string usage = "")
        {
            Command = command;
            Access = access;
            Flags = flags;
            ParameterCount = -1;
            Description = description;
            Usage = usage;
        }

        public CommandHandlerAttribute(string command, AccessLevel access, CommandHandlerFlag flags = CommandHandlerFlag.None)
        {
            Command = command;
            Access = access;
            Flags = flags;
            ParameterCount = -1;
            Description = "";
            Usage = "";
        }
    }
}
