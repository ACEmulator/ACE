using System;
using System.IO;

using Newtonsoft.Json;

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
        /// initializes from a config.json file specified by the path
        /// </summary>
        public static void Initialize(string path = @"Config.json")
        {
            try
            {
                Config = JsonConvert.DeserializeObject<MasterConfiguration>(File.ReadAllText(path));
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
