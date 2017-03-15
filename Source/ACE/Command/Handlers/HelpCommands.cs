using System;
using System.Reflection;
using System.Text;

using ACE.Network;
using ACE.Entity.Enum;
using System.Collections.Generic;

namespace ACE.Command.Handlers
{
    public static class HelpCommands
    {
        [CommandHandler("acehelp", AccessLevel.Player, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleAceHelpOnClient(Session session, params string[] parameters)
        {
            HelpCommands.HandleAceHelp(session, CommandChannel.TheClientChannel, parameters: parameters);
        }

        [CommandHandler("acecommands", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 0)]
        public static void HandleAceCommandsFromClient(Session session, params string[] parameters)
        {
            HelpCommands.HandleAceCommands(session, CommandChannel.TheClientChannel, parameters);
        }

        /// <summary>
        /// This method handles the acehelp command sent in from admin console or from player chat
        /// </summary>
        public static void HandleAceHelp(Session session, ICommandChannel cmdChannel, params string[] parameters)
        {
            cmdChannel.SendMsg(session, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            cmdChannel.SendMsg(session, "Please use @acecommands for a list of commands.");
        }

        /// <summary>
        /// This method handles the acecommands command sent in from admin console or from player chat
        /// </summary>
        public static void HandleAceCommands(Session session, ICommandChannel cmdChannel, params string[] parameters)
        {
            List<String> commandList = new List<string>();
            if (session != null)
            {
                string msg = string.Format(string.Format("Your Current access level is: {0}", session.AccessLevel));
                cmdChannel.SendMsg(session, msg);
            }
            foreach (var command in CommandManager.GetCommands())
            {
                if (session != null)
                {
                    // Show player commands accessible to current player
                    if (command.Attribute.Flags == CommandHandlerFlag.ConsoleInvoke)
                    {
                        continue;
                    }
                    int playerlvl = (int)session.AccessLevel;
                    int cmdlvl = (int)command.Attribute.Access;
                    if (playerlvl < cmdlvl)
                    {
                        continue;
                    }
                    //adding commands to a string list with @ prefix for use in the client chat window
                    //will show as @commandname (paramcount) (accesslevel) (i.e. @accessLevelCycle (0) (Player)
                    commandList.Add(string.Format("@{0} ({1}) ({2})", command.Attribute.Command, command.Attribute.ParameterCount, command.Attribute.Access));
                }
                else
                {
                    // Show console commands
                    if (command.Attribute.Flags == CommandHandlerFlag.RequiresWorld)
                    {
                        continue;
                    }
                    //adding commands to a string list without the @ prefix not necessary to issue commands from the server console
                    //will show as commandname (paramcount) (accesslevel) (i.e. acehelp (0) (Player)
                    commandList.Add(string.Format("{0} ({1}) ({2})", command.Attribute.Command, command.Attribute.ParameterCount, command.Attribute.Access));
                }
                
            }

            //iterate through all the available commands and send them to the client one at a time.
            for (int i = 0; i < commandList.Count; i++)
            {
                string msg = commandList[i];
                cmdChannel.SendMsg(session, msg);
            }
        }
    }
}
