using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading;

using log4net.Config;

using Microsoft.Extensions.Logging;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Common.Logging;
using ACE.Database;
using ACE.DatLoader;
using ACE.Server.Command;
using ACE.Server.Logging;
using ACE.Server.Managers;
using ACE.Server.Mods;
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
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint MM_BeginPeriod(uint uMilliseconds);

        /// <summary>
        /// The timeEndPeriod function clears a previously set minimum timer resolution
        /// https://docs.microsoft.com/en-us/windows/desktop/api/timeapi/nf-timeapi-timeendperiod
        /// </summary>
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint MM_EndPeriod(uint uMilliseconds);

        private static readonly ILogger log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.ConfigureAndWatch(logRepository, log4netFileInfo);

            // Route Microsoft.Extensions.Logging into log4net. Every logger created so far (static fields
            // initialised before Main) picks this provider up retroactively; log4net.config still governs output.
            LogManager.Factory.AddProvider(new Log4NetBridgeProvider(logRepository));

            if (Environment.ProcessorCount < 2)
                log.LogWarning("Only one vCPU was detected. ACE may run with limited performance. You should increase your vCPU count for anything more than a single player server.");

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
                log.LogError("{Message:l}", ex.ToString());
            }

            log.LogInformation("Starting ACEmulator...");

            if (IsRunningInContainer)
                log.LogInformation("ACEmulator is running in a container...");

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

            log.LogInformation("Initializing ConfigManager...");
            ConfigManager.Initialize();

            log.LogInformation("Initializing ModManager...");
            ModManager.Initialize();

            if (ConfigManager.Config.Server.WorldName != "ACEmulator")
            {
                consoleTitle = $"{ConfigManager.Config.Server.WorldName} | {consoleTitle}";
                Console.Title = consoleTitle;
            }

            if (ConfigManager.Config.Offline.PurgeDeletedCharacters)
            {
                log.LogInformation("Purging deleted characters, and their possessions, older than {PurgeDeletedCharactersDays} days ({AddDays})...", ConfigManager.Config.Offline.PurgeDeletedCharactersDays, DateTime.Now.AddDays(-ConfigManager.Config.Offline.PurgeDeletedCharactersDays).ToCommonString());
                ShardDatabaseOfflineTools.PurgeCharactersInParallel(ConfigManager.Config.Offline.PurgeDeletedCharactersDays, out var charactersPurged, out var playerBiotasPurged, out var possessionsPurged);
                log.LogInformation("Purged {CharactersPurged:N0} characters, {PlayerBiotasPurged:N0} player biotas and {PossessionsPurged:N0} possessions.", charactersPurged, playerBiotasPurged, possessionsPurged);
            }

            if (ConfigManager.Config.Offline.PurgeOrphanedBiotas)
            {
                log.LogInformation("Purging orphaned biotas...");
                ShardDatabaseOfflineTools.PurgeOrphanedBiotasInParallel(out var numberOfBiotasPurged);
                log.LogInformation("Purged {NumberOfBiotasPurged:N0} biotas.", numberOfBiotasPurged);
            }

            if (ConfigManager.Config.Offline.PruneDeletedCharactersFromFriendLists)
            {
                log.LogInformation("Pruning invalid friends from all friend lists...");
                ShardDatabaseOfflineTools.PruneDeletedCharactersFromFriendLists(out var numberOfFriendsPruned);
                log.LogInformation("Pruned {NumberOfFriendsPruned:N0} invalid friends found on friend lists.", numberOfFriendsPruned);
            }

            if (ConfigManager.Config.Offline.PruneDeletedObjectsFromShortcutBars)
            {
                log.LogInformation("Pruning invalid shortcuts from all shortcut bars...");
                ShardDatabaseOfflineTools.PruneDeletedObjectsFromShortcutBars(out var numberOfShortcutsPruned);
                log.LogInformation("Pruned {NumberOfShortcutsPruned:N0} deleted objects found on shortcut bars.", numberOfShortcutsPruned);
            }

            if (ConfigManager.Config.Offline.PruneDeletedCharactersFromSquelchLists)
            {
                log.LogInformation("Pruning invalid squelches from all squelch lists...");
                ShardDatabaseOfflineTools.PruneDeletedCharactersFromSquelchLists(out var numberOfSquelchesPruned);
                log.LogInformation("Pruned {NumberOfSquelchesPruned:N0} invalid squelched characters found on squelch lists.", numberOfSquelchesPruned);
            }

            if (ConfigManager.Config.Offline.AutoServerUpdateCheck)
                CheckForServerUpdate();
            else
                log.LogInformation("AutoServerVersionCheck is disabled...");

            if (ConfigManager.Config.Offline.AutoUpdateWorldDatabase)
            {
                CheckForWorldDatabaseUpdate();

                if (ConfigManager.Config.Offline.AutoApplyWorldCustomizations)
                    AutoApplyWorldCustomizations();
            }
            else
                log.LogInformation("AutoUpdateWorldDatabase is disabled...");

            if (ConfigManager.Config.Offline.AutoApplyDatabaseUpdates)
                AutoApplyDatabaseUpdates();
            else
                log.LogInformation("AutoApplyDatabaseUpdates is disabled...");

            // This should only be enabled manually. To enable it, simply uncomment this line
            //ACE.Database.OfflineTools.Shard.BiotaGuidConsolidator.ConsolidateBiotaGuids(0xA0000000, true, false, out int numberOfBiotasConsolidated, out int numberOfBiotasSkipped, out int numberOfErrors);
            //ACE.Database.OfflineTools.Shard.BiotaGuidConsolidator.ConsolidateBiotaGuids(0xD0000000, false, true, out int numberOfBiotasConsolidated2, out int numberOfBiotasSkipped2, out int numberOfErrors2);

            ShardDatabaseOfflineTools.CheckForBiotaPropertiesPaletteOrderColumnInShard();

            // pre-load starterGear.json, abort startup if file is not found as it is required to create new characters.
            if (Factories.StarterGearFactory.GetStarterGearConfiguration() == null)
            {
                log.LogCritical("Unable to load or parse starterGear.json. ACEmulator will now abort startup.");
                ServerManager.StartupAbort();
                Environment.Exit(0);
            }

            log.LogInformation("Initializing ServerManager...");
            ServerManager.Initialize();

            log.LogInformation("Initializing DatManager...");
            DatManager.Initialize(ConfigManager.Config.Server.DatFilesDirectory, true);

            if (ConfigManager.Config.DDD.EnableDATPatching)
            {
                log.LogInformation("Initializing DDDManager...");
                DDDManager.Initialize();
            }
            else
                log.LogInformation("DAT Patching Disabled...");

            log.LogInformation("Initializing DatabaseManager...");
            DatabaseManager.Initialize();

            if (DatabaseManager.InitializationFailure)
            {
                log.LogCritical("DatabaseManager initialization failed. ACEmulator will now abort startup.");
                ServerManager.StartupAbort();
                Environment.Exit(0);
            }

            log.LogInformation("Starting DatabaseManager...");
            DatabaseManager.Start();

            log.LogInformation("Starting PropertyManager...");
            PropertyManager.Initialize();

            log.LogInformation("Initializing GuidManager...");
            GuidManager.Initialize();

            if (ConfigManager.Config.Server.ServerPerformanceMonitorAutoStart)
            {
                log.LogInformation("Server Performance Monitor auto starting...");
                ServerPerformanceMonitor.Start();
            }

            if (ConfigManager.Config.Server.WorldDatabasePrecaching)
            {
                log.LogInformation("Precaching Weenies...");
                DatabaseManager.World.CacheAllWeenies();
                log.LogInformation("Precaching Cookbooks...");
                DatabaseManager.World.CacheAllCookbooks();
                log.LogInformation("Precaching Events...");
                DatabaseManager.World.GetAllEvents();
                log.LogInformation("Precaching House Portals...");
                DatabaseManager.World.CacheAllHousePortals();
                log.LogInformation("Precaching Points Of Interest...");
                DatabaseManager.World.CacheAllPointsOfInterest();
                log.LogInformation("Precaching Spells...");
                DatabaseManager.World.CacheAllSpells();
                log.LogInformation("Precaching Treasures - Death...");
                DatabaseManager.World.CacheAllTreasuresDeath();
                log.LogInformation("Precaching Treasures - Material Base...");
                DatabaseManager.World.CacheAllTreasureMaterialBase();
                log.LogInformation("Precaching Treasures - Material Groups...");
                DatabaseManager.World.CacheAllTreasureMaterialGroups();
                log.LogInformation("Precaching Treasures - Material Colors...");
                DatabaseManager.World.CacheAllTreasureMaterialColor();
                log.LogInformation("Precaching Treasures - Wielded...");
                DatabaseManager.World.CacheAllTreasureWielded();
            }
            else
                log.LogInformation("Precaching World Database Disabled...");

            log.LogInformation("Initializing PlayerManager...");
            PlayerManager.Initialize();

            log.LogInformation("Initializing HouseManager...");
            HouseManager.Initialize();

            log.LogInformation("Initializing InboundMessageManager...");
            InboundMessageManager.Initialize();

            log.LogInformation("Initializing SocketManager...");
            SocketManager.Initialize();

            log.LogInformation("Initializing WorldManager...");
            WorldManager.Initialize();

            log.LogInformation("Initializing EventManager...");
            EventManager.Initialize();

            // Free up memory before the server goes online. This can free up 6 GB+ on larger servers.
            log.LogInformation("Forcing .net garbage collection...");
            for (int i = 0; i < 10; i++)
            {
                // https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals
                // https://learn.microsoft.com/en-us/dotnet/api/system.runtime.gcsettings.largeobjectheapcompactionmode
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

                GC.Collect();
            }

            // This should be last
            log.LogInformation("Initializing CommandManager...");
            CommandManager.Initialize();

            //Register mod commands
            log.LogInformation("Registering ModManager commands...");
            ModManager.RegisterCommands();
            ModManager.ListMods();

            if (!PropertyManager.GetBool("world_closed", false).Item)
            {
                WorldManager.Open(null);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            log.LogError(exception, "{Message:l}", exception?.Message ?? e.ExceptionObject);
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            if (!IsRunningInContainer)
            {
                if (!ServerManager.ShutdownInitiated)
                    log.LogWarning("Unsafe server shutdown detected! Data loss is possible!");

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
                    log.LogError("{Message:l}", ex.ToString());
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
