using Newtonsoft.Json;

namespace ACE.Common
{
    public struct HeartbeatDefaults
    {
        /// <summary>
        /// Whether hearbeats are enabled
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Enabled { get; set; }

        /// <summary>
        /// The heartbeat endpoint
        /// </summary>
        [System.ComponentModel.DefaultValue("https://heartbeat.treestats.net")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Endpoint { get; set; }

        /// <summary>
        /// The heartbeat interval, in seconds
        /// </summary>
        [System.ComponentModel.DefaultValue(500)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Interval { get; set; }
    }
}
