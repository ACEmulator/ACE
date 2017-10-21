using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.CmdLineLauncher
{
    public class LauncherConfig
    {
        public LauncherConfig()
        { 
        }

        public string GameApi { get; set; }

        public string LoginServer { get; set; }

        public string GameServer { get; set; }

        public string ClientExe { get; set; }

        public static LauncherConfig Load()
        {
            string content = System.IO.File.ReadAllText("launcher_config.json");
            var config = JsonConvert.DeserializeObject<LauncherConfig>(content);
            return config;
        }
    }
}
