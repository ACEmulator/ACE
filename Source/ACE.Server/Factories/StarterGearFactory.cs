using System.IO;
using System.Reflection;
using ACE.Server.Entity;
using Newtonsoft.Json;

namespace ACE.Server.Factories
{
    public class StarterGearFactory
    {
        private static StarterGearConfiguration _config = LoadConfigFromResource();

        private static StarterGearConfiguration LoadConfigFromResource()
        {
            StarterGearConfiguration config;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ACE.Server.starterGear.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string contents = reader.ReadToEnd();
                config = JsonConvert.DeserializeObject<StarterGearConfiguration>(contents);
            }

            return config;
        }

        public static StarterGearConfiguration GetStarterGearConfiguration()
        {
            return _config;
        }
    }
}
