using System;
using System.Collections.Generic;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Server.Network;

namespace ACE.Server.Command.Handlers
{
    public static class ConsoleCommands
    {
        [CommandHandler("version", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Show server version information.", "")]
        public static void ShowVersion(Session session, params string[] parameters)
        {
            var msg = ServerBuildInfo.GetVersionInfo();
            Console.WriteLine(msg);
        }

        [CommandHandler("exit", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Shut down server immediately.", "")]
        public static void Exit(Session session, params string[] parameters)
        {
            AdminShardCommands.ShutdownServerNow(session, parameters);
        }

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
            DatManager.PortalDat.ExtractCategorizedPortalContents(exportDir);
            Console.WriteLine($"Export of portal.dat to {exportDir} complete.");
        }

        [CommandHandler("highres-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Export contents of client_highres.dat file.", "<export-directory-without-spaces>")]
        public static void ExportHighresDatContents(Session session, params string[] parameters)
        {
            if (DatManager.HighResDat == null)
            {
                Console.WriteLine("client_highres.dat file was not loaded.");
                return;
            }
            if (parameters?.Length != 1)
                Console.WriteLine("highres-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting client_highres.dat contents to {exportDir}.  This will take a while.");
            DatManager.HighResDat.ExtractCategorizedPortalContents(exportDir);
            Console.WriteLine($"Export of client_highres.dat to {exportDir} complete.");
        }

        [CommandHandler("language-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 1, "Export contents of client_local_English.dat file.", "<export-directory-without-spaces>")]
        public static void ExportLanguageDatContents(Session session, params string[] parameters)
        {
            if (DatManager.LanguageDat == null)
            {
                Console.WriteLine("client_highres.dat file was not loaded.");
                return;
            }
            if (parameters?.Length != 1)
                Console.WriteLine("language-export <export-directory-without-spaces>");

            string exportDir = parameters[0];

            Console.WriteLine($"Exporting client_local_English.dat contents to {exportDir}.  This will take a while.");
            DatManager.LanguageDat.ExtractCategorizedPortalContents(exportDir);
            Console.WriteLine($"Export of client_local_English.dat to {exportDir} complete.");
        }

        /// <summary>
        /// Export all wav files to a specific directory.
        /// </summary>
        [CommandHandler("wave-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Export Wave Files")]
        public static void ExportWaveFiles(Session session, params string[] parameters)
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

                    wave.ExportWave(exportDir);
                }
            }
            Console.WriteLine($"Export to {exportDir} complete.");
        }

        /// <summary>
        /// Export all texture/image files to a specific directory.
        /// </summary>
        [CommandHandler("image-export", AccessLevel.Admin, CommandHandlerFlag.ConsoleInvoke, 0, "Export Texture/Image Files")]
        public static void ExportImageFile(Session session, params string[] parameters)
        {
            string syntax = "image-export <export-directory-without-spaces> [id]";
            if (parameters?.Length < 1)
            {
                Console.WriteLine(syntax);
                return;
            }

            string exportDir = parameters[0];
            if (exportDir.Length == 0 || !System.IO.Directory.Exists(exportDir))
            {
                Console.WriteLine(syntax);
                return;
            }

            if (parameters.Length > 1)
            {
                uint imageId;
                if (parameters[1].StartsWith("0x"))
                {
                    string hex = parameters[1].Substring(2);
                    if (!uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.CurrentCulture, out imageId))
                    {
                        Console.WriteLine(syntax);
                        return;
                    }
                }
                else
                if (!uint.TryParse(parameters[1], out imageId))
                {
                    Console.WriteLine(syntax);
                    return;
                }

                var image = DatManager.PortalDat.ReadFromDat<Texture>(imageId);
                image.ExportTexture(exportDir);

                Console.WriteLine($"Exported " + imageId.ToString("X8") + " to " + exportDir + ".");
            }
            else
            {
                int portalFiles = 0;
                int highresFiles = 0;
                Console.WriteLine($"Exporting client_portal.dat textures and images to {exportDir}.  This may take a while.");
                foreach (KeyValuePair<uint, DatFile> entry in DatManager.PortalDat.AllFiles)
                {
                    if (entry.Value.GetFileType(DatDatabaseType.Portal) == DatFileType.Texture)
                    {
                        var image = DatManager.PortalDat.ReadFromDat<Texture>(entry.Value.ObjectId);
                        image.ExportTexture(exportDir);
                        portalFiles++;
                    }
                }
                Console.WriteLine($"Exported {portalFiles} total files from client_portal.dat to {exportDir}.");

                if (DatManager.HighResDat != null)
                {
                    foreach (KeyValuePair<uint, DatFile> entry in DatManager.HighResDat.AllFiles)
                    {
                        if (entry.Value.GetFileType(DatDatabaseType.Portal) == DatFileType.Texture)
                        {
                            var image = DatManager.HighResDat.ReadFromDat<Texture>(entry.Value.ObjectId);
                            image.ExportTexture(exportDir);
                            highresFiles++;
                        }
                    }
                    Console.WriteLine($"Exported {highresFiles} total files from client_highres.dat to {exportDir}.");
                }
                int totalFiles = portalFiles + highresFiles;
                Console.WriteLine($"Exported {totalFiles} total files to {exportDir}.");
            }
        }
    }
}
