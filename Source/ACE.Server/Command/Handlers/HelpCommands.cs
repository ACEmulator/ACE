using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Entity.Enum;
using ACE.Server.Managers;
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
                    session.Network.EnqueueSend(new GameMessageSystemChat("Note: You may substitute a forward slash (/) for the at symbol (@).", ChatMessageType.Broadcast));
                    session.Network.EnqueueSend(new GameMessageSystemChat("Use @help to get more information about commands supported by the client.", ChatMessageType.Broadcast));
                    session.Network.EnqueueSend(new GameMessageSystemChat("Available help:", ChatMessageType.Broadcast));
                    session.Network.EnqueueSend(new GameMessageSystemChat("@acehelp commands - Lists all commands.", ChatMessageType.Broadcast));
                    session.Network.EnqueueSend(new GameMessageSystemChat("You can also use @acecommands to get a complete list of the supported ACEmulator commands available to you.", ChatMessageType.Broadcast));
                    session.Network.EnqueueSend(new GameMessageSystemChat("To get more information about a specific command, use @acehelp command", ChatMessageType.Broadcast));
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
                    session.Network.EnqueueSend(new GameMessageSystemChat($"@{command.Attribute.Command} - {command.Attribute.Description}", ChatMessageType.Broadcast));
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Usage: @{command.Attribute.Command} {command.Attribute.Usage}", ChatMessageType.Broadcast));

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
                session.Network.EnqueueSend(new GameMessageSystemChat($"Unknown command: {parameters[0]}", ChatMessageType.Help));
                session.Network.EnqueueSend(new GameMessageSystemChat("Use @acecommands to get a complete list of commands available for you to use.", ChatMessageType.Broadcast));
                session.Network.EnqueueSend(new GameMessageSystemChat("To get more information about a specific command, use @acehelp command", ChatMessageType.Broadcast));
            }
            else
                Console.WriteLine($"Unknown command: {parameters[0]}");
        }

        // acecommands
        [CommandHandler("acecommands", AccessLevel.Player, CommandHandlerFlag.None, 0, "Lists all commands.")]
        public static void HandleACECommands(Session session, params string[] parameters)
        {
            List<String> commandList = new List<string>();

            if (session != null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("Note: You may substitute a forward slash (/) for the at symbol (@).", ChatMessageType.Broadcast));
                session.Network.EnqueueSend(new GameMessageSystemChat("For more information, type @acehelp < command >.", ChatMessageType.Broadcast));
            }
            else
                Console.WriteLine("For more information, type acehelp < command >.");

            foreach (var command in CommandManager.GetCommands())
            {
                if (session != null)
                {
                    if (command.Attribute.Flags == CommandHandlerFlag.ConsoleInvoke) // Skip Console Commands
                        continue;
                    if (session.AccessLevel < command.Attribute.Access) // Skip Commands which are higher than your current access level
                        continue;

                    commandList.Add(string.Format("@{0} - {1}", command.Attribute.Command, command.Attribute.Description));
                }
                else
                {
                    if (command.Attribute.Flags == CommandHandlerFlag.RequiresWorld) // Skip Commands that only work in game
                        continue;

                    commandList.Add(string.Format("{0} - {1}", command.Attribute.Command, command.Attribute.Description));
                }
            }

            commandList.Sort();

            for (int i = 0; i < commandList.Count; i++)
            {
                string message = commandList[i];
                if (session != null)
                    session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Broadcast));
                else
                    Console.WriteLine(message);
            }
        }
    }
}
