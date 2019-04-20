using System;
using ACE.Entity.Enum;

namespace ACE.Server.Command
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

        /// <summary>
        /// include the raw, unparsed command minus the command name as the first parameter
        /// </summary>
        public bool IncludeRaw { get; }

        public CommandHandlerAttribute(string command, AccessLevel access, CommandHandlerFlag flags = CommandHandlerFlag.None, bool includeRaw = false, int parameterCount = -1, string description = "", string usage = "")
        {
            Command = command;
            Access = access;
            Flags = flags;
            ParameterCount = parameterCount;
            Description = description;
            Usage = usage;
            IncludeRaw = includeRaw;
        }

        public CommandHandlerAttribute(string command, AccessLevel access, CommandHandlerFlag flags = CommandHandlerFlag.None, int parameterCount = -1, string description = "", string usage = "")
        {
            Command        = command;
            Access         = access;
            Flags          = flags;
            ParameterCount = parameterCount;
            Description    = description;
            Usage          = usage;
            IncludeRaw     = false;
        }

        public CommandHandlerAttribute(string command, AccessLevel access, CommandHandlerFlag flags = CommandHandlerFlag.None, string description = "", string usage = "")
        {
            Command = command;
            Access = access;
            Flags = flags;
            ParameterCount = -1;
            Description = description;
            Usage = usage;
            IncludeRaw = false;
        }

        public CommandHandlerAttribute(string command, AccessLevel access, CommandHandlerFlag flags = CommandHandlerFlag.None)
        {
            Command = command;
            Access = access;
            Flags = flags;
            ParameterCount = -1;
            Description = "";
            Usage = "";
            IncludeRaw = false;
        }
    }
}
