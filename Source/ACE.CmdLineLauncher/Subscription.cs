using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.CmdLineLauncher
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
