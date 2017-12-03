using System;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network;
using ACE.DatLoader;
using System.Collections.Generic;
using ACE.Database;

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
                if (entry.Value.GetFileType() == DatFileType.Wave)
                {
                    DatLoader.FileTypes.Wave.ExportWave(entry.Value.ObjectId, exportDir);
                }
            }
            Console.WriteLine($"Export to {exportDir} complete.");
        }

        [CommandHandler("redeploy-world", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke, 1, "Download and redeploy the world content, from github.")]
        public static void RedeployWorld(Session session, params string[] parameters)
        {
            bool forceRedploy = false;
            var sourceSelection = new SourceSelectionOption();
            var userModifiedFlagPresent = DatabaseManager.World.UserModifiedFlagPresent();

            // Match parameters
            if (parameters?.Length > 0)
            {
                foreach (string sourceSelectionItem in System.Enum.GetNames(typeof(SourceSelectionOption)))
                {
                    if (parameters[0].ToLower() == sourceSelectionItem.ToLower())
                    {
                        if (Enum.TryParse(sourceSelectionItem, out sourceSelection))
                            break;
                    }
                }

                string force = parameters[1];
                if (force.Length > 0)
                {
                    if (force.ToLowerInvariant().Contains("force"))
                    {
                        Console.WriteLine("Force redeploy reached!");
                        forceRedploy = true;
                    }
                }
            }
            if (forceRedploy || !userModifiedFlagPresent)
            {
                string errorResult = Database.Redeploy.RedeployDatabaseFromSource(DatabaseSelectionOption.World, sourceSelection);
                if (errorResult == null)
                    Console.WriteLine("The World Database has been deployed!");
                else
                    Console.WriteLine($"There was an error durring your request. {errorResult}");
                return;
            }
            Console.WriteLine("User created content has been detected in the database. Please export the current database or include the 'force' parameter with this command.");
        }

        [CommandHandler("redeploy", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke, 2,
            "Downloads and redeploys database content from github. WARNING: THIS CAN WIPE DATA!",
            "<datbase selection> <source selection> <force>\n\nYou must pass in a database selection and a source selection, but the force string is optional.\nDatabase Selection Options include: None, Authentication, Shard, World, All.\nSource Selections include: LocalDisk and Github.\n\nWARNING: THIS COMMAND MAY RESULT IN LOST DATA!")]
        public static void RedeployAllDatabases(Session session, params string[] parameters)
        {
            if (parameters?.Length < 2 && parameters?.Length > 3)
            {
                Console.WriteLine("Usage: redeploy <datbase selection> <source selection> force");
                return;
            }

            var databaseSelection = new DatabaseSelectionOption();
            var sourceSelection = new SourceSelectionOption();
            bool forceRedploy = false;
            
            if (parameters?.Length > 0)
            {
                // Loop through the enum to attempt at matching the first parameter with an option
                foreach (string dbSelection in System.Enum.GetNames(typeof(DatabaseSelectionOption)))
                {
                    if (parameters[0].ToLower() == dbSelection.ToLower())
                    {
                        if (Enum.TryParse(dbSelection, out databaseSelection))
                            break;
                    }
                }

                // Loop through the enum to attempt at matching the second parameter with an option
                foreach (string sourceSelectionItem in System.Enum.GetNames(typeof(SourceSelectionOption)))
                {
                    if (parameters[1].ToLower() == sourceSelectionItem.ToLower())
                    {
                        if (Enum.TryParse(sourceSelectionItem, out sourceSelection))
                            break;
                    }
                }

                if (parameters?.Length > 2)
                {
                    if (parameters[2].ToLowerInvariant().Contains("force"))
                    {
                        Console.WriteLine("Force redeploy reached!");
                        forceRedploy = true;
                    }
                }
            }
            var userModifiedFlagPresent = DatabaseManager.World.UserModifiedFlagPresent() && (databaseSelection == DatabaseSelectionOption.World || databaseSelection == DatabaseSelectionOption.All) ? true : false;
            if (forceRedploy || !userModifiedFlagPresent)
            {
                string errorResult = Database.Redeploy.RedeployDatabaseFromSource(databaseSelection, sourceSelection);
                // Database.RemoteContentSync.RedeployWorldDatabase();
                if (errorResult == null)
                    Console.WriteLine("All databases have been redeployed!");
                else
                    Console.WriteLine($"There was an error durring your request. {errorResult}");
                return;
            }
            Console.WriteLine("User modified objects were found in the database.\nYou must also pass the 'force' parameter with this command, to start the database reset process.");
        }

        [CommandHandler("download-github-content", AccessLevel.Developer, CommandHandlerFlag.ConsoleInvoke, 0,
            "Downloads content from github.",
            "")]
        public static void DownloadGithubContent(Session session, params string[] parameters)
        {
            Database.Redeploy.GetDataFiles(SourceSelectionOption.Github);
        }
    }
}
