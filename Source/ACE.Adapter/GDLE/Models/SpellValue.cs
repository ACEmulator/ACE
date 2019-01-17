using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class SpellValue
    {
        [JsonProperty("base_mana")]
        public uint BaseMana { get; set; }

        [JsonProperty("base_range_constant")]
        public double BaseRangeConstant { get; set; }

        [JsonProperty("base_range_mod")]
        public double BaseRangeMod { get; set; }

        [JsonProperty("bitfield")]
        public uint Bitfield { get; set; }

        [JsonProperty("caster_effect")]
        public uint CasterEffect { get; set; }

        [JsonProperty("category")]
        public uint Category { get; set; }

        [JsonProperty("component_loss")]
        public double ComponentLoss { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("display_order")]
        public uint DisplayOrder { get; set; }

        [JsonProperty("fizzle_effect")]
        public uint FizzleEffect { get; set; }

        [JsonProperty("formula")]
        public List<int> Formula { get; set; }

        [JsonProperty("formula_version")]
        public uint FormulaVersion { get; set; }

        [JsonProperty("iconID")]
        public uint IconId { get; set; }

        [JsonProperty("mana_mod")]
        public uint ManaMod { get; set; }

        [JsonProperty("meta_spell")]
        public MetaSpell MetaSpell { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("non_component_target_type")]
        public uint NonComponentTargetType { get; set; }

        [JsonProperty("power")]
        public uint Power { get; set; }

        [JsonProperty("recovery_amount")]
        public uint RecoveryAmount { get; set; }

        [JsonProperty("recovery_interval")]
        public uint RecoveryInterval { get; set; }

        [JsonProperty("school")]
        public int School { get; set; }

        [JsonProperty("spell_economy_mod")]
        public uint SpellEconomyMod { get; set; }

        [JsonProperty("target_effect")]
        public uint TargetEffect { get; set; }
    }
}
