using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Origin
    {
        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        [JsonPropertyName("z")]
        public float Z { get; set; }
    }
}
