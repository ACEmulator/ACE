using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACE.Common
{
    public class TransferConfiguration
    {
        /// <summary>
        /// List of server thumbprints this server will accept character migrations from
        /// </summary>
        public List<string> AllowMigrationFrom { get; set; }

        /// <summary>
        /// List of server thumbprints this server will accept character imports from
        /// </summary>
        public List<string> AllowImportFrom { get; set; }

        /// <summary>
        /// Used to form a URI for use with character transfers.
        /// </summary>
        public string ExternalIPAddressOrDNSName { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowBackup { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowImport { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowMigrate { get; set; }
    }
}
