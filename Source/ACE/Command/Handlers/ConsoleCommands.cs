using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;

namespace ACE.Command
{
    public static class ConsoleCommands
    {
        // help
        [CommandHandler("acehelp", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0)]
        public static void HandleACEHelp(Session session, params string[] parameters)
        {
            //TODO: ACEHELP output
            Console.WriteLine("TODO: Sorry, still need to implement this command");
        }
    }
}
