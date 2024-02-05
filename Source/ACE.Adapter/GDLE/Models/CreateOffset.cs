using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class CreateOffset
    {
        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }

        [JsonPropertyName("z")]
        public double Z { get; set; }

        [JsonPropertyName("w")]
        public double? W { get; set; }
    }
}
