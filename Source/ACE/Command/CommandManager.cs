using ACE.Network;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace ACE.Command
{
    public class CommandHandlerInfo
    {
        public Delegate Handler { get; set; }
        public CommandHandlerAttribute Attribute { get; set; }
    }

    public static class CommandManager
    {
        delegate void CommandHandler(Session session, params string[] parameters);
        private static Dictionary<string, CommandHandlerInfo> commandHandlers;

        public static void Initialise()
        {
            commandHandlers = new Dictionary<string, CommandHandlerInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var method in type.GetMethods())
                {
                    foreach (var attribute in method.GetCustomAttributes<CommandHandlerAttribute>())
                    {
                        var commandHandler = new CommandHandlerInfo()
                        {
                            Handler   = (CommandHandler)Delegate.CreateDelegate(typeof(CommandHandler), method),
                            Attribute = attribute
                        };

                        commandHandlers[attribute.Command] = commandHandler;
                    }
                }
            }

            var thread = new Thread(new ThreadStart(CommandThread));
            thread.IsBackground = true;
            thread.Start();
        }

        private static void CommandThread()
        {
            for (;;)
            {
                Console.Write("ACE >> ");

                string commandLine = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(commandLine))
                    continue;

                string command;
                string[] parameters;
                ParseCommand(commandLine, out command, out parameters);

                var commandHandler = GetCommandHandler(null, command, parameters);
                if (commandHandler != null)
                    ((CommandHandler)commandHandler.Handler).Invoke(null, parameters);
            }
        }

        private static void ParseCommand(string commandLine, out string command, out string[] parameters)
        {
            var commandSplit = commandLine.Split(' ');
            command    = commandSplit[0];
            parameters = new string[commandSplit.Length - 1];

            Array.Copy(commandSplit, 1, parameters, 0, commandSplit.Length - 1);
        }

        private static CommandHandlerInfo GetCommandHandler(Session session, string command, string[] parameters)
        {
            CommandHandlerInfo commandInfo;
            if (!commandHandlers.TryGetValue(command, out commandInfo))
                return null;

            if (session == null && !commandInfo.Attribute.ConsoleInvoke)
                return null;

            if (commandInfo.Attribute.ParameterCount != -1 && parameters.Length < commandInfo.Attribute.ParameterCount)
                return null;

            return commandInfo;
        }
    }
}
