
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class WieldedTreasure
    {
        [JsonPropertyName("continuesPreviousSet")]
        public bool ContinuesPreviousSet { get; set; }

        [JsonPropertyName("setStart")]
        public bool SetStart { get; set; }

        [JsonPropertyName("hasSubSet")]
        public bool HasSubSet { get; set; }

        [JsonPropertyName("probability")]
        public float Probability { get; set; }

        [JsonPropertyName("stackSize")]
        public int StackSize { get; set; }

        [JsonPropertyName("stackSizeVariance")]
        public float StackSizeVariance { get; set; }

        [JsonPropertyName("paletteId")]
        public uint PaletteId { get; set; }

        [JsonPropertyName("shade")]
        public float Shade { get; set; }

        [JsonPropertyName("unknown1")]
        public uint Unknown1 { get; set; }

        [JsonPropertyName("unknown3")]
        public uint Unknown3 { get; set; }

        [JsonPropertyName("unknown4")]
        public uint Unknown4 { get; set; }

        [JsonPropertyName("unknown5")]
        public uint Unknown5 { get; set; }

        [JsonPropertyName("unknown9")]
        public uint Unknown9 { get; set; }

        [JsonPropertyName("unknown10")]
        public uint Unknown10 { get; set; }

        [JsonPropertyName("unknown11")]
        public uint Unknown11 { get; set; }

        [JsonPropertyName("unknown12")]
        public uint Unknown12 { get; set; }

        [JsonPropertyName("weenieClassId")]
        public uint WeenieClassId { get; set; }
    }
}
