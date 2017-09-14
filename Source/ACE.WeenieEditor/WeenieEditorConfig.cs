using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.WeenieEditor
{
    public class WeenieEditorConfig
    {
        public WeenieEditorConfig()
        {
            // defaults
            RootUsername = "root";
            RootPassword = "";
            ServerIp = "127.0.0.1";
            ServerPort = "3306";
            WorldDatabaseName = "ace_world";
        }

        [JsonProperty("rootUsername")]
        public string RootUsername { get; set; } 

        [JsonProperty("rootPassword")]
        public string RootPassword { get; set; }

        [JsonProperty("serverIp")]
        public string ServerIp { get; set; }

        [JsonProperty("serverPort")]
        public string ServerPort { get; set; } 

        [JsonProperty("worldDatabaseName")]
        public string WorldDatabaseName { get; set; }

        public static WeenieEditorConfig Load()
        {
            WeenieEditorConfig config = null;

            if (File.Exists("config.json"))
            {
                string content = File.ReadAllText("config.json");
                config = JsonConvert.DeserializeObject<WeenieEditorConfig>(content);
            }
            else
            {
                config = new WeenieEditorConfig();
            }

            return config;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText("config.json", json);
        }
    }
}
