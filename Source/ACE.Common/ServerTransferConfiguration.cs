using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACE.Common
{
    public class ServerTransferConfiguration
    {
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
        public bool AllowExport { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowImport { get; set; }

        [System.ComponentModel.DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool DeleteUponExport { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowRepeatedImport { get; set; }
    }
}
