extern alias MySqlConnectorAlias;

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
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

using DouglasCrockford.JsMin;
using Newtonsoft.Json;

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

            // Typically, you wouldn't force the current culture on an entire application unless you know sure your application is used in a specific region (which ACE is not)
            // We do this because almost all of the client/user input/output code does not take culture into account, and assumes en-US formatting.
            // Without this, many commands that require special characters like , and . will break
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            // Init our text encoding options. This will allow us to use more than standard ANSI text, which the client also supports.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Look for the log4net.config first in the current environment directory, then in the ExecutingAssembly location
            var exeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var log4netConfig = Path.Combine(exeLocation, "log4net.config");
            var log4netConfigExample = Path.Combine(exeLocation, "log4net.config.example");
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
                    Console.WriteLine("log4net Configuration file is missing,  cloning from example file.");
                    File.Copy(log4netConfigExample, log4netConfig);
                }
            }

            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, log4netFileInfo);

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
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var serverVersion = fileVersionInfo.ProductVersion;
            Console.Title = @$"ACEmulator - v{serverVersion}";

            var configFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Config.js");
            if (!File.Exists(configFile))
            {
                DoOutOfBoxSetup(configFile);
            }

            log.Info("Initializing ConfigManager...");
            ConfigManager.Initialize();

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

            if (ConfigManager.Config.Server.ServerPerformanceMonitorAutoStart)
            {
                log.Info("Server Performance Monitor auto starting...");
                ServerPerformanceMonitor.Start();
            }

            if (ConfigManager.Config.Server.WorldDatabasePrecaching)
            {
                log.Info("Precaching Weenies...");
                DatabaseManager.World.CacheAllWeeniesInParallel();
                log.Info("Precaching House Portals...");
                DatabaseManager.World.CacheAllHousePortals();
                log.Info("Precaching Points Of Interest...");
                DatabaseManager.World.CacheAllPointsOfInterest();
                log.Info("Precaching Spells...");
                DatabaseManager.World.CacheAllSpells();
                log.Info("Precaching Events...");
                DatabaseManager.World.GetAllEvents();
                log.Info("Precaching Death Treasures...");
                DatabaseManager.World.CacheAllDeathTreasures();
                log.Info("Precaching Wielded Treasures...");
                DatabaseManager.World.CacheAllWieldedTreasuresInParallel();
                log.Info("Precaching Treasure Materials...");
                DatabaseManager.World.CacheAllTreasuresMaterialBaseInParallel();
                DatabaseManager.World.CacheAllTreasuresMaterialGroupsInParallel();
                log.Info("Precaching Treasure Colors...");
                DatabaseManager.World.CacheAllTreasuresMaterialColorInParallel();
                log.Info("Precaching Cookbooks...");
                DatabaseManager.World.CacheAllCookbooksInParallel();
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

        private static void DoOutOfBoxSetup(string configFile)
        {
            var exampleFile = new FileInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.js.example"));
            if (!exampleFile.Exists)
            {
                log.Error("config.js.example Configuration file is missing.  Please copy the file config.js.example to config.js and edit it to match your needs before running ACE.");
                throw new Exception("missing config.js configuration file");
            }
            else
            {
                Console.WriteLine("config.js Configuration file is missing,  cloning from example file.");
                File.Copy(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.js.example"), configFile, true);
            }

            var fileText = File.ReadAllText(configFile);
            var config = JsonConvert.DeserializeObject<MasterConfiguration>(new JsMinifier().Minify(fileText));

            Console.WriteLine("Performing setup for ACEmulator...");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Welcome to ACEmulator! To configure your world for first use, please follow the instructions below. Press enter at each prompt to accept default values.");
            Console.WriteLine();
            Console.WriteLine();

            Console.Write($"Enter the name for your World (default: \"{config.Server.WorldName}\"): ");
            var variable = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(variable))
                config.Server.WorldName = variable.Trim();
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("The next two entries should use defaults, unless you have specific network environments...");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write($"Enter the Host address for your World (default: \"{config.Server.Network.Host}\"): ");
            variable = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(variable))
                config.Server.Network.Host = variable.Trim();
            Console.WriteLine();

            Console.Write($"Enter the Port for your World (default: \"{config.Server.Network.Port}\"): ");
            variable = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(variable))
                config.Server.Network.Port = Convert.ToUInt32(variable.Trim());
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine();

            Console.Write($"Enter the directory location for your DAT files (default: \"{config.Server.DatFilesDirectory}\"): ");
            variable = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(variable))
            {
                var path = Path.GetFullPath(variable.Trim());
                if (!Path.EndsInDirectorySeparator(path))
                    path += Path.DirectorySeparatorChar;
                //path = path.Replace($"{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}");

                config.Server.DatFilesDirectory = path;
            }
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Next we will configure your SQL server connections. You will need to know your database name, username and password for each.");
            Console.WriteLine("Default names for the databases are recommended, and it is also recommended you not use root for login to database. The password must not be blank.");
            Console.WriteLine("It is also recommended the SQL server be hosted on the same machine as this server, so defaults for Host and Port would be ideal as well.");
            Console.WriteLine("As before, pressing enter will use default value.");
            Console.WriteLine();
            Console.WriteLine();

            Console.Write($"Enter the database name for your authentication database (default: \"{config.MySql.Authentication.Database}\"): ");
            variable = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(variable))
                config.MySql.Authentication.Database = variable.Trim();
            Console.WriteLine();

            Console.Write($"Enter the database name for your shard database (default: \"{config.MySql.Shard.Database}\"): ");
            variable = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(variable))
                config.MySql.Shard.Database = variable.Trim();
            Console.WriteLine();

            Console.Write($"Enter the database name for your world database (default: \"{config.MySql.World.Database}\"): ");
            variable = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(variable))
                config.MySql.World.Database = variable.Trim();
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Typically, all three databases will be on the same SQL server, is this how you want to proceed? (Y/n) ");
            variable = Console.ReadLine();
            if (!variable.Equals("n", StringComparison.OrdinalIgnoreCase) && !variable.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write($"Enter the Host address for your SQL server (default: \"{config.MySql.World.Host}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                {
                    config.MySql.Authentication.Host = variable.Trim();
                    config.MySql.Shard.Host = variable.Trim();
                    config.MySql.World.Host = variable.Trim();
                }
                Console.WriteLine();

                Console.Write($"Enter the Port for your SQL server (default: \"{config.MySql.World.Port}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                {
                    config.MySql.Authentication.Port = Convert.ToUInt32(variable.Trim());
                    config.MySql.Shard.Port = Convert.ToUInt32(variable.Trim());
                    config.MySql.World.Port = Convert.ToUInt32(variable.Trim());
                }
                Console.WriteLine();
            }
            else
            {
                Console.Write($"Enter the Host address for your authentication database (default: \"{config.MySql.Authentication.Host}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Authentication.Host = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the Port for your authentication database (default: \"{config.MySql.Authentication.Port}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Authentication.Port = Convert.ToUInt32(variable.Trim());
                Console.WriteLine();

                Console.Write($"Enter the Host address for your shard database (default: \"{config.MySql.Shard.Host}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Shard.Host = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the Port for your shard database (default: \"{config.MySql.Shard.Port}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Shard.Port = Convert.ToUInt32(variable.Trim());
                Console.WriteLine();

                Console.Write($"Enter the Host address for your world database (default: \"{config.MySql.World.Host}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.World.Host = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the Port for your world database (default: \"{config.MySql.World.Port}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.World.Port = Convert.ToUInt32(variable.Trim());
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Typically, all three databases will be on the using the same SQL server credentials, is this how you want to proceed? (Y/n) ");
            variable = Console.ReadLine();
            if (!variable.Equals("n", StringComparison.OrdinalIgnoreCase) && !variable.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write($"Enter the username for your SQL server (default: \"{config.MySql.World.Username}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                {
                    config.MySql.Authentication.Username = variable.Trim();
                    config.MySql.Shard.Username = variable.Trim();
                    config.MySql.World.Username = variable.Trim();
                }
                Console.WriteLine();

                Console.Write($"Enter the password for your SQL server (default: \"{config.MySql.World.Password}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                {
                    config.MySql.Authentication.Password = variable.Trim();
                    config.MySql.Shard.Password = variable.Trim();
                    config.MySql.World.Password = variable.Trim();
                }
            }
            else
            {
                Console.Write($"Enter the username for your authentication database (default: \"{config.MySql.Authentication.Username}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Authentication.Username = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the password for your authentication database (default: \"{config.MySql.Authentication.Password}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Authentication.Password = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the username for your shard database (default: \"{config.MySql.Shard.Username}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Shard.Username = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the password for your shard database (default: \"{config.MySql.Shard.Password}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Shard.Password = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the username for your world database (default: \"{config.MySql.World.Username}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.World.Username = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the password for your world database (default: \"{config.MySql.World.Password}\"): ");
                variable = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.World.Password = variable.Trim();
            }

            Console.WriteLine("commiting configuration to memory...");
            using (StreamWriter file = File.CreateText(configFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                //serializer.NullValueHandling = NullValueHandling.Ignore;
                //serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
                serializer.Serialize(file, config);
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Do you want to ACEmulator to attempt to initilize your SQL databases? This will erase any existing ACEmulator specific databases that may already exist on the server (Y/n): ");
            variable = Console.ReadLine();
            if (!variable.Equals("n", StringComparison.OrdinalIgnoreCase) && !variable.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();

                Console.WriteLine("Searching for base SQL scripts .... ");
                foreach (var file in new DirectoryInfo("DatabaseSetupScripts\\Base").GetFiles("*.sql"))
                {
                    Console.Write($"Found {file.Name} .... ");
                    var sqlDBFile = File.ReadAllText(file.FullName);
                    var sqlConnectInfo = $"server={config.MySql.World.Host};port={config.MySql.World.Port};user={config.MySql.World.Username};password={config.MySql.World.Password}";
                    switch (file.Name)
                    {
                        case "AuthenticationBase":
                            sqlConnectInfo = $"server={config.MySql.Authentication.Host};port={config.MySql.Authentication.Port};user={config.MySql.Authentication.Username};password={config.MySql.Authentication.Password}";
                            break;
                        case "ShardBase":
                            sqlConnectInfo = $"server={config.MySql.Shard.Host};port={config.MySql.Shard.Port};user={config.MySql.Shard.Username};password={config.MySql.Shard.Password}";
                            break;
                    }
                    var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection(sqlConnectInfo);
                    var script = new MySql.Data.MySqlClient.MySqlScript(sqlConnect, sqlDBFile);

                    Console.Write($"Importing into SQL server at {config.MySql.World.Host}:{config.MySql.World.Port} .... ");
                    try
                    {
                        script.StatementExecuted += new MySql.Data.MySqlClient.MySqlStatementExecutedEventHandler(OnStatementExecutedOutputDot);
                        var count = script.Execute();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {

                    }
                    Console.WriteLine(" complete!");
                }
                Console.WriteLine("Base SQL scripts import complete!");

                Console.WriteLine("Searching for Update SQL scripts .... ");

                Console.WriteLine("Searching for Authentication update SQL scripts .... ");
                foreach (var file in new DirectoryInfo("DatabaseSetupScripts\\Updates\\Authentication").GetFiles("*.sql"))
                {
                    Console.Write($"Found {file.Name} .... ");
                    var sqlDBFile = File.ReadAllText(file.FullName);
                    var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={config.MySql.Authentication.Host};port={config.MySql.Authentication.Port};user={config.MySql.Authentication.Username};password={config.MySql.Authentication.Password};database={config.MySql.Authentication.Database}");
                    var script = new MySql.Data.MySqlClient.MySqlScript(sqlConnect, sqlDBFile);

                    Console.Write($"Importing into {config.MySql.Authentication.Database} database on SQL server at {config.MySql.Authentication.Host}:{config.MySql.Authentication.Port} .... ");
                    try
                    {
                        script.StatementExecuted += new MySql.Data.MySqlClient.MySqlStatementExecutedEventHandler(OnStatementExecutedOutputDot);
                        var count = script.Execute();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {

                    }
                    Console.WriteLine(" complete!");
                }
                Console.WriteLine("Authentication update SQL scripts import complete!");

                Console.WriteLine("Searching for Shard update SQL scripts .... ");
                foreach (var file in new DirectoryInfo("DatabaseSetupScripts\\Updates\\Shard").GetFiles("*.sql"))
                {
                    Console.Write($"Found {file.Name} .... ");
                    var sqlDBFile = File.ReadAllText(file.FullName);
                    var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={config.MySql.Shard.Host};port={config.MySql.Shard.Port};user={config.MySql.Shard.Username};password={config.MySql.Shard.Password};database={config.MySql.Shard.Database}");
                    var script = new MySql.Data.MySqlClient.MySqlScript(sqlConnect, sqlDBFile);

                    Console.Write($"Importing into {config.MySql.Shard.Database} database on SQL server at {config.MySql.Shard.Host}:{config.MySql.Shard.Port} .... ");
                    try
                    {
                        script.StatementExecuted += new MySql.Data.MySqlClient.MySqlStatementExecutedEventHandler(OnStatementExecutedOutputDot);
                        var count = script.Execute();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {

                    }
                    Console.WriteLine(" complete!");
                }
                Console.WriteLine("Shard update SQL scripts import complete!");

                Console.WriteLine("Searching for World update SQL scripts .... ");
                foreach (var file in new DirectoryInfo("DatabaseSetupScripts\\Updates\\World").GetFiles("*.sql"))
                {
                    Console.Write($"Found {file.Name} .... ");
                    var sqlDBFile = File.ReadAllText(file.FullName);
                    var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={config.MySql.World.Host};port={config.MySql.World.Port};user={config.MySql.World.Username};password={config.MySql.World.Password};database={config.MySql.World.Database}");
                    var script = new MySql.Data.MySqlClient.MySqlScript(sqlConnect, sqlDBFile);

                    Console.Write($"Importing into {config.MySql.World.Database} database on SQL server at {config.MySql.World.Host}:{config.MySql.World.Port} .... ");
                    try
                    {
                        script.StatementExecuted += new MySql.Data.MySqlClient.MySqlStatementExecutedEventHandler(OnStatementExecutedOutputDot);
                        var count = script.Execute();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {

                    }
                    Console.WriteLine(" complete!");
                }
                Console.WriteLine("World update SQL scripts import complete!");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Do you want to download the latest world database and import it? (Y/n): ");
            variable = Console.ReadLine();
            if (!variable.Equals("n", StringComparison.OrdinalIgnoreCase) && !variable.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();

                Console.Write("Looking up latest release from ACEmulator/ACE-World-16PY-Patches .... ");
                // webrequest code provided by OptimShi
                var url = "https://api.github.com/repos/ACEmulator/ACE-World-16PY-Patches/releases";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla//5.0 (Windows NT 10.0; Win64; x64; rv:72.0) Gecko//20100101 Firefox//72.0";
                request.UserAgent = "ACE.Server";

                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                var html = reader.ReadToEnd();
                reader.Close();
                response.Close();

                dynamic json = JsonConvert.DeserializeObject(html);
                string tag = json[0].tag_name;
                string dbURL = json[0].assets[0].browser_download_url;
                string dbFileName = json[0].assets[0].name;
                // webrequest code provided by OptimShi

                Console.WriteLine($"Found {tag} !");

                Console.Write($"Downloading {dbFileName} .... ");
                using (var client = new WebClient())
                {
                    client.DownloadFile(dbURL, dbFileName);
                }
                Console.WriteLine("download complete!");

                Console.Write($"Extracting {dbFileName} .... ");
                ZipFile.ExtractToDirectory(dbFileName, ".", true);
                Console.WriteLine("extraction complete!");
                Console.Write($"Deleting {dbFileName} .... ");
                File.Delete(dbFileName);
                Console.WriteLine("Deleted!");

                var sqlFile = (string)dbFileName;
                sqlFile = sqlFile.Substring(0, sqlFile.Length - 4);

                Console.Write($"Reading {sqlFile} .... ");
                var sqlDB = File.ReadAllText(sqlFile);
                Console.WriteLine("complete!");

                var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={config.MySql.World.Host};port={config.MySql.World.Port};user={config.MySql.World.Username};password={config.MySql.World.Password}");
                var script = new MySql.Data.MySqlClient.MySqlScript(sqlConnect, sqlDB);

                Console.Write($"Importing {sqlFile} into SQL server at {config.MySql.World.Host}:{config.MySql.World.Port} (This will take a while, please be patient) .... ");
                try
                {
                    script.StatementExecuted += new MySql.Data.MySqlClient.MySqlStatementExecutedEventHandler(OnStatementExecutedOutputDot);
                    var count = script.Execute();
                }
                catch (MySql.Data.MySqlClient.MySqlException)
                {

                }
                Console.WriteLine(" complete!");

                Console.Write($"Deleting {sqlFile} .... ");
                File.Delete(sqlFile);
                Console.WriteLine("Deleted!");
            }

            Console.WriteLine("exiting setup for ACEmulator.");
        }

        private static void OnStatementExecutedOutputDot(object sender, MySql.Data.MySqlClient.MySqlScriptEventArgs args)
        {
            Console.Write(".");
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
