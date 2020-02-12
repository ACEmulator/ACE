using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class IntRequirement
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Unknown { get; set; }

        public int OperationType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public int Stat { get; set; }
        public int Value { get; set; }
    }
}
