using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Models
{
    /// <summary>
    ///
    /// </summary>
    public class ServerInformation
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("loginServer")]
        public string LoginServer { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("requiresAuthentication")]
        public bool RequiresAuthentication { get; set; }
    }
}
