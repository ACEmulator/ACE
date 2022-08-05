using System.IO;

using log4net;

namespace ACE.DatLoader
{
    public static class DatManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string datFile;

        private static int count;

        // End of retail Iteration versions.
        private static int ITERATION_CELL = 982;
        private static int ITERATION_PORTAL = 2072;
        private static int ITERATION_HIRES = 497;
        private static int ITERATION_LANGUAGE = 994;
        public static CellDatDatabase CellDat { get; private set; }

        public static PortalDatDatabase PortalDat { get; private set; }
        public static DatDatabase HighResDat { get; private set; }
        public static LanguageDatDatabase LanguageDat { get; private set; }

        public static void Initialize(string datFileDirectory, bool keepOpen = false, bool loadCell = true)
        {
            var datDir = Path.GetFullPath(Path.Combine(datFileDirectory));

            if (loadCell)
            {
                try
                {
                    datFile = Path.Combine(datDir, "client_cell_1.dat");
                    CellDat = new CellDatDatabase(datFile, keepOpen);
                    count = CellDat.AllFiles.Count;
                    log.Info($"Successfully opened {datFile} file, containing {count} records, iteration {CellDat.Iteration}");
                    if (CellDat.Iteration != ITERATION_CELL)
                        log.Warn($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_CELL}.");
                }
                catch (FileNotFoundException ex)
                {
                    log.Error($"An exception occured while attempting to open {datFile} file!  This needs to be corrected in order for Landblocks to load!");
                    log.Error($"Exception: {ex.Message}");
                }
            }

            try
            {
                datFile = Path.Combine(datDir, "client_portal.dat");
                PortalDat = new PortalDatDatabase(datFile, keepOpen);
                PortalDat.SkillTable.AddRetiredSkills();
                count = PortalDat.AllFiles.Count;
                log.Info($"Successfully opened {datFile} file, containing {count} records, iteration {PortalDat.Iteration}");
                if (PortalDat.Iteration != ITERATION_PORTAL)
                    log.Warn($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_PORTAL}.");
            }
            catch (FileNotFoundException ex)
            {
                log.Error($"An exception occured while attempting to open {datFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.js file. ***\n *** ACE will not run properly without this properly configured! ***\n");
                log.Error($"Exception: {ex.Message}");
            }

            // Load the client_highres.dat file. This is not required for ACE operation, so no exception needs to be generated.
            datFile = Path.Combine(datDir, "client_highres.dat");
            if (File.Exists(datFile))
            {
                HighResDat = new DatDatabase(datFile, keepOpen);
                count = HighResDat.AllFiles.Count;
                log.Info($"Successfully opened {datFile} file, containing {count} records, iteration {HighResDat.Iteration}");
                if (HighResDat.Iteration != ITERATION_HIRES)
                    log.Warn($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_HIRES}.");
            }

            try
            {
                datFile = Path.Combine(datDir, "client_local_English.dat");
                LanguageDat = new LanguageDatDatabase(datFile, keepOpen);
                count = LanguageDat.AllFiles.Count;
                log.Info($"Successfully opened {datFile} file, containing {count} records, iteration {LanguageDat.Iteration}");
                if(LanguageDat.Iteration != ITERATION_LANGUAGE)
                    log.Warn($"{datFile} iteration does not match expected end-of-retail version of {ITERATION_LANGUAGE}.");
            }
            catch (FileNotFoundException ex)
            {
                log.Error($"An exception occured while attempting to open {datFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.json file. ***\n *** ACE will not run properly without this properly configured! ***\n");
                log.Error($"Exception: {ex.Message}");
            }
        }
    }
}
