
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class StatMod
    {
        [JsonPropertyName("key")]
        public uint Key { get; set; }

        [JsonPropertyName("type")]
        public uint Type { get; set; }

        [JsonPropertyName("val")]
        public double Val { get; set; }
    }
}
