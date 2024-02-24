using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class SpellValue
    {
        [JsonPropertyName("base_mana")]
        public uint BaseMana { get; set; }

        [JsonPropertyName("base_range_constant")]
        public double BaseRangeConstant { get; set; }

        [JsonPropertyName("base_range_mod")]
        public double BaseRangeMod { get; set; }

        [JsonPropertyName("bitfield")]
        public uint Bitfield { get; set; }

        [JsonPropertyName("caster_effect")]
        public uint CasterEffect { get; set; }

        [JsonPropertyName("category")]
        public uint Category { get; set; }

        [JsonPropertyName("component_loss")]
        public double ComponentLoss { get; set; }

        [JsonPropertyName("desc")]
        public string Desc { get; set; }

        [JsonPropertyName("display_order")]
        public uint DisplayOrder { get; set; }

        [JsonPropertyName("fizzle_effect")]
        public uint FizzleEffect { get; set; }

        [JsonPropertyName("formula")]
        public List<int> Formula { get; set; }

        [JsonPropertyName("formula_version")]
        public uint FormulaVersion { get; set; }

        [JsonPropertyName("iconID")]
        public uint IconId { get; set; }

        [JsonPropertyName("mana_mod")]
        public uint ManaMod { get; set; }

        [JsonPropertyName("meta_spell")]
        public MetaSpell MetaSpell { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("non_component_target_type")]
        public uint NonComponentTargetType { get; set; }

        [JsonPropertyName("power")]
        public uint Power { get; set; }

        [JsonPropertyName("recovery_amount")]
        public uint RecoveryAmount { get; set; }

        [JsonPropertyName("recovery_interval")]
        public uint RecoveryInterval { get; set; }

        [JsonPropertyName("school")]
        public int School { get; set; }

        [JsonPropertyName("spell_economy_mod")]
        public uint SpellEconomyMod { get; set; }

        [JsonPropertyName("target_effect")]
        public uint TargetEffect { get; set; }
    }
}
