using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.Common
{
    public class GameConfiguration
    {
        public string WorldName { get; set; }

        public string Description { get; set; }
        
        public string Welcome { get; set; }

        public NetworkSettings Network { get; set; }

        public AccountDefaults Accounts { get; set; }

        public string DatFilesDirectory { get; set; }

        /// <summary>
        /// The ammount of seconds to wait before turning off the server. Default value is 60 (for 1 minute).
        /// </summary>
        [System.ComponentModel.DefaultValue(60)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public uint ShutdownInterval { get; set; }
    }
}
