using System;
using System.Linq;
using System.IO;

using ACE.Common;
using log4net;

namespace ACE.DatLoader
{
    public class DatManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static CellDatDatabase cellDat;

        private static PortalDatDatabase portalDat;

        private static string datFile;

        private static int count;

        public static CellDatDatabase CellDat { get { return cellDat; } }

        public static PortalDatDatabase PortalDat { get { return portalDat; } }
        
        public static void Initialize()
        {
            var datDir = Path.GetFullPath(Path.Combine(ConfigManager.Config.Server.DatFilesDirectory));
            try
            {
                datFile = Path.Combine(datDir, "client_cell_1.dat");
                log.Info($"Opening {datFile} file...");
                cellDat = new CellDatDatabase(datFile);
                count = cellDat.AllFiles.Count();
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
                log.Info($"Opening {datFile} file...");
                portalDat = new PortalDatDatabase(datFile);
                count = portalDat.AllFiles.Count();
                log.Info($"Successfully opened {datFile} file, containing {count} records");
            }
            catch (FileNotFoundException ex)
            {
                log.Info($"An exception occured while attempting to open {datFile} file!  This needs to be corrected in order for Landblocks to load!");
                log.Info($"Exception: {ex.Message}");
            }
        }
    }
}
