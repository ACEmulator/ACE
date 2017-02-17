using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

using ACE.Network;

namespace ACE.Command
{
    [Flags]
    public enum CommandHandlerFlag
    {
        None          = 0x00,
        ConsoleInvoke = 0x01,
        RequiresWorld = 0x02
    }

    public enum CommandHandlerResponse
    {
        Ok,
        InvalidCommand,
        NoConsoleInvoke,
        InvalidParameterCount,
        NotInWorld
    }

    public class CommandHandlerInfo
    {
        public Delegate Handler { get; set; }
        public CommandHandlerAttribute Attribute { get; set; }
    }

    public delegate void CommandHandler(Session session, params string[] parameters);

    public static class CommandManager
    {
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

                CommandHandlerInfo commandHandler;
                if (GetCommandHandler(null, command, parameters, out commandHandler) == CommandHandlerResponse.Ok)
                    ((CommandHandler)commandHandler.Handler).Invoke(null, parameters);
            }
        }

        public static void ParseCommand(string commandLine, out string command, out string[] parameters)
        {
            var commandSplit = commandLine.Split(' ');
            command    = commandSplit[0];
            parameters = new string[commandSplit.Length - 1];

            Array.Copy(commandSplit, 1, parameters, 0, commandSplit.Length - 1);
        }

        public static CommandHandlerResponse GetCommandHandler(Session session, string command, string[] parameters, out CommandHandlerInfo commandInfo)
        {
            if (!commandHandlers.TryGetValue(command, out commandInfo))
                return CommandHandlerResponse.InvalidCommand;

            if ((commandInfo.Attribute.Flags & CommandHandlerFlag.ConsoleInvoke) == 0 && session == null)
                return CommandHandlerResponse.NoConsoleInvoke;

            if (commandInfo.Attribute.ParameterCount != -1 && parameters.Length < commandInfo.Attribute.ParameterCount)
                return CommandHandlerResponse.InvalidParameterCount;

            if ((commandInfo.Attribute.Flags & CommandHandlerFlag.RequiresWorld) != 0 && (session == null || session.Player == null || !session.Player.InWorld))
                return CommandHandlerResponse.NotInWorld;

            return CommandHandlerResponse.Ok;
        }
    }
}
