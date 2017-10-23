using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Models
{
    public class ServerInformation
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("loginServer")]
        public string LoginServer { get; set; }

        [JsonProperty("requiresAuthentication")]
        public bool RequiresAuthentication { get; set; }
    }
}
