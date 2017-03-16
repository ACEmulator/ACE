using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using ACE.DatLoader;
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
        }        [CommandHandler("cell-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1)]
        public static void ExportCellDatContents(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
                Console.WriteLine("cell-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting cell.dat contents to {exportDir}.  This can take longer than an hour.");
            DatManager.CellDat.ExtractLandblockContents(exportDir);
            Console.WriteLine($"Export of cell.dat to {exportDir} complete.");
        }

        [CommandHandler("portal-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1)]
        public static void ExportPortalDatContents(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
                Console.WriteLine("portal-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting portal.dat contents to {exportDir}.  This will take a while.");
            DatManager.PortalDat.ExtractCategorizedContents(exportDir);
            Console.WriteLine($"Export of portal.dat to {exportDir} complete.");
        }    }
}
