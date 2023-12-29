using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Frame
    {
        [JsonPropertyName("origin")]
        public XYZ Position { get; set; }

        [JsonPropertyName("angles")]
        public Quaternion Rotations { get; set; }
    }
}
