
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class StatMod
    {
        [JsonProperty("key")]
        public uint Key { get; set; }

        [JsonProperty("type")]
        public uint Type { get; set; }

        [JsonProperty("val")]
        public double Val { get; set; }
    }
}
