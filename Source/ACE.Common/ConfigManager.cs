using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace ACE.Common
{
    public static class ConfigManager
    {
        public static MasterConfiguration Config { get; private set; }

        /// <summary>
        /// Full path to the configuration file used during initialization.
        /// </summary>
        public static string ConfigPath { get; private set; } = string.Empty;

        private static readonly object ApiKeyLock = new();

        /// <summary>
        /// initializes from a preloaded configuration
        /// </summary>
        public static void Initialize(MasterConfiguration configuration)
        {
            Config = configuration;
        }

        /// <summary>
        /// initializes from a Config.js file specified by the path
        /// </summary>
        public static void Initialize(string path = @"Config.js")
        {
            var directoryName = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path) ?? "Config.js";

            string pathToUse;

            // If no directory was specified, try both the current directory and the startup directory
            if (string.IsNullOrWhiteSpace(directoryName))
            {
                directoryName = Environment.CurrentDirectory;

                pathToUse = Path.Combine(directoryName, fileName);

                if (!File.Exists(pathToUse))
                {
                    // File not found in Environment.CurrentDirectory
                    // Lets try the ExecutingAssembly Location
                    var executingAssemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

                    directoryName = Path.GetDirectoryName(executingAssemblyLocation);

                    if (directoryName != null)
                        pathToUse = Path.Combine(directoryName, fileName);
                }
            }
            else
            {
                pathToUse = path;
            }

            ConfigPath = pathToUse;

            try
            {
                if (!File.Exists(pathToUse))
                {
                    Console.WriteLine("Configuration file is missing.  Please copy the file Config.js.example to Config.js and edit it to match your needs before running ACE.");
                    throw new Exception("missing configuration file");
                }

                var fileText = File.ReadAllText(pathToUse);

                Config = JsonSerializer.Deserialize<MasterConfiguration>(fileText, SerializerOptions);
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception occured while loading the configuration file!");
                Console.WriteLine($"Exception: {exception.Message}");

                // environment.exit swallows this exception for testing purposes.  we want to expose it.
                throw;
            }
        }

        public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = true
        };

        /// <summary>
        /// Reloads API keys from the configuration file when <see cref="Config.Server.Api.RequireApiKey"/> is true.
        /// </summary>
        public static void ReloadApiKeys(log4net.ILog logger = null)
        {
            if (!Config.Server.Api.RequireApiKey)
                return;

            if (string.IsNullOrEmpty(ConfigPath) || !File.Exists(ConfigPath))
                return;

            try
            {
                var fileText = File.ReadAllText(ConfigPath);
                var temp = JsonSerializer.Deserialize<MasterConfiguration>(fileText, SerializerOptions);
                if (temp == null)
                    return;

                var newKeys = temp.Server.Api.ApiKeys ?? Array.Empty<string>();

                lock (ApiKeyLock)
                {
                    if (newKeys.SequenceEqual(Config.Server.Api.ApiKeys))
                        return;

                    Config.Server.Api.ApiKeys = newKeys;
                }

                logger?.Info($"Reloaded {newKeys.Length} API keys from configuration");
            }
            catch (Exception ex)
            {
                logger?.Warn($"Failed to reload API keys: {ex.Message}");
            }
        }
    }
}
