using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using ACE.DatLoader;

namespace ACE.Command.Handlers
{
    public static class ConsoleCommands
    {
        [CommandHandler("cell-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Export contents of CELL DAT file.", "<export-directory-without-spaces>")]
        public static void ExportCellDatContents(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
                Console.WriteLine("cell-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting cell.dat contents to {exportDir}.  This can take longer than an hour.");
            DatManager.CellDat.ExtractLandblockContents(exportDir);
            Console.WriteLine($"Export of cell.dat to {exportDir} complete.");
        }

        [CommandHandler("portal-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Export contents of PORTAL DAT file.", "<export-directory-without-spaces>")]
        public static void ExportPortalDatContents(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
                Console.WriteLine("portal-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting portal.dat contents to {exportDir}.  This will take a while.");
            DatManager.PortalDat.ExtractCategorizedContents(exportDir);
            Console.WriteLine($"Export of portal.dat to {exportDir} complete.");
        }

        [CommandHandler("masterpreload", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Loads all 65k+ landblocks into memory, Caution.. it takes a very long time")]
        public static void MasterPreload(Session session, params string[] parameters)
        {
            Console.WriteLine($"Master Preload Loading ALL Landblocks..  This will take a while.");
            LandblockManager.LandBockPreload();
            Console.WriteLine($"Master Preload complete.");
        }

        [CommandHandler("diag", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Launches Landblock Diagnostic Monitor")]
        public static void Diag(Session session, params string[] parameters)
        {
            Diagnostics.Diagnostics.LandBlockDiag = true;
            Diagnostics.Common.Monitor.ShowDialog();
        }
    }
}
