
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class WieldedTreasure
    {
        [JsonProperty("continuesPreviousSet")]
        public bool ContinuesPreviousSet { get; set; }

        [JsonProperty("setStart")]
        public bool SetStart { get; set; }

        [JsonProperty("hasSubSet")]
        public bool HasSubSet { get; set; }

        [JsonProperty("probability")]
        public float Probability { get; set; }

        [JsonProperty("stackSize")]
        public int StackSize { get; set; }

        [JsonProperty("stackSizeVariance")]
        public float StackSizeVariance { get; set; }

        [JsonProperty("paletteId")]
        public uint PaletteId { get; set; }

        [JsonProperty("shade")]
        public float Shade { get; set; }

        [JsonProperty("unknown1")]
        public uint Unknown1 { get; set; }

        [JsonProperty("unknown3")]
        public uint Unknown3 { get; set; }

        [JsonProperty("unknown4")]
        public uint Unknown4 { get; set; }

        [JsonProperty("unknown5")]
        public uint Unknown5 { get; set; }

        [JsonProperty("unknown9")]
        public uint Unknown9 { get; set; }

        [JsonProperty("unknown10")]
        public uint Unknown10 { get; set; }

        [JsonProperty("unknown11")]
        public uint Unknown11 { get; set; }

        [JsonProperty("unknown12")]
        public uint Unknown12 { get; set; }

        [JsonProperty("weenieClassId")]
        public uint WeenieClassId { get; set; }
    }
}
