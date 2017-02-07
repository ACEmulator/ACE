using ACE.Cryptography;
using ACE.Database;
using ACE.Entity;
using ACE.Network;
using System;
using System.Diagnostics;
using System.IO;

namespace ACE.Command
{
    public static class ConsoleCommands
    {
        // help
        [CommandHandler("help", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0)]
        public static void HandleHelp(Session session, params string[] parameters)
        {
            //TODO: HELP output

        }
    }
}
