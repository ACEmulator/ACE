using Newtonsoft.Json;
using System.Collections.Generic;

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
        /// List of trusted server cert thumbprints this server will accept character transfers from
        /// </summary>
        public List<string> TrustedServerCertThumbprints { get; set; }

        /// <summary>
        /// Used to form a URI for use with character transfers.
        /// </summary>
        public string ExternalIPAddressOrDomainName { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool WorldDatabasePrecaching { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool LandblockPreloading { get; set; }

        public List<PreloadedLandblocks> PreloadedLandblocks { get; set; }

        /// <summary>
        /// The ammount of seconds to wait before turning off the server. Default value is 60 (for 1 minute).
        /// </summary>
        [System.ComponentModel.DefaultValue(60)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public uint ShutdownInterval { get; set; }
    }
}
