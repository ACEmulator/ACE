
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class EventValue
    {
        [JsonProperty("startTime")]
        public int StartTime { get; set; }

        [JsonProperty("endTime")]
        public int EndTime { get; set; }

        [JsonProperty("eventState")]
        public int EventState { get; set; }
    }
}
