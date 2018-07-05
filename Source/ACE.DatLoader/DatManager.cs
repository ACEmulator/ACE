using System.IO;

using log4net;

namespace ACE.DatLoader
{
    public static class DatManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string datFile;

        private static int count;

        public static CellDatDatabase CellDat { get; private set; }

        public static PortalDatDatabase PortalDat { get; private set; }
        public static PortalDatDatabase HighResDat { get; private set; }
        public static PortalDatDatabase LanguageDat { get; private set; }

        public static void Initialize(string datFileDirectory)
        {
            var datDir = Path.GetFullPath(Path.Combine(datFileDirectory));

            try
            {
                datFile = Path.Combine(datDir, "client_cell_1.dat");
                CellDat = new CellDatDatabase(datFile);
                count = CellDat.AllFiles.Count;
                log.Info($"Successfully opened {datFile} file, containing {count} records");
            }
            catch (FileNotFoundException ex)
            {
                log.Info($"An exception occured while attempting to open {datFile} file!  This needs to be corrected in order for Landblocks to load!");
                log.Info($"Exception: {ex.Message}");
            }

            try
            {
                datFile = Path.Combine(datDir, "client_portal.dat");
                PortalDat = new PortalDatDatabase(datFile);
                count = PortalDat.AllFiles.Count;
                log.Info($"Successfully opened {datFile} file, containing {count} records");
            }
            catch (FileNotFoundException ex)
            {
                log.Info($"An exception occured while attempting to open {datFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.json file. ***\n *** ACE will not run properly without this properly configured! ***\n");
                log.Info($"Exception: {ex.Message}");
            }

            // Load the client_highres.dat file. This is not required for ACE operation, so no exception needs to be generated.
            datFile = Path.Combine(datDir, "client_highres.dat");
            if (File.Exists(datFile))
            { 
                HighResDat = new PortalDatDatabase(datFile);
                count = HighResDat.AllFiles.Count;
                log.Info($"Successfully opened {datFile} file, containing {count} records");
            }

            // Load the client_local_English.dat file. This is not required for ACE operation, so no exception needs to be generated.
            datFile = Path.Combine(datDir, "client_local_English.dat");
            if (File.Exists(datFile))
            {
                LanguageDat = new PortalDatDatabase(datFile);
                count = LanguageDat.AllFiles.Count;
                log.Info($"Successfully opened {datFile} file, containing {count} records");
            }

        }
    }
}
