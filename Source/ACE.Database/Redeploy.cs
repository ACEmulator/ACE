using ACE.Common;
using ACE.Database;
using ACE.Entity.Enum;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    /// <summary>
    /// Remote Content sync is used to download content from the Github Api.
    /// </summary>
    public static class Redeploy
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Database CurrentDb { get; set; }

        public static bool RedeploymentActive { get; private set; } = false;

        /// <summary>
        /// External user agent used when connecting to the github Api, or another location.
        /// </summary>
        private static string ApiUserAgent { get; set; } = "ACEmulator";

        /// <summary>
        /// Url to download the latest version of ACE-World.
        /// </summary>
        private static string WorldGithubDownload { get; set; }

        /// <summary>
        /// Filename for the ACE-World Release.
        /// </summary>
        private static string WorldGithubFilename { get; set; }

        /// <summary>
        /// Local path pointing too the Extracted ACE-World Data.
        /// </summary>
        private static string WorldDataPath { get; set; } = "Database\\ACE-World\\";

        /// <summary>
        /// Database/Updates/World/
        /// </summary>
        private static string WoldGithubUpdatePath { get; set; } = "Database\\Updates\\World\\";

        /// <summary>
        /// Database/Base/World/
        /// </summary>
        private static string WoldGithubBaseSqlPath { get; set; } = "Database\\Base\\";

        /// <summary>
        /// WorldBase.sql
        /// </summary>
        private static string WoldGithubBaseSqlFile { get; set; } = "WorldBase.sql";

        /// <summary>
        /// The amount of calls left before being rate limited.
        /// </summary>
        public static int TotalApiCallsAvailable { get; set; } = 60;

        /// <summary>
        /// The amount of calls left before being rate limited.
        /// </summary>
        public static int RemaingApiCalls { get; set; } = 60;

        /// <summary>
        /// The time when the Github API will accept more requests.
        /// </summary>
        public static DateTime? ApiResetTime { get; set; } = DateTime.Today.AddYears(1);

        /// <summary>
        /// Default ACE Authentication database name.
        /// </summary>
        private static readonly string DefaultAuthenticationDatabaseName = "ace_auth";

        /// <summary>
        /// Default ACE Shard database name.
        /// </summary>
        private static readonly string DefaultShardDatabaseName = "ace_shard";

        /// <summary>
        /// Default ACE World database name.
        /// </summary>
        private static readonly string DefaultWorldDatabaseName = "ace_world";

        /// <summary>
        /// Downloads for the Authentication Database.
        /// </summary>
        private static GithubResourceList AuthenticationDownloads { get; set; } = new GithubResourceList();

        /// <summary>
        /// Downloads for the Shard Database.
        /// </summary>
        private static GithubResourceList ShardDownloads { get; set; } = new GithubResourceList();

        /// <summary>
        /// Downloads for the World Database.
        /// </summary>
        private static GithubResourceList WorldDownloads { get; set; } = new GithubResourceList();

        /// <summary>
        /// Setups the databse functions and default database specific resource lists.
        /// </summary>
        private static void Initialize()
        {
            CurrentDb = new Database();
            CurrentDb.Initialize(ConfigManager.Config.MySql.World.Host,
                          ConfigManager.Config.MySql.World.Port,
                          ConfigManager.Config.MySql.World.Username,
                          ConfigManager.Config.MySql.World.Password,
                          ConfigManager.Config.MySql.World.Database,
                          false);
            AuthenticationDownloads.DefaultDatabaseName = DefaultAuthenticationDatabaseName;
            AuthenticationDownloads.ConfigDatabaseName = ConfigManager.Config.MySql.Authentication.Database;
            ShardDownloads.DefaultDatabaseName = DefaultShardDatabaseName;
            ShardDownloads.ConfigDatabaseName = ConfigManager.Config.MySql.Shard.Database;
            WorldDownloads.DefaultDatabaseName = DefaultWorldDatabaseName;
            WorldDownloads.ConfigDatabaseName = ConfigManager.Config.MySql.World.Database;
        }

        /// <summary>
        /// Changes the database.
        /// </summary>
        private static void ResetDatabaseConnection(string newDatabase)
        {
            CurrentDb.ResetConnectionString(ConfigManager.Config.MySql.World.Host,
                          ConfigManager.Config.MySql.World.Port,
                          ConfigManager.Config.MySql.World.Username,
                          ConfigManager.Config.MySql.World.Password,
                          newDatabase,
                          false);
        }

        /// <summary>
        /// Captures the Rate Limit Values from the Response Header
        /// </summary>
        private static void CaptureWebHeaderData(WebHeaderCollection headers)
        {
            if (headers?.Count > 0)
            {
                int tmpInt = 0;
                double rateLimitEpoch = 0;
                // capture the total api calls
                if (int.TryParse(headers.Get("X-RateLimit-Limit"), out tmpInt))
                {
                    TotalApiCallsAvailable = tmpInt;
                }
                // capture the remaining api calls
                if (int.TryParse(headers.Get("X-RateLimit-Remaining"), out tmpInt))
                {
                    RemaingApiCalls = tmpInt;
                }
                // capture the timestamp for rate limite reset
                if (double.TryParse(headers.Get("X-RateLimit-Reset"), out rateLimitEpoch))
                {
                    ApiResetTime = ConvertFromUnixTimestamp(rateLimitEpoch);
                }
            }
        }

        /// <summary>
        /// Retreieves a string from a web location.
        /// </summary>
        public static string RetrieveWebString(string updateUrl)
        {
            using (WebClient webClient = new WebClient())
            {
                var result = string.Empty;
                WebClient w = new WebClient();
                // Header is required for github
                w.Headers.Add("User-Agent", ApiUserAgent);
                try
                {
                    result = w.DownloadString(updateUrl);
                }
                catch (WebException e)
                {
                    if (e.Response != null)
                    {
                        CaptureWebHeaderData(e.Response.Headers);
                        return null;
                    }
                    else
                        throw;
                }
                catch
                {
                    return null;
                }
                CaptureWebHeaderData(w.ResponseHeaders);
                return (result);
            }
        }

        /// <summary>
        /// Retrieves a file from a web location.
        /// </summary>
        private static bool RetrieveWebContent(string url, string destinationFilePath)
        {
            using (WebClient webClient = new WebClient())
            {
                WebClient w = new WebClient();
                // Header is required for github
                w.Headers.Add("User-Agent", ApiUserAgent);
                w.DownloadFile(url, destinationFilePath);
                if (File.Exists(destinationFilePath))
                    return true;
            }
            log.Info($"Troubles downloading {url} {destinationFilePath}");
            return false;
        }

        /// <summary>
        /// Implement if needed: converts content from individual files when called from the Github API.
        /// </summary>
        // private static string readApiContent(string base64string)
        // {
        //    var bin = Convert.FromBase64String(base64string);
        //    return Encoding.UTF8.GetString(bin);
        // }

        /// <summary>
        /// Checks if a Path exists and creates one if it doesn't.
        /// </summary>
        private static bool CheckLocalDataPath(string dataPath)
        {
            // Check to verify config has a valid path:
            if (dataPath?.Length > 0)
            {
                var currentPath = Path.GetFullPath(dataPath);
                // Check too see if path exists and create if does not exist.
                if (!Directory.Exists(currentPath))
                {
                    try
                    {
                        Directory.CreateDirectory(currentPath);
                    }
                    catch
                    {
                        log.Info($"Could not create directory: {dataPath}");
                        return false;
                    }
                }
                PermissionSet perms = new PermissionSet(PermissionState.None);
                FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.Write, currentPath);
                perms.AddPermission(writePermission);
                if (!perms.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
                {
                    // You don't have write permissions
                    log.Info($"Write permissions missing in: {dataPath}");
                    return false;
                }
                // All checks pass, so the directory is good to use.
                return true;
            }
            log.Info($"Configuration error, missing datapath!");
            return false;
        }

        /// <summary>
        /// Attempts to retrieve contents from a Github directory/folder structure.
        /// </summary>
        /// <param name="url">String value containing web location to parse from the Github API.</param>
        /// <returns>true on success, false on failure</returns>
        private static bool RetrieveGithubFolder(string url)
        {
            // Check to see if the input is usable and the data path is valid
            if (url?.Length > 0)
            {
                var localDataPath = Path.GetFullPath(ConfigManager.Config.ContentServer.LocalContentPath);
                List<string> directoryUrls = new List<string>();
                directoryUrls.Add(url);
                var downloads = new List<Tuple<string, string>>();
                // Recurse api and collect all downloads
                while (directoryUrls.Count > 0)
                {
                    var currentUrl = directoryUrls.LastOrDefault();
                    var apiRequestData = RetrieveWebString(currentUrl);
                    if (apiRequestData != null)
                    {
                        var repoFiles = JArray.Parse(apiRequestData);
                        var repoPath = Path.Combine(localDataPath, Path.GetDirectoryName(repoFiles[0]["path"].ToString()));
                        CheckLocalDataPath(repoPath);
                        if (repoFiles?.Count > 0)
                        {
                            foreach (var file in repoFiles)
                            {
                                if (file["type"].ToString() == "dir")
                                {
                                    directoryUrls.Add(file["url"].ToString());
                                    CheckLocalDataPath(Path.Combine(localDataPath, file["path"].ToString()));
                                }
                                else
                                {
                                    downloads.Add(new Tuple<string, string>(item1: file["download_url"].ToString(), item2: Path.Combine(localDataPath, file["path"].ToString())));
                                }
                            }
                        }
                        // Cancel because the string was caught in exception
                        if (repoFiles == null)
                        {
                            log.Info($"No files found within {repoPath}");
                            return false;
                        }
                    }
                    else
                    {
                        log.Info($"Issue found calling API.");
                        return false;
                    }
                    // Remove the parsed url
                    directoryUrls.Remove(currentUrl);
                }
                // Download the files from the downloads tuple
                foreach (var download in downloads)
                {
                    // If we cannot Retrieve content, return false for failure
                    if (!RetrieveWebContent(download.Item1, download.Item2))
                        log.Info($"Trouble downloading {download.Item1} : {download.Item2}");
                }
                return true;
            }
            log.Info($"Invalid Url provided, please check configuration.");
            return false;
        }

        /// <summary>
        /// Parses a file path too determine the correct database.
        /// </summary>
        private static Tuple<string, GithubResourceType> GetDatabaseNameAndResourceType(string filePath, string fileName)
        {
            var localType = GithubResourceType.Unknown;
            var databaseName = string.Empty;
            if (fileName.Contains(".txt"))
            {
                localType = GithubResourceType.TextFile;
            }
            else if (filePath.Contains("/Base"))
            {
                localType = GithubResourceType.SqlBaseFile;
                if (fileName.Contains("AuthenticationBase"))
                {
                    databaseName = DefaultAuthenticationDatabaseName;
                }
                else if (fileName.Contains("ShardBase"))
                {
                    databaseName = DefaultShardDatabaseName;
                }
                else if (fileName.Contains("WorldBase"))
                {
                    databaseName = DefaultWorldDatabaseName;
                }
            }
            else if (filePath.Contains("/Updates"))
            {
                localType = GithubResourceType.SqlUpdateFile;
                if (filePath.Contains("/Authentication"))
                {
                    databaseName = DefaultAuthenticationDatabaseName;
                }
                else if (filePath.Contains("/Shard"))
                {
                    databaseName = DefaultShardDatabaseName;
                }
                else if (filePath.Contains("/World"))
                {
                    databaseName = DefaultWorldDatabaseName;
                }
            }
            else if (filePath.Contains("/ACE-World"))
            {
                databaseName = DefaultWorldDatabaseName;
                localType = GithubResourceType.WorldReleaseSqlFile;
            }
            else if (filePath.Contains(".sql"))
            {
                localType = GithubResourceType.SqlFile;
            }
            return Tuple.Create<string, GithubResourceType>(databaseName, localType);
        }

        /// <summary>
        /// Obtains files and folders from the Github repository.
        /// </summary>
        /// <param name="url">Url to the Github API</param>
        /// <returns>List of downloaded files; null on error</returns>
        private static List<GithubResource> RetrieveGithubFolderList(string url)
        {
            if (RemaingApiCalls > 0 || (DateTime.Now >= ApiResetTime.Value))
            {
                // Check to see if the input is usable and the data path is valid
                if (url?.Length > 0)
                {
                    List<GithubResource> downloadList = new List<GithubResource>();
                    var localDataPath = Path.GetFullPath(ConfigManager.Config.ContentServer.LocalContentPath);
                    // Directories collected from the API
                    List<string> directoryUrls = new List<string>();
                    directoryUrls.Add(url);
                    // Recurse api and collect all downloads
                    while (directoryUrls.Count > 0)
                    {
                        var currentUrl = directoryUrls.LastOrDefault();
                        var content = RetrieveWebString(currentUrl);
                        var repoFiles = content != null ? JArray.Parse(content) : null;
                        if (repoFiles?.Count > 0)
                        {
                            foreach (var file in repoFiles)
                            {
                                var search = file["path"].ToString();
                                if (search.Contains("Database"))
                                {
                                    if (file["type"].ToString() == "dir")
                                    {
                                        directoryUrls.Add(file["url"].ToString());
                                        CheckLocalDataPath(Path.Combine(localDataPath, file["path"].ToString()));
                                    }
                                    else
                                    {
                                        var fileName = file["name"].ToString();
                                        var info = GetDatabaseNameAndResourceType(search, fileName);
                                        var databaseName = info.Item1;
                                        var localType = info.Item2;
                                        downloadList.Add(new GithubResource()
                                        {
                                            DatabaseName = databaseName,
                                            Type = localType,
                                            SourceUri = file["download_url"].ToString(),
                                            SourcePath = file["path"].ToString(),
                                            FileName = file["name"].ToString(),
                                            FilePath = Path.GetFullPath(Path.Combine(localDataPath, file["path"].ToString())),
                                            FileSize = (int)file["size"],
                                            Hash = file["sha"].ToString()
                                        });
                                    }
                                }
                            }
                        }
                        // Cancel because the string was caught in exception
                        if (repoFiles == null)
                        {
                            log.Info($"No files found within {currentUrl}");
                            return null;
                        }
                        // Remove the parsed url
                        directoryUrls.Remove(currentUrl);
                    }
                    // Download the files from the downloads tuple
                    foreach (var download in downloadList)
                    {
                        // If we cannot Retrieve content, return false for failure
                        if (!RetrieveWebContent(download.SourceUri, download.FilePath))
                            log.Info($"Trouble downloading {download.SourceUri} : {download.FilePath}");
                    }
                    return downloadList;
                }
                log.Info($"Invalid Url provided, please check configuration.");
                return null;
            }

            log.Info($"You have exhausted your Github API limt. Please wait till {ApiResetTime}");
            return null;
        }

        private static GithubResource RetreieveWorldDataFile()
        {
            if (RemaingApiCalls > 0)
            {
                // attempt to download the latest ACE-World json data
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        WebClient w = new WebClient();
                        // Header is required for github
                        w.Headers.Add("User-Agent", "ACEManager");
                        var json = JObject.Parse(w.DownloadString(ConfigManager.Config.ContentServer.WorldArchiveUrl));
                        // Extract relevant details
                        WorldGithubDownload = (string)json["assets"][0]["browser_download_url"];
                        WorldGithubFilename = (string)json["assets"][0]["name"];
                        // (string)json["name"] + (string)json["tag_name"] + (string)json["published_at"];
                        // Collect header info that tells how much retries and time left till reset.
                        CaptureWebHeaderData(w.ResponseHeaders);
                    }
                }
                catch (Exception error)
                {
                    log.Info($"Trouble capturing metadata from the Github API. {error.ToString()}");
                    return null;
                }

                var worldArchive = Path.GetFullPath(Path.Combine(ConfigManager.Config.ContentServer.LocalContentPath, WorldGithubFilename));
                var worldPath = Path.GetFullPath(Path.Combine(ConfigManager.Config.ContentServer.LocalContentPath, WorldDataPath));

                if (RetrieveWebContent(WorldGithubDownload, worldArchive))
                {
                    // Extract & delete
                    var extractionError = ExtractZip(worldArchive, worldPath);
                    if (extractionError?.Length > 0)
                    {
                        log.Info($"Could not extract {worldArchive} {extractionError}");
                        return null;
                    }
                }

                // Grab the archive folder path
                var files = from file in Directory.EnumerateFiles(worldPath) where !file.Contains(".txt") select new { File = file };

                GithubResource resource = new GithubResource();

                foreach (var file in files)
                {
                    resource.FileName = Path.GetFileName(file.File);
                    resource.FilePath = file.File;
                    resource.DatabaseName = DefaultWorldDatabaseName;
                    resource.Type = GithubResourceType.WorldReleaseSqlFile;
                }

                return resource;
            }
            // No more calls left, detail the time remaining
            log.Error($"You have exhausted your Github API limt. Please wait till {ApiResetTime}");
            return null;
        }

        /// <summary>
        /// Deletes and re-creates a database.
        /// </summary>
        private static void ResetDatabase(string databaseName)
        {
            ResetDatabaseConnection(string.Empty);
            // Delete Database, to clear everything including stored procs and views.
            var dropResult = CurrentDb.DropDatabase(databaseName);
            if (dropResult != null)
            {
                log.Info($"Error dropping database: {dropResult}");
            }

            // Create Database
            var createResult = CurrentDb.CreateDatabase(databaseName);
            if (createResult != null)
            {
                log.Info($"Error dropping database: {createResult}");
            }
        }

        /// <summary>
        /// Moves downloads to the appropriate resource list.
        /// </summary>
        private static void ParseDownloads(List<GithubResource> list)
        {
            foreach (var download in list)
            {
                if (download.DatabaseName == DefaultAuthenticationDatabaseName)
                {
                    AuthenticationDownloads.Downloads.Add(download);
                }
                else if (download.DatabaseName == DefaultShardDatabaseName)
                {
                    ShardDownloads.Downloads.Add(download);
                }
                else if (download.DatabaseName == DefaultWorldDatabaseName)
                {
                    WorldDownloads.Downloads.Add(download);
                }
            }
        }

        /// <summary>
        /// Scoures a directory and finds relevante data on the local system that originated from Github.
        /// </summary>
        /// <param name="directoryPath"></param>
        private static List<GithubResource> ParseLocalDatabase(string directoryPath)
        {
            List<GithubResource> resources = null;

            var actualPath = Path.GetFullPath(Path.Combine(directoryPath));

            if (Directory.Exists(actualPath))
            {
                // Instance the new object
                resources = new List<GithubResource>();

                var files = from file in Directory.EnumerateFiles(actualPath, "*.*", SearchOption.AllDirectories) where !file.Contains(".txt") select new { File = file };

                foreach (var file in files)
                {
                    var path = Path.GetDirectoryName(file.File);
                    var searchPath = path.Replace(actualPath, string.Empty).Replace(@"\", @"/");
                    var fileName = Path.GetFileName(file.File);
                    var fileSize = new FileInfo(file.File).Length;
                    var item = GetDatabaseNameAndResourceType(searchPath, fileName);
                    if (item.Item1.Length > 0)
                    {
                        resources.Add(new GithubResource() { FileName = fileName, FilePath = file.File, SourcePath = searchPath, SourceUri = file.File, DatabaseName = item.Item1, Type = item.Item2, FileSize = Convert.ToInt32(fileSize) });
                    }
                }
            }

            return resources;
        }

        /// <summary>
        /// Get the data files from local disk.
        /// </summary>
        /// <returns></returns>
        private static List<GithubResource> LocalSync()
        {
            var databaseFiles = ParseLocalDatabase(ConfigManager.Config.ContentServer.LocalContentPath);
            return databaseFiles;
        }

        /// <summary>
        /// Get the data files from github.
        /// </summary>
        /// <returns></returns>
        private static List<GithubResource> RemoteSync()
        {
            var databaseFiles = RetrieveGithubFolderList(ConfigManager.Config.ContentServer.DatabaseUrl);
            log.Info("Downloading ACE-World Archive.");
            var worldFile = RetreieveWorldDataFile();
            if (worldFile != null)
            {
                databaseFiles.Add(worldFile);
            }
            else
            {
                log.Error("Error downloading worldfile!");
                return null;
            }
            return databaseFiles;
        }

        /// <summary>
        /// Clears the content folder of files and directories.
        /// </summary>
        /// <param name="folderPath"></param>
        private static void wipeContentFolder(string folderPath)
        {
            foreach (var file in Directory.GetFiles(folderPath))
            {
                File.Delete(file);
            }
            foreach (var dir in Directory.GetDirectories(folderPath))
            {
                Directory.Delete(dir, true);
            }
        }

        /// <summary>
        /// Retreives the content from the source provided.
        /// </summary>
        /// <param name="source">Github or LocalDisk must be used</param>
        /// <returns>A list of file resources for use with the database</returns>
        public static List<GithubResource> GetDataFiles(SourceSelectionOption source)
        {
            var updateMessage = $"Retreiving files from {Enum.GetName(typeof(SourceSelectionOption), source)}";
            log.Info(updateMessage);
            // Local database download Path
            var localDataPath = Path.GetFullPath(Path.Combine(ConfigManager.Config.ContentServer.LocalContentPath));

            // Check the data path and create if needed.
            if (CheckLocalDataPath(localDataPath))
            {
                List<GithubResource> fileList = new List<GithubResource>();

                // Attempt to get remote data from github.
                if (source == SourceSelectionOption.Github)
                {
                    // Delete the database content to remove old content
                    if (Directory.Exists(localDataPath))
                        wipeContentFolder(localDataPath);
                    fileList = RemoteSync();
                }
                // Load prevoous download/local data
                else if (source == SourceSelectionOption.LocalDisk)
                    fileList = LocalSync();

                return fileList;
            }
            return null;
        }

        /// <summary>
        /// Attempts to download and redeploy data from Github, to the specified database(s). WARNING: CAN CAUSE LOSS OF DATA IF USED IMPROPERLY!
        /// </summary>
        /// <remarks>                        
        /// Load Data from 2 or 3 locations, in sequential order:
        ///    First Search Path: ${Downloads}\\Database\\Base\\
        ///    Second Search Path if updating world database: ACE-World\\${WorldGithubFilename}
        ///    Third Search Path: ${Downloads}\\Database\\Updates\\
        /// </remarks>
        public static string RedeployDatabaseFromSource(DatabaseSelectionOption databaseSelection, SourceSelectionOption dataSource)
        {
            if (RedeploymentActive)
                return "There is already an active redeployment in progress...";
            if (databaseSelection == DatabaseSelectionOption.None)
                return "You must select a database other than 0 (None)..";
            if (dataSource == SourceSelectionOption.None)
                return "You must select a source option other than 0 (None)..";
            // Determine if the config settings appear valid:
            if (ConfigManager.Config.ContentServer == null)
                return "ContentServer configration missing from config! Please edit the config!";
            if (ConfigManager.Config.ContentServer.LocalContentPath?.Length <= 0)
                return $"Data path missing from the ContentServer config! Please edit the config! {ConfigManager.Config.ContentServer.LocalContentPath}";

            log.Info($"A full database Redeployment has been initiated for {Enum.GetName(typeof(DatabaseSelectionOption), databaseSelection)}!");

            RedeploymentActive = true;
            // Setup the database requirements.
            Initialize();
            List<GithubResource> databaseFiles = null;
            // Download the database files from Github:
            log.Info("Attempting download of all database files from Github Folder.");

            databaseFiles = GetDataFiles(dataSource);

            if (databaseFiles == null)
            {
                if (dataSource == SourceSelectionOption.Github)
                {
                    RedeploymentActive = false;
                    var couldNotDownload = RemaingApiCalls == 0 ? $"API limit reached, please wait until {ApiResetTime.ToString()} and then try again." : "Unknown issue downloading.";
                    log.Info(couldNotDownload);
                    return couldNotDownload;
                }
            }

            if (databaseFiles?.Count > 0)
            {
                List<GithubResourceList> resources = new List<GithubResourceList>();

                ParseDownloads(databaseFiles);
                if (databaseSelection == DatabaseSelectionOption.All)
                {
                    resources.Add(AuthenticationDownloads);
                    resources.Add(ShardDownloads);
                    resources.Add(WorldDownloads);
                }
                else
                {
                    switch (databaseSelection)
                    {
                        case DatabaseSelectionOption.Authentication:
                            {
                                resources.Add(AuthenticationDownloads);
                                break;
                            }
                        case DatabaseSelectionOption.Shard:
                            {
                                resources.Add(ShardDownloads);
                                break;
                            }
                        case DatabaseSelectionOption.World:
                            {
                                resources.Add(WorldDownloads);
                                break;
                            }
                    }
                }

                foreach (var resource in resources)
                {
                    if (resource.Downloads.Count == 0) continue;

                    var baseFile = string.Empty;
                    List<string> updates = new List<string>();
                    var worldFile = string.Empty;

                    // Seporate base files from updates
                    foreach (var download in resource.Downloads)
                    {
                        if (download.Type == GithubResourceType.SqlBaseFile)
                        {
                            baseFile = download.FilePath;
                        }
                        if (download.Type == GithubResourceType.SqlUpdateFile)
                        {
                            updates.Add(download.FilePath);
                        }
                        if (download.Type == GithubResourceType.WorldReleaseSqlFile)
                        {
                            worldFile = download.FilePath;
                        }
                    }

                    try
                    {
                        // Delete and receate the database
                        ResetDatabase(resource.ConfigDatabaseName);

                        // First sequence, load the world base
                        if (File.Exists(baseFile))
                            ReadAndLoadScript(baseFile, resource.ConfigDatabaseName, resource.DefaultDatabaseName);
                        else
                        {
                            var errorMessage = $"There was an error locating the base file {baseFile} for {resource.DefaultDatabaseName}!";
                            log.Info(errorMessage);
                        }

                        // Second, if this is the world database, we will load ACE-World
                        if (resource.DefaultDatabaseName == DefaultWorldDatabaseName)
                        {
                            if (File.Exists(worldFile))
                                ReadAndLoadScript(worldFile, resource.ConfigDatabaseName, resource.DefaultDatabaseName);
                            else
                            {
                                var errorMessage = $"There was an error locating the base file {worldFile} for {resource.DefaultDatabaseName}!";
                                log.Info(errorMessage);
                                return errorMessage;
                            }
                        }

                        // Last, run all updates
                        if (updates.Count() > 0)
                        {
                            foreach (var file in updates)
                            {
                                ReadAndLoadScript(file, resource.ConfigDatabaseName, resource.DefaultDatabaseName);
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        var errorMessage = error.Message;
                        if (error.InnerException != null)
                        {
                            errorMessage += " Inner: " + error.InnerException.Message;
                        }
                        log.Info(errorMessage);
                        RedeploymentActive = false;
                        return errorMessage;
                    }
                }
                RedeploymentActive = false;
                // Success
                return null;
            }
            RedeploymentActive = false;
            // Could not find configuration or error in function.
            var configErrorMessage = "No data files were found on local disk or an unknown error has occurred.";
            log.Info(configErrorMessage);
            return configErrorMessage;
        }

        /// <summary>
        /// Reads a file and updates database names from the config, then loads file the file content into the database.
        /// </summary>
        private static string ReadAndLoadScript(string sqlFile, string databaseName, string defaultDatabase)
        {
            log.Info($"Database: {databaseName} Loading {sqlFile}...");
            // open file into string
            string sqlInputFile = File.ReadAllText(sqlFile);
            if (databaseName != defaultDatabase)
            {
                sqlInputFile = sqlInputFile.Replace(defaultDatabase, databaseName);
            }
            ResetDatabaseConnection(databaseName);
            return CurrentDb.ExecuteSqlQueryOrScript(sqlInputFile, databaseName, true);
        }

        /// <summary>
        /// Attempts to extract a file from a directory, into a relative path. If ACEManager.Config.SaveOldWorldArchives is false, then the archive will also be deleted.
        /// </summary>
        private static string ExtractZip(string filePath, string destinationPath)
        {
            // $"Extracting Zip {filePath}...";
            if (Directory.Exists(destinationPath)) Directory.Delete(destinationPath, true);
            Directory.CreateDirectory(destinationPath);
            if (!File.Exists(filePath))
            {
                return "ERROR: Zip missing!";
            }

            log.Info($"Extracting archive {filePath}");
            try
            {
                ZipFile.ExtractToDirectory(filePath, destinationPath);
            }
            catch (Exception error)
            {
                return error.Message;
            }
            finally
            {
                log.Info($"Deleting archive {filePath}");
                File.Delete(filePath);
            }
            return null;
        }

        /// <summary>
        /// Converts a double to epoch time. Used with Github.
        /// </summary>
        public static DateTime ConvertFromUnixTimestamp(double unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}
