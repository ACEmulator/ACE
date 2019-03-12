using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using ACE.Entity.Enum;
using ACE.Server.Network;

using log4net;

namespace ACE.Server.Command
{
    public static class CommandManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Dictionary<string, CommandHandlerInfo> commandHandlers;

        public static IEnumerable<CommandHandlerInfo> GetCommands()
        {
            return commandHandlers.Select(p => p.Value);
        }

        public static IEnumerable<CommandHandlerInfo> GetCommandByName(string commandname)
        {
            return commandHandlers.Select(p => p.Value).Where(p => p.Attribute.Command == commandname);
        }

        public static void Initialize()
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
                            Handler = (CommandHandler)Delegate.CreateDelegate(typeof(CommandHandler), method),
                            Attribute = attribute
                        };

                        commandHandlers[attribute.Command] = commandHandler;
                    }
                }
            }

            var thread = new Thread(new ThreadStart(CommandThread));
            thread.Name = "Command Manager";
            thread.IsBackground = true;
            thread.Start();
        }

        public class ConsoleCommandOverallResult
        {
            public CommandHandlerResponse? CommandHandlerResponse { get; set; }
            public ConsoleCommandResult ConsoleCommandResult { get; set; }
        }
        public enum ConsoleCommandResult
        {
            Success,
            ParseError,
            InvocationError,
            CommandHandlerException,
            CommandHandlerResponseError
        }

        /// <summary>
        /// execute a command submitted via the console
        /// </summary>
        /// <param name="commandLine">the submitted command</param>
        /// <returns></returns>
        public static ConsoleCommandOverallResult ConsoleCommand(string commandLine)
        {
            string command = null;
            string[] parameters = null;
            try
            {
                ParseCommand(commandLine, out command, out parameters);
            }
            catch (Exception ex)
            {
                log.Error($"Exception while parsing command: {commandLine}", ex);
                return new ConsoleCommandOverallResult() { ConsoleCommandResult = ConsoleCommandResult.ParseError };
            }
            CommandHandlerResponse chr = CommandHandlerResponse.InvalidCommand;
            CommandHandlerInfo commandHandler = null;
            try
            {
                chr = GetCommandHandler(null, command, parameters, out commandHandler);
            }
            catch (Exception ex)
            {
                log.Error($"Exception while getting command handler for: {commandLine}", ex);
                return new ConsoleCommandOverallResult() { ConsoleCommandResult = ConsoleCommandResult.CommandHandlerException };
            }
            if (chr == CommandHandlerResponse.Ok)
            {
                try
                {
                    // Add command to world manager's main thread...
                    ((CommandHandler)commandHandler.Handler).Invoke(null, parameters);
                    return new ConsoleCommandOverallResult() { ConsoleCommandResult = ConsoleCommandResult.Success, CommandHandlerResponse = chr };
                }
                catch (Exception ex)
                {
                    log.Error($"Exception while invoking command handler for: {commandLine}", ex);
                    return new ConsoleCommandOverallResult() { ConsoleCommandResult = ConsoleCommandResult.InvocationError, CommandHandlerResponse = chr };
                }
            }
            else
            {
                return new ConsoleCommandOverallResult() { ConsoleCommandResult = ConsoleCommandResult.CommandHandlerResponseError, CommandHandlerResponse = chr };
            }
        }

        private static void CommandThread()
        {
            Console.WriteLine("");
            Console.WriteLine("ACEmulator command prompt ready.");
            Console.WriteLine("");
            Console.WriteLine("Type \"acecommands\" for help.");
            Console.WriteLine("");

            for (;;)
            {
                Console.Write("ACE >> ");

                string commandLine = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(commandLine))
                    continue;

                ConsoleCommand(commandLine);
            }
        }

        public static void ParseCommand(string commandLine, out string command, out string[] parameters)
        {
            if (commandLine == "/" || commandLine == "")
            {
                command = null;
                parameters = null;
                return;
            }
            var commandSplit = commandLine.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            command = commandSplit[0];
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
                        listParameters.Add(parameters[start].Replace("\"", ""));
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
            if (command == null || parameters == null)
            {
                commandInfo = null;
                return CommandHandlerResponse.InvalidCommand;
            }
            bool isSUDOauthorized = false;

            if (command.ToLower() == "sudo")
            {
                string sudoCommand = "";
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
                    var sudoParameters = new string[parameters.Length - 1];
                    for (int i = 1; i < parameters.Length; i++)
                        sudoParameters[i - 1] = parameters[i];
                    parameters = sudoParameters;
                }
            }

            if (!commandHandlers.TryGetValue(command, out commandInfo))
            {
                // Provide some feedback for why the console command failed
                if (session == null)
                    Console.WriteLine($"Invalid Command");

                return CommandHandlerResponse.InvalidCommand;
            }

            if ((commandInfo.Attribute.Flags & CommandHandlerFlag.ConsoleInvoke) != 0 && session != null)
                return CommandHandlerResponse.NoConsoleInvoke;

            if (session != null)
            {
                bool isAdvocate = session.Player.IsAdvocate;
                bool isSentinel = session.Player.IsEnvoy; // we map this to envoy
                bool isEnvoy = isSentinel;
                bool isArch = session.Player.IsArch;
                bool isAdmin = session.Player.IsAdmin;

                if (commandInfo.Attribute.Access == AccessLevel.Advocate && !(isAdvocate || isSentinel || isEnvoy || isArch || isAdmin || isSUDOauthorized)
                    || commandInfo.Attribute.Access == AccessLevel.Sentinel && !(isSentinel || isEnvoy || isArch || isAdmin || isSUDOauthorized)
                    || commandInfo.Attribute.Access == AccessLevel.Envoy && !(isEnvoy || isArch || isAdmin || isSUDOauthorized)
                    || commandInfo.Attribute.Access == AccessLevel.Developer && !(isArch || isAdmin || isSUDOauthorized)
                    || commandInfo.Attribute.Access == AccessLevel.Admin && !(isAdmin || isSUDOauthorized))
                    return CommandHandlerResponse.NotAuthorized;
            }

            if (commandInfo.Attribute.ParameterCount != -1 && parameters.Length < commandInfo.Attribute.ParameterCount)
            {
                // Provide some feedback for why the console command failed
                if (session == null)
                    Console.WriteLine($"The syntax of the command is incorrect.\nUsage: " + commandInfo.Attribute.Command + " " + commandInfo.Attribute.Usage);
                return CommandHandlerResponse.InvalidParameterCount;
            }

            if ((commandInfo.Attribute.Flags & CommandHandlerFlag.RequiresWorld) != 0 && (session == null || session.Player == null || session.Player.CurrentLandblock == null))
                return CommandHandlerResponse.NotInWorld;

            if (isSUDOauthorized)
                return CommandHandlerResponse.SudoOk;

            return CommandHandlerResponse.Ok;
        }
    }
}
