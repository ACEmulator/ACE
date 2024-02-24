using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Position
    {
        [JsonPropertyName("frame")]
        public Frame Frame { get; set; }

        [JsonPropertyName("objcell_id")]
        public uint LandCellId { get; set; }
    }
}
