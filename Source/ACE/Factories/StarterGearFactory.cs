using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;
using Newtonsoft.Json;

namespace ACE.Factories
{
    public class StarterGearFactory
    {
        private static StarterGearConfiguration _config = LoadConfigFromResource();

        private static StarterGearConfiguration LoadConfigFromResource()
        {
            StarterGearConfiguration config;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "starterGear.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string contents = reader.ReadToEnd();
                config = JsonConvert.DeserializeObject<StarterGearConfiguration>(contents);
            }

            return config;
        }

        public static void GrantStarterItems(IPlayer player)
        {
            // TODO: implement
        }
    }
}
