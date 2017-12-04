using Newtonsoft.Json;
using System;
using System.IO;

namespace ACE.ACClientLauncher
{
    public class LauncherConfig
    {
        public string GameApi { get; set; }

        public string LoginServer { get; set; }

        public string GameServer { get; set; }

        public string ClientExe { get; set; }

        public static LauncherConfig Load()
        {
            var currentLocation = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "launcher_config.json"));
            if (File.Exists(currentLocation))
            {
#if DEBUG
                Console.WriteLine($"Reading config from: {currentLocation}");
#endif
                LauncherConfig config = null;
                string content = System.IO.File.ReadAllText(currentLocation);
                try
                {
                    config = JsonConvert.DeserializeObject<LauncherConfig>(content);
                } catch (JsonException jsonError)
                {
                    Console.WriteLine($"Error in Config, please check: {jsonError.Message}");
                }
                return config;
            }
            return null;
        }
    }
}
