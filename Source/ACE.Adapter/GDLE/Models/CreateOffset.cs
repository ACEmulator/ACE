
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class CreateOffset
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }

        [JsonProperty("w", NullValueHandling = NullValueHandling.Ignore)]
        public double? W { get; set; }
    }
}
