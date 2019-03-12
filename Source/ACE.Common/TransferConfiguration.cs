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

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowBackup { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowImport { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowMigrate { get; set; }

        [System.ComponentModel.DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int KeepMigrationsForDays { get; set; }
    }
}
