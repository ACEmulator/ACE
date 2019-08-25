using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Command.Handlers
{
    public static class HelpCommands
    {
        // acehelp (command)
        [CommandHandler("acehelp", AccessLevel.Player, CommandHandlerFlag.None, 0, "Displays help.", "(command)")]
        public static void HandleACEHelp(Session session, params string[] parameters)
        {         
            if (parameters?.Length <= 0)
            {
                if (session != null)
                {
                    var msg = "Note: You may substitute a forward slash (/) for the at symbol (@).\n"
                            + "Use @help to get more information about commands supported by the client.\n"
                            + "Available help:\n"
                            + "@acehelp commands - Lists all commands.\n"
                            + "You can also use @acecommands to get a complete list of the supported ACEmulator commands available to you.\n"
                            + "To get more information about a specific command, use @acehelp command\n";
                    session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                }

                return;
            }

            if (parameters?[0] == "commands") // Mimick @help commands command
            {
                HandleACECommands(session, parameters);
                return;
            }
                
            foreach (var command in CommandManager.GetCommandByName(parameters[0]))
            {
                if (session != null)
                {
                    if (command.Attribute.Flags == CommandHandlerFlag.ConsoleInvoke)
                        continue;
                    if (session.AccessLevel < command.Attribute.Access)
                        continue;

                    var msg = $"@{command.Attribute.Command} - {command.Attribute.Description}\n"
                            + $"Usage: @{command.Attribute.Command} {command.Attribute.Usage}\n";

                    session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

                    return;
                }

                if (command.Attribute.Flags == CommandHandlerFlag.RequiresWorld)
                    continue;
                Console.WriteLine($"{command.Attribute.Command} - {command.Attribute.Description}");
                Console.WriteLine($"Usage: {command.Attribute.Command} {command.Attribute.Usage}");

                return;
            }

            if (session != null)
            {
                var msg = "Use @acecommands to get a complete list of commands available for you to use.\n"
                        + "To get more information about a specific command, use @acehelp command\n";

                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown command: {parameters[0]}", ChatMessageType.Help),
                                            new Network.GameEvent.Events.GameEventWeenieError(session, WeenieError.ThatIsNotAValidCommand),
                                            new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
            }
            else
                Console.WriteLine($"Unknown command: {parameters[0]}");
        }

        // acecommands
        [CommandHandler("acecommands", AccessLevel.Player, CommandHandlerFlag.None, 0, "Lists all commands.", "<access level or search>")]
        public static void HandleACECommands(Session session, params string[] parameters)
        {
            var commandList = new List<string>();

            var msgHeader = "Note: You may substitute a forward slash (/) for the at symbol (@).\n"
                          + "For more information, type @acehelp < command >.\n";

            if (session == null)            
                Console.WriteLine("For more information, type acehelp < command >.");

            var accessLevel = session != null ? session.AccessLevel : AccessLevel.Admin;
            var exact = false;
            string search = null;

            if (parameters.Length > 0)
            {
                var param = parameters[0];
                if (Enum.TryParse(param, true, out AccessLevel pAccessLevel) && pAccessLevel <= accessLevel)
                {
                    accessLevel = pAccessLevel;
                    exact = true;
                }
                else
                    search = param;
            }

            var restrict = session != null ? CommandHandlerFlag.ConsoleInvoke : CommandHandlerFlag.RequiresWorld;

            var commands = from cmd in CommandManager.GetCommands()
                           where (exact ? cmd.Attribute.Access == accessLevel : cmd.Attribute.Access <= accessLevel) && cmd.Attribute.Flags != restrict
                           && (search != null ? $"{cmd.Attribute.Access} {cmd.Attribute.Command} {cmd.Attribute.Description}".Contains(search, StringComparison.OrdinalIgnoreCase) : true)
                           orderby cmd.Attribute.Command
                           select cmd;

            foreach (var command in commands)
                commandList.Add(string.Format("@{0} - {1}", command.Attribute.Command, command.Attribute.Description));

            var msg = string.Join("\n", commandList);

            if (session != null)
                session.Network.EnqueueSend(new GameMessageSystemChat(msgHeader + msg, ChatMessageType.Broadcast));
            else
                Console.WriteLine(msg);
        }
    }
}
