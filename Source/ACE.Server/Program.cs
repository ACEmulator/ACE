using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

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
    partial class Program
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

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly bool IsRunningInContainer = Convert.ToBoolean(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"));

        public static void Main(string[] args)
        {
            var consoleTitle = $"ACEmulator - v{ServerBuildInfo.FullVersion}";

            Console.Title = consoleTitle;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Typically, you wouldn't force the current culture on an entire application unless you know sure your application is used in a specific region (which ACE is not)
            // We do this because almost all of the client/user input/output code does not take culture into account, and assumes en-US formatting.
            // Without this, many commands that require special characters like , and . will break
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            // Init our text encoding options. This will allow us to use more than standard ANSI text, which the client also supports.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Look for the log4net.config first in the current environment directory, then in the ExecutingAssembly location
            var exeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var containerConfigDirectory = "/ace/Config";
            var log4netConfig = Path.Combine(exeLocation, "log4net.config");
            var log4netConfigExample = Path.Combine(exeLocation, "log4net.config.example");
            var log4netConfigContainer = Path.Combine(containerConfigDirectory, "log4net.config");

            if (IsRunningInContainer && File.Exists(log4netConfigContainer))
                File.Copy(log4netConfigContainer, log4netConfig, true);

            var log4netFileInfo = new FileInfo("log4net.config");
            if (!log4netFileInfo.Exists)
                log4netFileInfo = new FileInfo(log4netConfig);

            if (!log4netFileInfo.Exists)
            {
                var exampleFile = new FileInfo(log4netConfigExample);
                if (!exampleFile.Exists)
                {
                    Console.WriteLine("log4net Configuration file is missing.  Please copy the file log4net.config.example to log4net.config and edit it to match your needs before running ACE.");
                    throw new Exception("missing log4net configuration file");
                }
                else
                {
                    if (!IsRunningInContainer)
                    {
                        Console.WriteLine("log4net Configuration file is missing,  cloning from example file.");
                        File.Copy(log4netConfigExample, log4netConfig);
                    }
                    else
                    {                        
                        if (!File.Exists(log4netConfigContainer))
                        {
                            Console.WriteLine("log4net Configuration file is missing, ACEmulator is running in a container,  cloning from docker file.");
                            var log4netConfigDocker = Path.Combine(exeLocation, "log4net.config.docker");
                            File.Copy(log4netConfigDocker, log4netConfig);
                            File.Copy(log4netConfigDocker, log4netConfigContainer);
                        }
                        else
                        {
                            File.Copy(log4netConfigContainer, log4netConfig);
                        }

                    }
                }
            }

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository, log4netFileInfo);

            if (Environment.ProcessorCount < 2)
                log.Warn("Only one vCPU was detected. ACE may run with limited performance. You should increase your vCPU count for anything more than a single player server.");

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

            if (IsRunningInContainer)
                log.Info("ACEmulator is running in a container...");
            
            var configFile = Path.Combine(exeLocation, "Config.js");
            var configConfigContainer = Path.Combine(containerConfigDirectory, "Config.js");

            if (IsRunningInContainer && File.Exists(configConfigContainer))
                File.Copy(configConfigContainer, configFile, true);

            if (!File.Exists(configFile))
            {
                if (!IsRunningInContainer)
                    DoOutOfBoxSetup(configFile);
                else
                {
                    if (!File.Exists(configConfigContainer))
                    {
                        DoOutOfBoxSetup(configFile);
                        File.Copy(configFile, configConfigContainer);
                    }
                    else
                        File.Copy(configConfigContainer, configFile);
                }
            }

            log.Info("Initializing ConfigManager...");
            ConfigManager.Initialize();

            if (ConfigManager.Config.Server.WorldName != "ACEmulator")
            {
                consoleTitle = $"{ConfigManager.Config.Server.WorldName} | {consoleTitle}";
                Console.Title = consoleTitle;
            }

            if (ConfigManager.Config.Offline.PurgeDeletedCharacters)
            {
                log.Info($"Purging deleted characters, and their possessions, older than {ConfigManager.Config.Offline.PurgeDeletedCharactersDays} days ({DateTime.Now.AddDays(-ConfigManager.Config.Offline.PurgeDeletedCharactersDays)})...");
                ShardDatabaseOfflineTools.PurgeCharactersInParallel(ConfigManager.Config.Offline.PurgeDeletedCharactersDays, out var charactersPurged, out var playerBiotasPurged, out var possessionsPurged);
                log.Info($"Purged {charactersPurged:N0} characters, {playerBiotasPurged:N0} player biotas and {possessionsPurged:N0} possessions.");
            }

            if (ConfigManager.Config.Offline.PurgeOrphanedBiotas)
            {
                log.Info($"Purging orphaned biotas...");
                ShardDatabaseOfflineTools.PurgeOrphanedBiotasInParallel(out var numberOfBiotasPurged);
                log.Info($"Purged {numberOfBiotasPurged:N0} biotas.");
            }

            if (ConfigManager.Config.Offline.PruneDeletedCharactersFromFriendLists)
            {
                log.Info($"Pruning invalid friends from all friend lists...");
                ShardDatabaseOfflineTools.PruneDeletedCharactersFromFriendLists(out var numberOfFriendsPruned);
                log.Info($"Pruned {numberOfFriendsPruned:N0} invalid friends found on friend lists.");
            }

            if (ConfigManager.Config.Offline.PruneDeletedObjectsFromShortcutBars)
            {
                log.Info($"Pruning invalid shortcuts from all shortcut bars...");
                ShardDatabaseOfflineTools.PruneDeletedObjectsFromShortcutBars(out var numberOfShortcutsPruned);
                log.Info($"Pruned {numberOfShortcutsPruned:N0} deleted objects found on shortcut bars.");
            }

            if (ConfigManager.Config.Offline.PruneDeletedCharactersFromSquelchLists)
            {
                log.Info($"Pruning invalid squelches from all squelch lists...");
                ShardDatabaseOfflineTools.PruneDeletedCharactersFromSquelchLists(out var numberOfSquelchesPruned);
                log.Info($"Pruned {numberOfSquelchesPruned:N0} invalid squelched characters found on squelch lists.");
            }

            if (ConfigManager.Config.Offline.AutoServerUpdateCheck)
                CheckForServerUpdate();
            else
                log.Info($"AutoServerVersionCheck is disabled...");

            if (ConfigManager.Config.Offline.AutoUpdateWorldDatabase)
            {
                CheckForWorldDatabaseUpdate();

                if (ConfigManager.Config.Offline.AutoApplyWorldCustomizations)
                    AutoApplyWorldCustomizations();
            }
            else
                log.Info($"AutoUpdateWorldDatabase is disabled...");

            if (ConfigManager.Config.Offline.AutoApplyDatabaseUpdates)
                AutoApplyDatabaseUpdates();
            else
                log.Info($"AutoApplyDatabaseUpdates is disabled...");

            // This should only be enabled manually. To enable it, simply uncomment this line
            //ACE.Database.OfflineTools.Shard.BiotaGuidConsolidator.ConsolidateBiotaGuids(0xA0000000, true, false, out int numberOfBiotasConsolidated, out int numberOfBiotasSkipped, out int numberOfErrors);
            //ACE.Database.OfflineTools.Shard.BiotaGuidConsolidator.ConsolidateBiotaGuids(0xD0000000, false, true, out int numberOfBiotasConsolidated2, out int numberOfBiotasSkipped2, out int numberOfErrors2);

            ShardDatabaseOfflineTools.CheckForBiotaPropertiesPaletteOrderColumnInShard();

            // pre-load starterGear.json, abort startup if file is not found as it is required to create new characters.
            if (Factories.StarterGearFactory.GetStarterGearConfiguration() == null)
            {
                log.Fatal("Unable to load or parse starterGear.json. ACEmulator will now abort startup.");
                ServerManager.StartupAbort();
                Environment.Exit(0);
            }

            log.Info("Initializing ServerManager...");
            ServerManager.Initialize();

            log.Info("Initializing DatManager...");
            DatManager.Initialize(ConfigManager.Config.Server.DatFilesDirectory, true);

            log.Info("Initializing DatabaseManager...");
            DatabaseManager.Initialize();

            if (DatabaseManager.InitializationFailure)
            {
                log.Fatal("DatabaseManager initialization failed. ACEmulator will now abort startup.");
                ServerManager.StartupAbort();
                Environment.Exit(0);
            }

            log.Info("Starting DatabaseManager...");
            DatabaseManager.Start();

            log.Info("Starting PropertyManager...");
            PropertyManager.Initialize();

            log.Info("Initializing GuidManager...");
            GuidManager.Initialize();

            if (ConfigManager.Config.Server.ServerPerformanceMonitorAutoStart)
            {
                log.Info("Server Performance Monitor auto starting...");
                ServerPerformanceMonitor.Start();
            }

            if (ConfigManager.Config.Server.WorldDatabasePrecaching)
            {
                log.Info("Precaching Weenies...");
                DatabaseManager.World.CacheAllWeenies();
                log.Info("Precaching Cookbooks...");
                DatabaseManager.World.CacheAllCookbooks();
                log.Info("Precaching Events...");
                DatabaseManager.World.GetAllEvents();
                log.Info("Precaching House Portals...");
                DatabaseManager.World.CacheAllHousePortals();
                log.Info("Precaching Points Of Interest...");
                DatabaseManager.World.CacheAllPointsOfInterest();
                log.Info("Precaching Spells...");
                DatabaseManager.World.CacheAllSpells();
                log.Info("Precaching Treasures - Death...");
                DatabaseManager.World.CacheAllTreasuresDeath();
                log.Info("Precaching Treasures - Material Base...");
                DatabaseManager.World.CacheAllTreasureMaterialBase();
                log.Info("Precaching Treasures - Material Groups...");
                DatabaseManager.World.CacheAllTreasureMaterialGroups();
                log.Info("Precaching Treasures - Material Colors...");
                DatabaseManager.World.CacheAllTreasureMaterialColor();
                log.Info("Precaching Treasures - Wielded...");
                DatabaseManager.World.CacheAllTreasureWielded();
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

            // Free up memory before the server goes online. This can free up 6 GB+ on larger servers.
            log.Info("Forcing .net garbage collection...");
            for (int i = 0 ; i < 10 ; i++)
                GC.Collect();

            // This should be last
            log.Info("Initializing CommandManager...");
            CommandManager.Initialize();

            if (!PropertyManager.GetBool("world_closed", false).Item)
            {
                WorldManager.Open(null);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error(e.ExceptionObject);
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            if (!IsRunningInContainer)
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
            else
            {
                ServerManager.DoShutdownNow();
                DatabaseManager.Stop();
            }
        }
    }
}
