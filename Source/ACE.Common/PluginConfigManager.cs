using DouglasCrockford.JsMin;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ACE.Common
{
    /// <summary>
    /// template for a configuration manager that logs.
    /// For plugins that need a persistent file based configuration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PluginConfigManager<T>
    {
        public static T Config { get; private set; }

        public enum LogLevel
        {
            Info,
            Error,
            Fatal,
            Debug
        }

        public delegate void LogCallbackDelegate(string msg, LogLevel lev = LogLevel.Info, Exception ex = null);

        public static bool ConfigSucessfullyLoaded { get; private set; } = false;

        /// <summary>
        /// initializes from a preloaded configuration
        /// </summary>
        public static void Initialize(T configuration)
        {
            Config = configuration;
        }

        /// <summary>
        /// initializes from a Config.js file specified by the path
        /// </summary>
        public static void Initialize(LogCallbackDelegate LogCallback)
        {
            string pluginName = Assembly.GetAssembly(typeof(T)).GetName().Name;
            string[] anp = pluginName.Split('.');
            if (anp.Length < 3)
            {
                string blurb = $"Plugin configuration class is to be used by plugin assemblies.";
                LogCallback(blurb, LogLevel.Fatal, null);
                throw new Exception(blurb);
            };
            string filename = anp.Skip(2).Aggregate((a, b) => a + "." + b).ToLower() + ".js";
            string fp = Path.Combine(Path.Combine(Path.Combine(Environment.CurrentDirectory, "Plugins"), pluginName), filename);
            string dirSep = Path.DirectorySeparatorChar.ToString();
            string relPath = $"{dirSep}Plugins{dirSep}{pluginName}{dirSep}{filename}";
            try
            {
                if (!File.Exists(fp))
                {
                    string blurb = $"Plugin configuration file is missing.  Please copy the file {filename}.example to {relPath} and edit it to match your needs before enabling this plugin.";
                    LogCallback(blurb, LogLevel.Fatal, null);
                    throw new Exception(blurb);
                }
                else
                {
                    Config = JsonConvert.DeserializeObject<T>(new JsMinifier().Minify(File.ReadAllText(fp)));
                    LogCallback($"{pluginName} loaded configuration from {relPath}", LogLevel.Debug);
                    ConfigSucessfullyLoaded = true;
                }
            }
            catch (Exception exception)
            {
                LogCallback($"An exception occured while loading the {filename} configuration file!", LogLevel.Fatal, exception);
                // environment.exit swallows this exception for testing purposes.  we want to expose it.
                throw;
            }
        }
    }
}
