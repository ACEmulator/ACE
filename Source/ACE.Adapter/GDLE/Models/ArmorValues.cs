using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class ArmorValues
    {
        [JsonPropertyName("base_armor")]
        public int BaseArmor { get; set; }

        [JsonPropertyName("armor_vs_slash")]
        public int ArmorVsSlash { get; set; }

        [JsonPropertyName("armor_vs_pierce")]
        public int ArmorVsPierce { get; set; }

        [JsonPropertyName("armor_vs_bludgeon")]
        public int ArmorVsBludgeon { get; set; }

        [JsonPropertyName("armor_vs_cold")]
        public int ArmorVsCold { get; set; }

        [JsonPropertyName("armor_vs_fire")]
        public int ArmorVsFire { get; set; }

        [JsonPropertyName("armor_vs_acid")]
        public int ArmorVsAcid { get; set; }

        [JsonPropertyName("armor_vs_electric")]
        public int ArmorVsElectric { get; set; }

        [JsonPropertyName("armor_vs_nether")]
        public int ArmorVsNether { get; set; }
    }
}
