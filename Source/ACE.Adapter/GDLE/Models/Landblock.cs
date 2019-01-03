
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Landblock
    {
        [JsonProperty("key")]
        public uint Key { get; set; }

        [JsonProperty("value")]
        public LandblockValue Value { get; set; }
    }
}
