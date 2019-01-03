
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Quest
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public QuestValue Value { get; set; }
    }
}
