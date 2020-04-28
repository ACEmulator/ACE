extern alias MySqlConnectorAlias;

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;

using ACE.Common;

using Newtonsoft.Json;

namespace ACE.Server
{
    partial class Program
    {
        private static void CheckForWorldDatabaseUpdate()
        {
            log.Info($"Automatic World Database Update started...");
            var worldDb = new Database.WorldDatabase();
            var currentVersion = worldDb.GetVersion();
            log.Info($"Current World Database version: Base - {currentVersion.BaseVersion} | Patch - {currentVersion.PatchVersion}");

            var url = "https://api.github.com/repos/ACEmulator/ACE-World-16PY-Patches/releases";
            var request = (HttpWebRequest)WebRequest.Create(url);
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

            if (currentVersion.PatchVersion != tag)
            {
                log.Info($"Latest patch version is {tag} -- Update Required!");
                UpdateToLatestWorldDatabase(dbURL, dbFileName);
                var newVersion = worldDb.GetVersion();
                log.Info($"Updated World Database version: Base - {newVersion.BaseVersion} | Patch - {newVersion.PatchVersion}");
            }
            else
            {
                log.Info($"Latest patch version is {tag} -- No Update Required!");
            }

            log.Info($"Automatic World Database Update complete.");
        }

        private static void UpdateToLatestWorldDatabase(string dbURL, string dbFileName)
        {
            Console.WriteLine();

            if (IsRunningInContainer)
            {
                Console.WriteLine(" ");
                Console.WriteLine("This process will take a while, depending on many factors, and may look stuck while reading and importing the world database, please be patient! ");
                Console.WriteLine(" ");
            }

            Console.Write($"Downloading {dbFileName} .... ");
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(dbURL, dbFileName);
                }
                catch
                {
                    Console.Write($"Download for {dbFileName} failed!");
                    return;
                }
            }
            Console.WriteLine("download complete!");

            Console.Write($"Extracting {dbFileName} .... ");
            ZipFile.ExtractToDirectory(dbFileName, ".", true);
            Console.WriteLine("extraction complete!");
            Console.Write($"Deleting {dbFileName} .... ");
            File.Delete(dbFileName);
            Console.WriteLine("Deleted!");

