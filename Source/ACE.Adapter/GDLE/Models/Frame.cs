
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Frame
    {
        [JsonProperty("angles")]
        public Angles Angles { get; set; }

        [JsonProperty("origin")]
        public Origin Origin { get; set; }
    }
}
