using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Angles
    {
        [JsonPropertyName("w")]
        public float W { get; set; }

        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        [JsonPropertyName("z")]
        public float Z { get; set; }
    }
}
