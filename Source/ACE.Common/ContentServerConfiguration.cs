using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    public class ContentServerConfiguration
    {
        /// <summary>
        /// Api URL to current Database root Folder.
        /// </summary>
        [DefaultValue("https://api.github.com/repositories/79078680/contents/Database/")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string DatabaseUrl { get; set; }

        /// <summary>
        /// Url to download the latest release archive of ACE-World.
        /// </summary>
        [DefaultValue("https://api.github.com/repos/ACEmulator/ACE-World/releases/latest")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string WorldArchiveUrl { get; set; }

        /// <summary>
        /// Local hard disk path that will be used to save the database files, retrieved from Github.
        /// </summary>
        [DefaultValue("Content\\")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string LocalContentPath { get; set; }
    }
}
