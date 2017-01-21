using System;

namespace ACE.Command
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandHandlerAttribute : Attribute
    {
        public string Command { get; }
        public int ParameterCount { get; }
        public bool ConsoleInvoke { get; }

        public CommandHandlerAttribute(string command, int parameterCount = -1, bool consoleInvoke = true)
        {
            Command        = command;
            ParameterCount = parameterCount;
            ConsoleInvoke  = consoleInvoke;
        }
    }
}
