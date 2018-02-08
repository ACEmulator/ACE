using Newtonsoft.Json;

namespace ACE.Common
{
    public struct AccountDefaults
    {
        /// <summary>
        /// whether or not the server uses account level permissions or character level permissions (as AC retail did). for backwards compatibility, this is true
        /// by default.
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool OverrideCharacterPermissions { get; set; }

        /// <summary>
        /// Default AccessLevel for new accounts. Used when accesslevel is not specified by user.  for backwards compatibility, this is 0 (Player)
        /// by default.
        /// </summary>
        [System.ComponentModel.DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public uint DefaultAccessLevel { get; set; }

        /// <summary>
        /// whether or not this server creates new accounts when one does not exist.  for backwards compatibility, this is true
        /// by default.
        /// </summary>
        [System.ComponentModel.DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool AllowAutoAccountCreation { get; set; }
    }
}
