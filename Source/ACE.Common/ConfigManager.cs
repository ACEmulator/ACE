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
            string fpOld = Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(path) + ".json");
            string fpNew = Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(path) + ".js");
            string fpChoice = null;
            try
            {
                if (!File.Exists(fpNew) && File.Exists(fpOld))
                {
                    File.Move(fpOld, fpNew);
                    fpChoice = fpNew;
                }
                else if (File.Exists(fpNew))
                {
                    fpChoice = fpNew;
                }
                else
                {
                    Console.WriteLine("Configuration file is missing.  Please copy the file Config.js.example to Config.js and edit it to match your needs before running ACE.");
                    throw new Exception("missing configuration file");
                }
                Config = JsonConvert.DeserializeObject<MasterConfiguration>(new JsMinifier().Minify(File.ReadAllText(fpChoice)));
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
