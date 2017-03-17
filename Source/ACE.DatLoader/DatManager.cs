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

        static DatManager()
        {
            try
            {
                datFile = ConfigManager.Config.Server.DatFilesDirectory + "\\client_cell_1.dat";
                cellDat = new CellDatDatabase(datFile);
                count = cellDat.AllFiles.Count();
                log.Info($"Successfully opened {datFile} file, containing {count} records");
            }
            catch (FileNotFoundException ex)
            {
                log.Info($"An exception occured while attempting to open {datFile} file!");
                log.Info($"Exception: {ex.Message}");
                Environment.Exit(-1);
            }

            try
            {
                datFile = ConfigManager.Config.Server.DatFilesDirectory + "\\client_portal.dat";
                portalDat = new PortalDatDatabase(datFile);
                count = portalDat.AllFiles.Count();
                log.Info($"Successfully opened {datFile} file, containing {count} records");
            }
            catch (FileNotFoundException ex)
            {
                log.Info($"An exception occured while attempting to open {datFile} file!");
                log.Info($"Exception: {ex.Message}");
                Environment.Exit(-1);
            }
        }
    }
}
