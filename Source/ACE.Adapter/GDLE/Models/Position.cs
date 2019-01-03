
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Position
    {
        [JsonProperty("objcell_id")]
        public uint ObjCellId { get; set; }

        [JsonProperty("frame")]
        public Frame Frame { get; set; }
    }
}
