using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class LandblockValue
    {
        [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
        public List<LandblockLink> Links { get; set; }

        [JsonProperty("weenies")]
        public List<LandblockWeeny> Weenies { get; set; }
    }
}
