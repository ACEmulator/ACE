using System;

using System.Collections.Generic;
using Newtonsoft.Json;

namespace ACE.Common
{
    /// <summary>
    /// This section configures handling of client DAT patching from server's DAT files
    /// </summary>
    public class DDDConfiguration
    {
        /// <summary>
        /// Allow server to patch client DAT files using server's DAT files via DDDManager
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool EnableDATPatching { get; set; }

        /// <summary>
        /// Upon server startup, precache all DAT files that would be sent as compressed data
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool PrecacheCompressedDATFiles { get; set; }
    }
}
