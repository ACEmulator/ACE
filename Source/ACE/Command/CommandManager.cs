using System;
using System.Linq;
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

        public static IEnumerable<CommandHandlerInfo>GetCommands()
        {
            
            return commandHandlers.Select(p => p.Value);
        }

        public static IEnumerable<CommandHandlerInfo> GetConsoleCommands()
        {
            return commandHandlers.Select(p => p.Value).Where(p => p.Attribute.Flags == CommandHandlerFlag.ConsoleInvoke);
        }

        private static string EncodeCommand(string command, CommandHandlerFlag flag) { return command + ":" + flag.ToString(); }
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
                        commandHandlers[EncodeCommand(attribute.Command, attribute.Flags)] = commandHandler;
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

            if (commandLine.Contains("\""))
            {
                var listParameters = new List<string>();

                for (int start = 0; start < parameters.Length; start++)
                    if (!parameters[start].StartsWith("\""))
                        listParameters.Add(parameters[start]);
                    else
                    {
                        listParameters.Add(parameters[start].Replace("\"",""));
                        for (int end = start + 1; end < parameters.Length; end++)
                        {
                            if (!parameters[end].EndsWith("\""))
                                listParameters[start] = listParameters[start] + " " + parameters[end];
                            else
                            {
                                listParameters[start] = listParameters[start] + " " + parameters[end].Replace("\"", "");
                                start = end;
                                break;
                            }
                        }
                    }
                Array.Resize(ref parameters, listParameters.Count);
                parameters = listParameters.ToArray();
            }
        }

        public static CommandHandlerResponse GetCommandHandler(Session session, string command, string[] parameters, out CommandHandlerInfo commandInfo)
        {
            bool isSUDOauthorized = false;

            if (command.ToLower() == "sudo")
            {
                string sudoCommand = "";
                string[] sudoParameters;
                if (parameters.Length > 0)
                    sudoCommand = parameters[0];

                if (!commandHandlers.TryGetValue(sudoCommand, out commandInfo))
                    return CommandHandlerResponse.InvalidCommand;

                if (session == null)
                {
                    Console.WriteLine("SUDO does not work on the console because you already have full access. Remove SUDO from command and execute again.");
                    return CommandHandlerResponse.InvalidCommand;
                }

                if (commandInfo.Attribute.Access <= session.AccessLevel)
                    isSUDOauthorized = true;

                if (isSUDOauthorized)
                {
                    command = sudoCommand;
                    sudoParameters = new string[parameters.Length - 1];
                    for (int i = 1; i < parameters.Length; i++)
                        sudoParameters[i - 1] = parameters[i];
                    parameters = sudoParameters;
                }
            }

            CommandHandlerFlag flag = CommandHandlerFlag.RequiresWorld;
            if (session == null)
            {
                flag = CommandHandlerFlag.ConsoleInvoke;
            }
            if (!commandHandlers.TryGetValue(EncodeCommand(command, flag), out commandInfo))
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
                
                if (commandInfo.Attribute.Access == AccessLevel.Advocate && !(isAdvocate || isSentinel || isEnvoy || isArch || isAdmin || isSUDOauthorized)
                    || commandInfo.Attribute.Access == AccessLevel.Sentinel && !(isSentinel || isEnvoy || isArch || isAdmin || isSUDOauthorized)
                    || commandInfo.Attribute.Access == AccessLevel.Envoy && !(isEnvoy || isArch || isAdmin || isSUDOauthorized)
                    || commandInfo.Attribute.Access == AccessLevel.Developer && !(isArch || isAdmin || isSUDOauthorized)
                    || commandInfo.Attribute.Access == AccessLevel.Admin && !(isAdmin || isSUDOauthorized))
                    return CommandHandlerResponse.NotAuthorized;
            }

            if (commandInfo.Attribute.ParameterCount != -1 && parameters.Length < commandInfo.Attribute.ParameterCount)
                return CommandHandlerResponse.InvalidParameterCount;

            if ((commandInfo.Attribute.Flags & CommandHandlerFlag.RequiresWorld) != 0 && (session == null || session.Player == null || !session.Player.InWorld))
                return CommandHandlerResponse.NotInWorld;

            if (isSUDOauthorized)
                return CommandHandlerResponse.SudoOk;

            return CommandHandlerResponse.Ok;
        }
    }
}
