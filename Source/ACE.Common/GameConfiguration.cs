using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACE.Common
{
    public class GameConfiguration
    {
        /// <summary>
        /// maximum characters per account, could possibly be moved to the shard.account table?
        /// </summary>
        public const uint SlotCount = 11;

        /// <summary>
        /// The list of allowable characters for character names
        /// </summary>
        public const string AllowedCharacterNameCharacters = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const int CharacterNameMinimumLength = 1;
        public const int CharacterNameMaximumLength = 32;

        public string WorldName { get; set; }

        public string Description { get; set; }
        public NetworkSettings Network { get; set; }

        public AccountDefaults Accounts { get; set; }

        public string DatFilesDirectory { get; set; }

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

        public CertificateConfiguration CertificateConfiguration { get; set; }
    }
}
