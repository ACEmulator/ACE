
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class StringRequirement
    {
        [JsonProperty("OperationType")]
        public int OperationType { get; set; }

        [JsonProperty("Stat")]
        public int Stat { get; set; }

        [JsonProperty("Unknown")]
        public int Unknown { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
