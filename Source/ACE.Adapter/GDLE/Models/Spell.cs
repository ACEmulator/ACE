
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Spell
    {
        [JsonPropertyName("degrade_limit")]
        public double? DegradeLimit { get; set; }

        [JsonPropertyName("degrade_modifier")]
        public uint? DegradeModifier { get; set; }

        [JsonPropertyName("duration")]
        public double? Duration { get; set; }

        [JsonPropertyName("smod")]
        public StatMod StatMod { get; set; }

        [JsonPropertyName("spellCategory")]
        public uint? SpellCategory { get; set; }

        [JsonPropertyName("spell_id")]
        public uint SpellId { get; set; }

        [JsonPropertyName("boost")]
        public int? Boost { get; set; }

        [JsonPropertyName("boostVariance")]
        public int? BoostVariance { get; set; }

        [JsonPropertyName("dt")]
        public int? DamageType { get; set; }

        [JsonPropertyName("bitfield")]
        public uint? Bitfield { get; set; }

        [JsonPropertyName("dest")]
        public int? Dest { get; set; }

        [JsonPropertyName("lossPercent")]
        public double? LossPercent { get; set; }

        [JsonPropertyName("maxBoostAllowed")]
        public int? MaxBoostAllowed { get; set; }

        [JsonPropertyName("proportion")]
        public double? Proportion { get; set; }

        [JsonPropertyName("sourceLoss")]
        public int? SourceLoss { get; set; }

        [JsonPropertyName("src")]
        public int? Source { get; set; }

        [JsonPropertyName("transferCap")]
        public int? TransferCap { get; set; }

        [JsonPropertyName("bNonTracking")]
        public bool? NonTracking { get; set; }

        [JsonPropertyName("baseIntensity")]
        public int? BaseIntensity { get; set; }

        [JsonPropertyName("createOffset")]
        public CreateOffset CreateOffset { get; set; }

        [JsonPropertyName("critFreq")]
        public uint? CritFreq { get; set; }

        [JsonPropertyName("critMultiplier")]
        public uint? CritMultiplier { get; set; }

        [JsonPropertyName("defaultLaunchAngle")]
        public uint? DefaultLaunchAngle { get; set; }

        [JsonPropertyName("dims")]
        public CreateOffset Dims { get; set; }

        [JsonPropertyName("elementalModifier")]
        public uint? ElementalModifier { get; set; }

        [JsonPropertyName("etype")]
        public uint? EType { get; set; }

        [JsonPropertyName("ignoreMagicResist")]
        public int? IgnoreMagicResist { get; set; }

        [JsonPropertyName("imbuedEffect")]
        public uint? ImbuedEffect { get; set; }

        [JsonPropertyName("numProjectiles")]
        public int? NumProjectiles { get; set; }

        [JsonPropertyName("numProjectilesVariance")]
        public double? NumProjectilesVariance { get; set; }

        [JsonPropertyName("padding")]
        public CreateOffset Padding { get; set; }

        [JsonPropertyName("peturbation")]
        public CreateOffset Peturbation { get; set; }

        [JsonPropertyName("slayerCreatureType")]
        public int? SlayerCreatureType { get; set; }

        [JsonPropertyName("slayerDamageBonus")]
        public uint? SlayerDamageBonus { get; set; }

        [JsonPropertyName("spreadAngle")]
        public uint? SpreadAngle { get; set; }

        [JsonPropertyName("variance")]
        public int? Variance { get; set; }

        [JsonPropertyName("verticalAngle")]
        public uint? VerticalAngle { get; set; }

        [JsonPropertyName("wcid")]
        public uint? Wcid { get; set; }

        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonPropertyName("link")]
        public int? Link { get; set; }

        [JsonPropertyName("portal_lifetime")]
        public uint? PortalLifetime { get; set; }

        [JsonPropertyName("pos")]
        public Position Position { get; set; }

        [JsonPropertyName("align")]
        public int? Align { get; set; }

        [JsonPropertyName("max_power")]
        public int? MaxPower { get; set; }

        [JsonPropertyName("min_power")]
        public int? MinPower { get; set; }

        [JsonPropertyName("number")]
        public int? Number { get; set; }

        [JsonPropertyName("number_variance")]
        public double? NumberVariance { get; set; }

        [JsonPropertyName("power_variance")]
        public uint? PowerVariance { get; set; }

        [JsonPropertyName("school")]
        public uint? School { get; set; }

        [JsonPropertyName("damage_ratio")]
        public double? DamageRatio { get; set; }

        [JsonPropertyName("drain_percentage")]
        public double? DrainPercentage { get; set; }
    }
}
