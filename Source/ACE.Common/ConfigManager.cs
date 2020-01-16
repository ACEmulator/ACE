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

            try
            {
                if (!File.Exists(pathToUse))
                {
                    Console.WriteLine("Configuration file is missing.  Please copy the file Config.js.example to Config.js and edit it to match your needs before running ACE.");
                    throw new Exception("missing configuration file");
                }

                var fileText = File.ReadAllText(pathToUse);

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
