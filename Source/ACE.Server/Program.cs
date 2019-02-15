using System;
using System.IO;
using System.Runtime.InteropServices;

using log4net;
using log4net.Config;

using ACE.Common;
using ACE.Database;
using ACE.DatLoader;
using ACE.Server.Command;
using ACE.Server.Managers;
using ACE.Server.Network.Managers;

namespace ACE.Server
{
    class Program
    {
        /// <summary>
        /// The timeBeginPeriod function sets the minimum timer resolution for an application or device driver. Used to manipulate the timer frequency.
        /// https://docs.microsoft.com/en-us/windows/desktop/api/timeapi/nf-timeapi-timebeginperiod
        /// Important note: This function affects a global Windows setting. Windows uses the lowest value (that is, highest resolution) requested by any process.
        /// </summary>
        [DllImport("winmm.dll", EntryPoint="timeBeginPeriod")]
        public static extern uint MM_BeginPeriod(uint uMilliseconds);

        /// <summary>
        /// The timeEndPeriod function clears a previously set minimum timer resolution
        /// https://docs.microsoft.com/en-us/windows/desktop/api/timeapi/nf-timeapi-timeendperiod
        /// </summary>
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint MM_EndPeriod(uint uMilliseconds);

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Init our text encoding options. This will allow us to use more than standard ANSI text, which the client also supports.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            // Do system specific initializations here
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // On many windows systems, the default resolution for Thread.Sleep is 15.6ms. This allows us to command a tighter resolution
                    MM_BeginPeriod(1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

            log.Info("Starting ACEmulator...");
            Console.Title = @"ACEmulator";

            log.Info("Initializing ConfigManager...");
            ConfigManager.Initialize();

            log.Info("Initializing ServerManager...");
            ServerManager.Initialize();

            log.Info("Initializing DatManager...");
            DatManager.Initialize(ConfigManager.Config.Server.DatFilesDirectory, true);

            log.Info("Initializing DatabaseManager...");
            DatabaseManager.Initialize();

            log.Info("Starting DatabaseManager...");
            DatabaseManager.Start();

            log.Info("Starting PropertyManager...");
            PropertyManager.Initialize();

            log.Info("Initializing GuidManager...");
            GuidManager.Initialize();

            if (ConfigManager.Config.Server.WorldDatabasePrecaching)
            {
                log.Info("Precaching Weenies...");
                DatabaseManager.World.CacheAllWeeniesInParallel();
                log.Info("Precaching House Portals...");
                DatabaseManager.World.CacheAllHousePortals();
                log.Info("Precaching Points Of Interest...");
                DatabaseManager.World.CacheAllPointsOfInterest();
                log.Info("Precaching Cookbooks...");
                DatabaseManager.World.CacheAllCookbooksInParallel();
                log.Info("Precaching Spells...");
                DatabaseManager.World.CacheAllSpells();
                log.Info("Precaching Events...");
                DatabaseManager.World.GetAllEvents();
                log.Info("Precaching Death Treasures...");
                DatabaseManager.World.CacheAllDeathTreasures();
                log.Info("Precaching Wielded Treasures...");
                DatabaseManager.World.CacheAllWieldedTreasuresInParallel();
            }
            else
                log.Info("Precaching World Database Disabled...");

            log.Info("Initializing PlayerManager...");
            PlayerManager.Initialize();

            log.Info("Initializing HouseManager...");
            HouseManager.Initialize();

            log.Info("Initializing InboundMessageManager...");
            InboundMessageManager.Initialize();

            log.Info("Initializing SocketManager...");
            SocketManager.Initialize();

            log.Info("Initializing WorldManager...");
            WorldManager.Initialize();

            log.Info("Initializing EventManager...");
            EventManager.Initialize();

            // This should be last
            log.Info("Initializing CommandManager...");
            CommandManager.Initialize();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error(e.ExceptionObject);
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            if (!ServerManager.ShutdownInitiated)
                log.Warn("Unsafe server shutdown detected! Data loss is possible!");

            PropertyManager.StopUpdating();
            DatabaseManager.Stop();

            // Do system specific cleanup here
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    MM_EndPeriod(1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
    }
}
