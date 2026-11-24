using System.IO;

using Microsoft.Extensions.Logging;

using ACE.Common.Logging;

namespace ACE.DatLoader
{
    public static class DatManager
    {
        private static readonly ILogger log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                    log.LogInformation("Successfully opened {DatFile} file, containing {Count} records, iteration {Iteration}", datFile, count, CellDat.Iteration);
                    if (CellDat.Iteration != ITERATION_CELL)
                        log.LogWarning("{DatFile} iteration does not match expected end-of-retail version of {ITERATION_CELL}.", datFile, ITERATION_CELL);
                }
                catch (FileNotFoundException ex)
                {
                    log.LogError("An exception occured while attempting to open {DatFile} file!  This needs to be corrected in order for Landblocks to load!", datFile);
                    log.LogError("Exception: {Message}", ex.Message);
                }
            }

            try
            {
                datFile = Path.Combine(datDir, "client_portal.dat");
                PortalDat = new PortalDatDatabase(datFile, keepOpen);
                PortalDat.SkillTable.AddRetiredSkills();
                count = PortalDat.AllFiles.Count;
                log.LogInformation("Successfully opened {DatFile} file, containing {Count} records, iteration {Iteration}", datFile, count, PortalDat.Iteration);
                if (PortalDat.Iteration != ITERATION_PORTAL)
                    log.LogWarning("{DatFile} iteration does not match expected end-of-retail version of {ITERATION_PORTAL}.", datFile, ITERATION_PORTAL);
            }
            catch (FileNotFoundException ex)
            {
                log.LogError("An exception occured while attempting to open {DatFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.js file. ***\n *** ACE will not run properly without this properly configured! ***\n", datFile);
                log.LogError("Exception: {Message}", ex.Message);
            }

            // Load the client_highres.dat file. This is not required for ACE operation, so no exception needs to be generated.
            datFile = Path.Combine(datDir, "client_highres.dat");
            if (File.Exists(datFile))
            {
                HighResDat = new DatDatabase(datFile, keepOpen);
                count = HighResDat.AllFiles.Count;
                log.LogInformation("Successfully opened {DatFile} file, containing {Count} records, iteration {Iteration}", datFile, count, HighResDat.Iteration);
                if (HighResDat.Iteration != ITERATION_HIRES)
                    log.LogWarning("{DatFile} iteration does not match expected end-of-retail version of {ITERATION_HIRES}.", datFile, ITERATION_HIRES);
            }

            try
            {
                datFile = Path.Combine(datDir, "client_local_English.dat");
                LanguageDat = new LanguageDatDatabase(datFile, keepOpen);
                count = LanguageDat.AllFiles.Count;
                log.LogInformation("Successfully opened {DatFile} file, containing {Count} records, iteration {Iteration}", datFile, count, LanguageDat.Iteration);
                if(LanguageDat.Iteration != ITERATION_LANGUAGE)
                    log.LogWarning("{DatFile} iteration does not match expected end-of-retail version of {ITERATION_LANGUAGE}.", datFile, ITERATION_LANGUAGE);
            }
            catch (FileNotFoundException ex)
            {
                log.LogError("An exception occured while attempting to open {DatFile} file!\n\n *** Please check your 'DatFilesDirectory' setting in the config.json file. ***\n *** ACE will not run properly without this properly configured! ***\n", datFile);
                log.LogError("Exception: {Message}", ex.Message);
            }
        }
    }
}
