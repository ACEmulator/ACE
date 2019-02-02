using System;
using System.IO;
using DouglasCrockford.JsMin;
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
        public static void Initialize()
        {
            string fn = @"Config.json";
            string fpOld = Path.Combine(Environment.CurrentDirectory, fn);
            string fpNew = Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(fn) + ".js");
            string fpChoice = null;

            try
            {
                if (!File.Exists(fpNew) && File.Exists(fpOld))
                {
                    fpChoice = fpOld;
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
                //Config = JsonConvert.DeserializeObject<MasterConfiguration>(File.ReadAllText(path));
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
