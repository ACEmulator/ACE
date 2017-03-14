using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using ACE.Command.Handlers;

namespace ACE.Command
{
    public static class ConsoleCommands
    {
        [CommandHandler("acehelp", AccessLevel.Player, CommandHandlerFlag.ConsoleInvoke, 0)]
        public static void HandleAceHelpOnServer(Session session, params string[] parameters)
        {
            HelpCommands.HandleAceHelp(session, CommandChannel.TheServerChannel, parameters);
        }
        [CommandHandler("acecommands", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0)]
        public static void HandleAceCommandsFromServer(Session session, params string[] parameters)
        {
            HelpCommands.HandleAceCommands(session, CommandChannel.TheServerChannel, parameters);
        }
    }
}
