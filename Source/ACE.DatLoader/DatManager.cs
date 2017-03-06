using System;
using System.Linq;
using System.IO;

using ACE.Common;

namespace ACE.DatLoader
{
    public class DatManager
    {
        public static DatDatabase celldat;
        public static DatDatabase portaldat;

        private static string datFile;
        private static int count;

        public static void Initialise()
        {
            try
            {
                datFile = ConfigManager.Config.Server.DatFilesDirectory + "\\client_cell_1.dat";
                celldat = new DatDatabase(datFile);
                count = celldat.AllFiles.Count();
                Console.WriteLine($"Successfully opened {datFile} file, containing {count} records");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"An exception occured while attempting to open {datFile} file!");
                Console.WriteLine($"Exception: {ex.Message}");
                Environment.Exit(-1);
            }

            try
            {
                datFile = ConfigManager.Config.Server.DatFilesDirectory + "\\client_portal.dat";
                portaldat = new DatDatabase(datFile);
                count = portaldat.AllFiles.Count();
                Console.WriteLine($"Successfully opened {datFile} file, containing {count} records");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"An exception occured while attempting to open {datFile} file!");
                Console.WriteLine($"Exception: {ex.Message}");
                Environment.Exit(-1);
            }
        }
    }
}