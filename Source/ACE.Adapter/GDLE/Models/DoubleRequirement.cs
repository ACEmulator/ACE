
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class DoubleRequirement
    {
        [JsonProperty("OperationType")]
        public int OperationType { get; set; }

        [JsonProperty("Stat")]
        public int Stat { get; set; }

        [JsonProperty("Unknown", NullValueHandling = NullValueHandling.Ignore)]
        public int? Unknown { get; set; }

        [JsonProperty("Value")]
        public double Value { get; set; }

        [JsonProperty("Message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
