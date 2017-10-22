using ACE.Common;
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
    public static class ServiceConfig {
        public static void LoadServiceConfig()
        {
            string debugPath = @"..\..\..\..\ACE\Config.json"; // default path for debug
            string path = "Config.json"; // default path for user installations

            if (!File.Exists(path) && File.Exists(debugPath))
                path = debugPath;

            var config = JsonConvert.DeserializeObject<MasterConfiguration>(File.ReadAllText(path));

            ConfigManager.Initialize(config);
        }
    }
}
