using System;
using System.IO;
using System.Reflection;

using ACE.Server.Entity;

using log4net;
using Newtonsoft.Json;

namespace ACE.Server.Factories
{
    public class StarterGearFactory
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static StarterGearConfiguration _config = LoadConfigFromResource();

        private static StarterGearConfiguration LoadConfigFromResource()
        {
            // Look for the starterGear.json first in the current environment directory, then in the ExecutingAssembly location
            var exeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var containerConfigDirectory = "/ace/Config";

            var starterGearFileName = "starterGear.json";

            var starterGearFile = Path.Combine(exeLocation, starterGearFileName);
            var starterGearFileContainer = Path.Combine(containerConfigDirectory, starterGearFileName);

            if (Program.IsRunningInContainer && File.Exists(starterGearFileContainer))
                File.Copy(starterGearFileContainer, starterGearFile, true);

            StarterGearConfiguration config;

            try
            {
                var starterGearText = File.ReadAllText(starterGearFile);

                config = JsonConvert.DeserializeObject<StarterGearConfiguration>(starterGearText);

                return config;
            }
            catch (Exception ex)
            {
                log.Error($"StarterGearFactory.LoadConfigFromResource() - {ex}: {ex.Message}; {ex.InnerException}: {ex.InnerException.Message}");
            }

            return null;
        }

        public static StarterGearConfiguration GetStarterGearConfiguration()
        {
            try
            {
                return _config;
            }
            catch
            {
                return null;
            }
        }
    }
}
