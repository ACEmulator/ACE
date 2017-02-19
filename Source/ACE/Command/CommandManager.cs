using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

using ACE.Network;

namespace ACE.Command
{
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
            Console.WriteLine("");
            Console.WriteLine("ACEmulator command prompt ready.");
            //Console.WriteLine("Type acehelp and press enter for additional instructions."); TODO: flesh out acehelp to assist new server owners
            Console.WriteLine("");

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

            if ((commandInfo.Attribute.Flags & CommandHandlerFlag.ConsoleInvoke) != 0 && session != null)
                return CommandHandlerResponse.NoConsoleInvoke;

            if (session != null)
            {
                bool isAdvocate = false;
                bool isSentinel = false;
                bool isEnvoy = false;
                bool isArch = false;
                bool isAdmin = false;
                session.Player.PropertiesBool.TryGetValue(PropertyBool.IsAdvocate, out isAdvocate);
                session.Player.PropertiesBool.TryGetValue(PropertyBool.IsSentinel, out isSentinel);
                session.Player.PropertiesBool.TryGetValue(PropertyBool.IsPsr, out isEnvoy);
                session.Player.PropertiesBool.TryGetValue(PropertyBool.IsArch, out isArch);
                session.Player.PropertiesBool.TryGetValue(PropertyBool.IsAdmin, out isAdmin);
                
                // if (commandInfo.Attribute.Access > session.AccessLevel) 
                // TODO: Hook in a "SUDO" command which checks Session.AccessLevel instead of Session.Player.PropertiesBools for accesslevel
                if (commandInfo.Attribute.Access == AccessLevel.Advocate && !(isAdvocate || isSentinel || isEnvoy || isArch || isAdmin)
                    || commandInfo.Attribute.Access == AccessLevel.Sentinel && !(isSentinel || isEnvoy || isArch || isAdmin)
                    || commandInfo.Attribute.Access == AccessLevel.Envoy && !(isEnvoy || isArch || isAdmin)
                    || commandInfo.Attribute.Access == AccessLevel.Developer && !(isArch || isAdmin)
                    || commandInfo.Attribute.Access == AccessLevel.Admin && !(isAdmin))
                    return CommandHandlerResponse.NotAuthorized;
            }

            if (commandInfo.Attribute.ParameterCount != -1 && parameters.Length < commandInfo.Attribute.ParameterCount)
                return CommandHandlerResponse.InvalidParameterCount;

            if ((commandInfo.Attribute.Flags & CommandHandlerFlag.RequiresWorld) != 0 && (session == null || session.Player == null || !session.Player.InWorld))
                return CommandHandlerResponse.NotInWorld;

            return CommandHandlerResponse.Ok;
        }
    }
}
