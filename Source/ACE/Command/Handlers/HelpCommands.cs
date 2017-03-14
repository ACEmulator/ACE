using System;
using System.Reflection;
using System.Text;

using ACE.Network;
using ACE.Entity.Enum;

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
            if (session != null)
            {
                // Solely for development testing
                ChangePlayerAccessLevel(session);
                string msg = string.Format(string.Format("Changed your access level to : {0}", session.AccessLevel));
                cmdChannel.SendMsg(session, msg);
            }
            cmdChannel.SendMsg(session, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            cmdChannel.SendMsg(session, "Please use @acecommands for a list of commands.");
        }

        /// <summary>
        /// Cycle player access level; this only makes sense during development & testing
        /// </summary>
        /// <param name="session"></param>
        private static void ChangePlayerAccessLevel(Session session)
        {
            int lvl = (int)session.AccessLevel;
            lvl = (lvl + 1) % 5;
            session.AccessLevel = (AccessLevel)lvl;
        }
        /// <summary>
        /// This method handles the acecommands command sent in from admin console or from player chat
        /// </summary>
        public static void HandleAceCommands(Session session, ICommandChannel cmdChannel, params string[] parameters)
        {
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
                }
                else
                {
                    // Show console commands
                    if (command.Attribute.Flags == CommandHandlerFlag.RequiresWorld)
                    {
                        continue;
                    }
                }
                string msg = string.Format("@{0} ({1}) ({2})",
                    command.Attribute.Command, command.Attribute.ParameterCount, command.Attribute.Access);
                cmdChannel.SendMsg(session, msg);
            }
        }
    }
}
