
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class LandblockLink
    {
        [JsonProperty("_linkinfo")]
        public string Linkinfo { get; set; }

        [JsonProperty("source")]
        public uint Source { get; set; }

        [JsonProperty("target")]
        public uint Target { get; set; }
    }
}
