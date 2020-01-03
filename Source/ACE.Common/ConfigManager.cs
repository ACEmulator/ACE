using System;
using System.IO;

using Newtonsoft.Json;
using DouglasCrockford.JsMin;

namespace ACE.Common
{
    public static class ConfigManager
    {
        public static MasterConfiguration Config { get; private set; }

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
        public static void Initialize(string filename = @"Config.js")
        {
            var directoryName = Path.GetDirectoryName(filename);
            var fileName = Path.GetFileName(filename) ?? "Config.js";

            string fullPath = null;

            // If no directory was specified, try both the current directory and the startup directory
            if (string.IsNullOrWhiteSpace(directoryName))
            {
                directoryName = Environment.CurrentDirectory;

                fullPath = Path.Combine(directoryName, fileName);

                if (!File.Exists(fullPath))
                {
                    // File not found in Environment.CurrentDirectory
                    // Lets try the ExecutingAssembly Location
                    var executingAssemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

                    directoryName = Path.GetDirectoryName(executingAssemblyLocation);

                    if (directoryName != null)
                        fullPath = Path.Combine(directoryName, fileName);
                }
            }

            try
            {
                if (fullPath == null || !File.Exists(fullPath))
                {
                    Console.WriteLine("Configuration file is missing.  Please copy the file Config.js.example to Config.js and edit it to match your needs before running ACE.");
                    throw new Exception("missing configuration file");
                }

                var fileText = File.ReadAllText(fullPath);

                Config = JsonConvert.DeserializeObject<MasterConfiguration>(new JsMinifier().Minify(fileText));
            }
            catch (Exception exception)
            {
                Console.WriteLine("An exception occured while loading the configuration file!");
                Console.WriteLine($"Exception: {exception.Message}");

                // environment.exit swallows this exception for testing purposes.  we want to expose it.
                throw;
            }
        }
    }
}
