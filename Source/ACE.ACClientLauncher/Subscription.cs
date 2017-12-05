using Newtonsoft.Json;
using System;

namespace ACE.ACClientLauncher
{
    public class Subscription
    { 
        [JsonProperty("subscriptionId")]
        public uint SubscriptionId { get; set; }

        [JsonProperty("subscriptionGuid")]
        public Guid SubscriptionGuid { get; set; }

        [JsonProperty("accountGuid")]
        public Guid AccountGuid { get; set; }

        [JsonProperty("accessLevel")]
        public AccessLevel AccessLevel { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
