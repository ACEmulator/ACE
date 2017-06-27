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

        [CommandHandler("loadALB", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Loads all 65k+ Landblocks, Caution.. it takes a very long time")]
        public static void LoadLALB(Session session, params string[] parameters)
        {
            Console.WriteLine($"Loading ALL Landblocks..  This will take a while.  type abortALB to stop");
            LandblockLoader.StartLoading();
        }

        [CommandHandler("abortALB", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Aborts ALL Landblock loading process")]
        public static void AbortLL(Session session, params string[] parameters)
        {
            LandblockLoader.StopLoading();
            Console.WriteLine($"Landblock load aborting.");
        }

        [CommandHandler("loadLB", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Loads Landblock by LandblockId")]
        public static void LoadLandBlock(Session session, params string[] parameters)
        {
            try
            {
                uint rawid;
                if (!uint.TryParse(parameters[0], out rawid))
                    return;

                LandblockId blockid = new LandblockId(rawid);
                LandblockManager.ForceLoadLandBlock(blockid);
            }
            catch
            {
                Console.WriteLine($"Invalid LandblockId");
            }
        }

        [CommandHandler("diag", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Launches Landblock Diagnostic Monitor")]
        public static void Diag(Session session, params string[] parameters)
        {
            Diagnostics.Diagnostics.LandBlockDiag = true;
            Diagnostics.Common.Monitor.ShowDialog();
        }
    }
}
