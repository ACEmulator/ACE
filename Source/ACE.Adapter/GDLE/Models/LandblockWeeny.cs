
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class LandblockWeeny
    {
        [JsonProperty("_desc")]
        public string Desc { get; set; }

        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("pos")]
        public Position Position { get; set; }

        [JsonProperty("wcid")]
        public uint WCID { get; set; }
    }
}
