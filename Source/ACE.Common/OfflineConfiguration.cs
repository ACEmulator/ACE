using System;

using Newtonsoft.Json;

namespace ACE.Common
{
    /// <summary>
    /// This section can trigger events that may happen before the world starts up, or after it shuts down
    /// The shard should be in a disconnected state from any active ACE world
    /// </summary>
    public class OfflineConfiguration
    {
        /// <summary>
        /// Purge characters that have been deleted longer than PruneDeletedCharactersDays
        /// These characters, and their associated biotas, will be deleted permanantly!
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool PurgeDeletedCharacters { get; set; }

        /// <summary>
        /// Number of days a character must have been deleted for before eligible for purging
        /// </summary>
        [System.ComponentModel.DefaultValue(30)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int PurgeDeletedCharactersDays { get; set; } = 30;

        /// <summary>
        /// This will purge biotas that are completely disconnected from the world
        /// These may have been items that were never deleted properly, items that were given to the town crier before delete was implemented, etc...
        /// This can be time consuming so it's not something you would have set to true for every server startup. You might run this once every few months
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool PurgeOrphanedBiotas { get; set; }
    }
}
