
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class QuestValue
    {
        [JsonProperty("mindelta")]
        public int MinDelta { get; set; }

        [JsonProperty("maxsolves")]
        public int MaxSolves { get; set; }

        [JsonProperty("fullname")]
        public string FullName { get; set; }
    }
}
