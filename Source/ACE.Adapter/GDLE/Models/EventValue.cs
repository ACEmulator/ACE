
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class EventValue
    {
        [JsonPropertyName("startTime")]
        public int StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public int EndTime { get; set; }

        [JsonPropertyName("eventState")]
        public int EventState { get; set; }
    }
}
