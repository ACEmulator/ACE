using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Common
{
    public class ServiceConfig
    {
        public ServiceConfig()
        {
        }

        public string ApiBindAddress { get; set; }

        public string ApiBindPort { get; set; }

        public static ServiceConfig Load()
        {
            var currentLocation = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "service_config.json"));
            if (File.Exists(currentLocation))
            {
#if DEBUG
                Console.WriteLine($"Reading config from: {currentLocation}");
#endif
                ServiceConfig config = null;
                string content = System.IO.File.ReadAllText(currentLocation);
                try
                {
                    config = JsonConvert.DeserializeObject<ServiceConfig>(content);
                }
                catch (JsonException jsonError)
                {
                    Console.WriteLine($"Error in Config, please check: {jsonError.Message}");
                }
                return config;
            }
            return null;
        }
    }
}
