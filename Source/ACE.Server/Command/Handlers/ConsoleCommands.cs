using System;
using System.Collections.Generic;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;

namespace ACE.Server.Command.Handlers
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
            Console.WriteLine($"Landblock load aborting");
            LandblockLoader.StopLoading();
        }

        [CommandHandler("loadLB", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Loads Landblock by LandblockId")]
        public static void LoadLandBlock(Session session, params string[] parameters)
        {
            try
            {
                uint rawid;
                if (!uint.TryParse(parameters[0], out rawid))
                    return;
                LandblockManager.ForceLoadLandBlock(new LandblockId((rawid) << 16));
            }
            catch
            {
                Console.WriteLine($"Invalid LandblockId");
            }
        }

        [CommandHandler("diag", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Launches Landblock Diagnostic Monitor")]
        public static void Diag(Session session, params string[] parameters)
        {
            // TODO: Diagnostics uses WinForms, which is not supported in .net standard/core.
            // TODO: We need a better way to expose diagnostic information moving forward.
            Console.WriteLine("diag has been disabled when we switched to .net standard 2.0 and needs to be reworked");
            // Diagnostics.Diagnostics.LandBlockDiag = true;
            // Diagnostics.Common.Monitor.ShowDialog();
        }

        /// <summary>
        /// Export all wav files to a specific directory.
        /// </summary>
        [CommandHandler("wave-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Export Wave Files")]
        public static void CMT(Session session, params string[] parameters)
        {
            if (parameters?.Length != 1)
            {
                Console.WriteLine("wave-export <export-directory-without-spaces>");
                return;
            }

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting portal.dat WAV files to {exportDir}.  This may take a while.");
            foreach (KeyValuePair<uint, DatFile> entry in DatManager.PortalDat.AllFiles)
            {
                if (entry.Value.GetFileType(DatDatabaseType.Portal) == DatFileType.Wave)
                {
                    var wave = DatManager.PortalDat.ReadFromDat<Wave>(entry.Value.ObjectId);

                    Wave.ExportWave(wave, entry.Value.ObjectId, exportDir);
                }
            }
            Console.WriteLine($"Export to {exportDir} complete.");
        }
    }
}
