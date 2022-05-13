extern alias MySqlConnectorAlias;

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using ACE.Common;

using DouglasCrockford.JsMin;
using Newtonsoft.Json;

namespace ACE.Server
{
    partial class Program
    {
        private static void DoOutOfBoxSetup(string configFile)
        {
            var exeLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var configJsExample = Path.Combine(exeLocation, "Config.js.example");
            var exampleFile = new FileInfo(configJsExample);
            if (!exampleFile.Exists)
            {
                log.Error("config.js.example Configuration file is missing.  Please copy the file config.js.example to config.js and edit it to match your needs before running ACE.");
                throw new Exception("missing config.js configuration file");
            }
            else
            {
                if (!IsRunningInContainer)
                {
                    Console.WriteLine("config.js Configuration file is missing,  cloning from example file.");
                    File.Copy(configJsExample, configFile, true);
                }
                else
                {
                    Console.WriteLine("config.js Configuration file is missing, ACEmulator is running in a container,  cloning from docker file.");
                    var configJsDocker = Path.Combine(exeLocation, "Config.js.docker");
                    File.Copy(configJsDocker, configFile, true);
                }
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
            if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_WORLD_NAME");
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
            if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_DAT_FILES_DIRECTORY");
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
            Console.WriteLine("Next we will configure your SQL server connections. You will need to provide a database name, username and password for each.");
            Console.WriteLine("Default names for the databases are recommended, and it is also recommended you not use root for login to database. The password must not be blank.");
            Console.WriteLine("It is also recommended the SQL server be hosted on the same machine as this server, so defaults for Host and Port would be ideal as well.");
            Console.WriteLine("As before, pressing enter will use default value.");
            Console.WriteLine();
            Console.WriteLine();

            Console.Write($"Enter the database name for your authentication database (default: \"{config.MySql.Authentication.Database}\"): ");
            variable = Console.ReadLine();
            if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_AUTH_DATABASE_NAME");
            if (!string.IsNullOrWhiteSpace(variable))
                config.MySql.Authentication.Database = variable.Trim();
            Console.WriteLine();

            Console.Write($"Enter the database name for your shard database (default: \"{config.MySql.Shard.Database}\"): ");
            variable = Console.ReadLine();
            if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_SHARD_DATABASE_NAME");
            if (!string.IsNullOrWhiteSpace(variable))
                config.MySql.Shard.Database = variable.Trim();
            Console.WriteLine();

            Console.Write($"Enter the database name for your world database (default: \"{config.MySql.World.Database}\"): ");
            variable = Console.ReadLine();
            if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_WORLD_DATABASE_NAME");
            if (!string.IsNullOrWhiteSpace(variable))
                config.MySql.World.Database = variable.Trim();
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Typically, all three databases will be on the same SQL server, is this how you want to proceed? (Y/n) ");
            variable = Console.ReadLine();
            if (IsRunningInContainer) variable = "n";
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
                if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_AUTH_DATABASE_HOST");
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Authentication.Host = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the Port for your authentication database (default: \"{config.MySql.Authentication.Port}\"): ");
                variable = Console.ReadLine();
                if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_AUTH_DATABASE_PORT");
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Authentication.Port = Convert.ToUInt32(variable.Trim());
                Console.WriteLine();

                Console.Write($"Enter the Host address for your shard database (default: \"{config.MySql.Shard.Host}\"): ");
                variable = Console.ReadLine();
                if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_SHARD_DATABASE_HOST");
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Shard.Host = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the Port for your shard database (default: \"{config.MySql.Shard.Port}\"): ");
                variable = Console.ReadLine();
                if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_SHARD_DATABASE_PORT");
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.Shard.Port = Convert.ToUInt32(variable.Trim());
                Console.WriteLine();

                Console.Write($"Enter the Host address for your world database (default: \"{config.MySql.World.Host}\"): ");
                variable = Console.ReadLine();
                if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_WORLD_DATABASE_HOST");
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.World.Host = variable.Trim();
                Console.WriteLine();

                Console.Write($"Enter the Port for your world database (default: \"{config.MySql.World.Port}\"): ");
                variable = Console.ReadLine();
                if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("ACE_SQL_WORLD_DATABASE_PORT");
                if (!string.IsNullOrWhiteSpace(variable))
                    config.MySql.World.Port = Convert.ToUInt32(variable.Trim());
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Typically, all three databases will be on the using the same SQL server credentials, is this how you want to proceed? (Y/n) ");
            variable = Console.ReadLine();
            if (IsRunningInContainer) variable = "y";
            if (!variable.Equals("n", StringComparison.OrdinalIgnoreCase) && !variable.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write($"Enter the username for your SQL server (default: \"{config.MySql.World.Username}\"): ");
                variable = Console.ReadLine();
                if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("MYSQL_USER");
                if (!string.IsNullOrWhiteSpace(variable))
                {
                    config.MySql.Authentication.Username = variable.Trim();
                    config.MySql.Shard.Username = variable.Trim();
                    config.MySql.World.Username = variable.Trim();
                }
                Console.WriteLine();

                Console.Write($"Enter the password for your SQL server (default: \"{config.MySql.World.Password}\"): ");
                variable = Console.ReadLine();
                if (IsRunningInContainer) variable = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
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
            Console.Write("Do you want to ACEmulator to attempt to initialize your SQL databases? This will erase any existing ACEmulator specific databases that may already exist on the server (Y/n): ");
            variable = Console.ReadLine();
            if (IsRunningInContainer) variable = Convert.ToBoolean(Environment.GetEnvironmentVariable("ACE_SQL_INITIALIZE_DATABASES")) ? "y" : "n";
            if (!variable.Equals("n", StringComparison.OrdinalIgnoreCase) && !variable.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();

                Console.Write($"Waiting for connection to SQL server at {config.MySql.World.Host}:{config.MySql.World.Port} .... ");
                for (; ; )
                {
                    try
                    {
                        using (var sqlTestConnection = new MySql.Data.MySqlClient.MySqlConnection($"server={config.MySql.World.Host};port={config.MySql.World.Port};user={config.MySql.World.Username};password={config.MySql.World.Password};DefaultCommandTimeout=120"))
                        {
                            Console.Write(".");
                            sqlTestConnection.Open();
                        }

                        break;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        Console.Write(".");
                        Thread.Sleep(5000);
                    }
                }
                Console.WriteLine(" connected!");

                if (IsRunningInContainer)
                {
                    Console.Write("Clearing out temporary ace% database .... ");
                    var sqlDBFile = "DROP DATABASE `ace%`;";
                    var sqlConnectInfo = $"server={config.MySql.World.Host};port={config.MySql.World.Port};user={config.MySql.World.Username};password={config.MySql.World.Password};DefaultCommandTimeout=120";
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
                    Console.WriteLine(" done!");
                }

                Console.WriteLine("Searching for base SQL scripts .... ");
                foreach (var file in new DirectoryInfo($"DatabaseSetupScripts{Path.DirectorySeparatorChar}Base").GetFiles("*.sql").OrderBy(f => f.Name))
                {
                    Console.Write($"Found {file.Name} .... ");
                    var sqlDBFile = File.ReadAllText(file.FullName);
                    var sqlConnectInfo = $"server={config.MySql.World.Host};port={config.MySql.World.Port};user={config.MySql.World.Username};password={config.MySql.World.Password};DefaultCommandTimeout=120";
                    switch (file.Name)
                    {
                        case "AuthenticationBase.sql":
                            sqlConnectInfo = $"server={config.MySql.Authentication.Host};port={config.MySql.Authentication.Port};user={config.MySql.Authentication.Username};password={config.MySql.Authentication.Password};DefaultCommandTimeout=120";
                            sqlDBFile = sqlDBFile.Replace("ace_auth", config.MySql.Authentication.Database);
                            break;
                        case "ShardBase.sql":
                            sqlConnectInfo = $"server={config.MySql.Shard.Host};port={config.MySql.Shard.Port};user={config.MySql.Shard.Username};password={config.MySql.Shard.Password};DefaultCommandTimeout=120";
                            sqlDBFile = sqlDBFile.Replace("ace_shard", config.MySql.Shard.Database);
                            break;
                        case "WorldBase.sql":
                        default:
                            //sqlConnectInfo = $"server={config.MySql.World.Host};port={config.MySql.World.Port};user={config.MySql.World.Username};password={config.MySql.World.Password};DefaultCommandTimeout=120";
                            sqlDBFile = sqlDBFile.Replace("ace_world", config.MySql.World.Database);
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

                PatchDatabase("Authentication", config.MySql.Authentication.Host, config.MySql.Authentication.Port, config.MySql.Authentication.Username, config.MySql.Authentication.Password, config.MySql.Authentication.Database, config.MySql.Shard.Database, config.MySql.World.Database);

                PatchDatabase("Shard", config.MySql.Shard.Host, config.MySql.Shard.Port, config.MySql.Shard.Username, config.MySql.Shard.Password, config.MySql.Authentication.Database, config.MySql.Shard.Database, config.MySql.World.Database);

                PatchDatabase("World", config.MySql.World.Host, config.MySql.World.Port, config.MySql.World.Username, config.MySql.World.Password, config.MySql.Authentication.Database, config.MySql.Shard.Database, config.MySql.World.Database);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Do you want to download the latest world database and import it? (Y/n): ");
            variable = Console.ReadLine();
            if (IsRunningInContainer) variable = Convert.ToBoolean(Environment.GetEnvironmentVariable("ACE_SQL_DOWNLOAD_LATEST_WORLD_RELEASE")) ? "y" : "n";
            if (!variable.Equals("n", StringComparison.OrdinalIgnoreCase) && !variable.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();

                if (IsRunningInContainer)
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("This process will take a while, depending on many factors, and may look stuck while reading and importing the world database, please be patient! ");
                    Console.WriteLine(" ");
                }

                Console.Write("Looking up latest release from ACEmulator/ACE-World-16PY-Patches .... ");

                var url = "https://api.github.com/repos/ACEmulator/ACE-World-16PY-Patches/releases/latest";
                using var client = new WebClient();
                var html = client.GetStringFromURL(url).Result;

                dynamic json = JsonConvert.DeserializeObject(html);
                string tag = json.tag_name;
                string dbURL = json.assets[0].browser_download_url;
                string dbFileName = json.assets[0].name;

                Console.WriteLine($"Found {tag} !");

                Console.Write($"Downloading {dbFileName} .... ");
                var dlTask = client.DownloadFile(dbURL, dbFileName);
                dlTask.Wait();
                Console.WriteLine("download complete!");

                Console.Write($"Extracting {dbFileName} .... ");
                ZipFile.ExtractToDirectory(dbFileName, ".", true);
                Console.WriteLine("extraction complete!");
                Console.Write($"Deleting {dbFileName} .... ");
                File.Delete(dbFileName);
                Console.WriteLine("Deleted!");

                var sqlFile = dbFileName.Substring(0, dbFileName.Length - 4);
                Console.Write($"Importing {sqlFile} into SQL server at {config.MySql.World.Host}:{config.MySql.World.Port} (This will take a while, please be patient) .... ");
                using (var sr = File.OpenText(sqlFile))
                {
                    var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={config.MySql.World.Host};port={config.MySql.World.Port};user={config.MySql.World.Username};password={config.MySql.World.Password};DefaultCommandTimeout=120");

                    var line = string.Empty;
                    var completeSQLline = string.Empty;

                    var dbname = config.MySql.World.Database;

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace("ace_world", dbname);
                        //do minimal amount of work here
                        if (line.EndsWith(";"))
                        {
                            completeSQLline += line + Environment.NewLine;

                            var script = new MySql.Data.MySqlClient.MySqlScript(sqlConnect, completeSQLline);
                            try
                            {
                                script.StatementExecuted += new MySql.Data.MySqlClient.MySqlStatementExecutedEventHandler(OnStatementExecutedOutputDot);
                                var count = script.Execute();
                            }
                            catch (MySql.Data.MySqlClient.MySqlException)
                            {

                            }
                            completeSQLline = string.Empty;
                        }
                        else
                            completeSQLline += line + Environment.NewLine;
                    }
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
    }
}
