using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.AuthApi.Controllers
{
    public class ServerInfo
    {
        /// <summary>
        /// hoste name (DNS) or ip address
        /// </summary>
        [JsonProperty("hostName")]
        public string HostName { get; set; }

        [JsonProperty("gameServerPort")]
        public int GameServerPort { get; set; }

        [JsonProperty("apiPort")]
        public int ApiPort { get; set; }

        /// <summary>
        /// name of the server that will be displayed in the list
        /// </summary>
        [JsonProperty("serverName")]
        public string ServerName { get; set; }

        /// <summary>
        /// description giving more information about the server (such as server options, etc)
        /// </summary>
        [JsonProperty("serverDescription")]
        public string ServerDescription { get; set; }
    }
}
