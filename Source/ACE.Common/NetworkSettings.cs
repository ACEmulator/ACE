using Newtonsoft.Json;

namespace ACE.Common
{
    public class NetworkSettings
    {
        public string Host { get; set; }

        public uint Port { get; set; }

        /// <summary>
        /// Increasing this setting will allow more Accounts to connect with this server.
        /// </summary>
        /// <remarks>
        /// WARNING: Must be greater then 0 to allow users to connect.
        /// </remarks>
        [System.ComponentModel.DefaultValue(128)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public uint MaximumAllowedSessions { get; set; }

        /// <summary>
        /// The amount of seconds until an active session will be declared dead/inactive. Default value is 60 (for 1 minute).
        /// </summary>
        [System.ComponentModel.DefaultValue(60)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public uint DefaultSessionTimeout { get; set; }
    }
}
