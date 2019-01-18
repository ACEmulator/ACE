
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Origin
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }
    }
}