            var sqlFile = dbFileName.Substring(0, dbFileName.Length - 4);
            Console.Write($"Importing {sqlFile} into SQL server at {ConfigManager.Config.MySql.World.Host}:{ConfigManager.Config.MySql.World.Port} (This will take a while, please be patient) .... ");
            using (var sr = File.OpenText(sqlFile))
            {
                var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={ConfigManager.Config.MySql.World.Host};port={ConfigManager.Config.MySql.World.Port};user={ConfigManager.Config.MySql.World.Username};password={ConfigManager.Config.MySql.World.Password}");

                var line = string.Empty;
                var completeSQLline = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
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

        private static string GetContentFolder()
        {
            var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={ConfigManager.Config.MySql.Shard.Host};port={ConfigManager.Config.MySql.Shard.Port};user={ConfigManager.Config.MySql.Shard.Username};password={ConfigManager.Config.MySql.Shard.Password};database={ConfigManager.Config.MySql.Shard.Database}");
            var sqlQuery = "SELECT `value` FROM config_properties_string WHERE `key` = 'content_folder';";
            var sqlCommand = new MySql.Data.MySqlClient.MySqlCommand(sqlQuery, sqlConnect);

            sqlConnect.Open();
            var sqlReader = sqlCommand.ExecuteReader();

            var content_folder = sqlReader.HasRows ? sqlReader.GetString(0) : @".\Content";

            sqlReader.Close();
            sqlCommand.Connection.Close();

            // handle relative path
            if (content_folder.StartsWith("."))
            {
                var cwd = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
                content_folder = cwd + content_folder;
            }

            return content_folder;
        }

        private static void AutoApplyWorldCustomizations()
        {
            var content_folder = GetContentFolder();

            Console.WriteLine($"Searching for World Customization SQL scripts .... ");

            var contentDI = new DirectoryInfo($"{content_folder}{Path.DirectorySeparatorChar}WorldCustomizations");

            if (contentDI.Exists)
            {
                foreach (var file in contentDI.GetFiles("*.sql").OrderBy(f => f.Name))
                {
                    Console.Write($"Found {file.Name} .... ");
                    var sqlDBFile = File.ReadAllText(file.FullName);
                    var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={ConfigManager.Config.MySql.World.Host};port={ConfigManager.Config.MySql.World.Port};user={ConfigManager.Config.MySql.World.Username};password={ConfigManager.Config.MySql.World.Password};database={ConfigManager.Config.MySql.World.Database}");
                    var script = new MySql.Data.MySqlClient.MySqlScript(sqlConnect, sqlDBFile);

                    Console.Write($"Importing into World database on SQL server at {ConfigManager.Config.MySql.World.Host}:{ConfigManager.Config.MySql.World.Port} .... ");
                    try
                    {
                        script.StatementExecuted += new MySql.Data.MySqlClient.MySqlStatementExecutedEventHandler(OnStatementExecutedOutputDot);
                        var count = script.Execute();
                        //Console.Write($" {count} database records affected ....");
                        Console.WriteLine(" complete!");
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        Console.WriteLine($" error!");
                        Console.WriteLine($" Unable to apply patch due to following exception: {ex}");
                    }
                }
            }
            Console.WriteLine($"World Customization SQL scripts import complete!");
        }

        private static void AutoApplyDatabaseUpdates()
        {
            log.Info($"Automatic Database Patching started...");
            Thread.Sleep(1000);

            PatchDatabase("Authentication", ConfigManager.Config.MySql.Authentication.Host, ConfigManager.Config.MySql.Authentication.Port, ConfigManager.Config.MySql.Authentication.Username, ConfigManager.Config.MySql.Authentication.Password, ConfigManager.Config.MySql.Authentication.Database);
            PatchDatabase("Shard", ConfigManager.Config.MySql.Shard.Host, ConfigManager.Config.MySql.Shard.Port, ConfigManager.Config.MySql.Shard.Username, ConfigManager.Config.MySql.Shard.Password, ConfigManager.Config.MySql.Shard.Database);
            PatchDatabase("World", ConfigManager.Config.MySql.World.Host, ConfigManager.Config.MySql.World.Port, ConfigManager.Config.MySql.World.Username, ConfigManager.Config.MySql.World.Password, ConfigManager.Config.MySql.World.Database);

            Thread.Sleep(1000);
            log.Info($"Automatic Database Patching complete.");
        }

        private static void PatchDatabase(string dbType, string host, uint port, string username, string password, string database)
        {
            var updatesFile = $"DatabaseSetupScripts{Path.DirectorySeparatorChar}Updates{Path.DirectorySeparatorChar}{dbType}{Path.DirectorySeparatorChar}applied_updates.txt";
            var appliedUpdates = Array.Empty<string>();
            if (File.Exists(updatesFile))
                appliedUpdates = File.ReadAllLines(updatesFile);

            Console.WriteLine($"Searching for {dbType} update SQL scripts .... ");
            foreach (var file in new DirectoryInfo($"DatabaseSetupScripts{Path.DirectorySeparatorChar}Updates{Path.DirectorySeparatorChar}{dbType}").GetFiles("*.sql").OrderBy(f => f.Name))
            {
                if (appliedUpdates.Contains(file.Name))
                    continue;

                Console.Write($"Found {file.Name} .... ");
                var sqlDBFile = File.ReadAllText(file.FullName);
                var sqlConnect = new MySql.Data.MySqlClient.MySqlConnection($"server={host};port={port};user={username};password={password};database={database}");
                var script = new MySql.Data.MySqlClient.MySqlScript(sqlConnect, sqlDBFile);

                Console.Write($"Importing into {database} database on SQL server at {host}:{port} .... ");
                try
                {
                    script.StatementExecuted += new MySql.Data.MySqlClient.MySqlStatementExecutedEventHandler(OnStatementExecutedOutputDot);
                    var count = script.Execute();
                    //Console.Write($" {count} database records affected ....");
                    Console.WriteLine(" complete!");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    Console.WriteLine($" error!");
                    Console.WriteLine($" Unable to apply patch due to following exception: {ex}");
                }
                File.AppendAllText(updatesFile, file.Name + Environment.NewLine);
            }

            Console.WriteLine($"{dbType} update SQL scripts import complete!");
        }
    }
}
