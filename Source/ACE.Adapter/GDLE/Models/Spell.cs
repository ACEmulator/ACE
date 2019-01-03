
using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Spell
    {
        [JsonProperty("degrade_limit", NullValueHandling = NullValueHandling.Ignore)]
        public double? DegradeLimit { get; set; }

        [JsonProperty("degrade_modifier", NullValueHandling = NullValueHandling.Ignore)]
        public uint? DegradeModifier { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public double? Duration { get; set; }

        [JsonProperty("smod", NullValueHandling = NullValueHandling.Ignore)]
        public StatMod StatMod { get; set; }

        [JsonProperty("spellCategory", NullValueHandling = NullValueHandling.Ignore)]
        public uint? SpellCategory { get; set; }

        [JsonProperty("spell_id")]
        public uint SpellId { get; set; }

        [JsonProperty("boost", NullValueHandling = NullValueHandling.Ignore)]
        public int? Boost { get; set; }

        [JsonProperty("boostVariance", NullValueHandling = NullValueHandling.Ignore)]
        public int? BoostVariance { get; set; }

        [JsonProperty("dt", NullValueHandling = NullValueHandling.Ignore)]
        public int? DamageType { get; set; }

        [JsonProperty("bitfield", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Bitfield { get; set; }

        [JsonProperty("dest", NullValueHandling = NullValueHandling.Ignore)]
        public int? Dest { get; set; }

        [JsonProperty("lossPercent", NullValueHandling = NullValueHandling.Ignore)]
        public double? LossPercent { get; set; }

        [JsonProperty("maxBoostAllowed", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxBoostAllowed { get; set; }

        [JsonProperty("proportion", NullValueHandling = NullValueHandling.Ignore)]
        public double? Proportion { get; set; }

        [JsonProperty("sourceLoss", NullValueHandling = NullValueHandling.Ignore)]
        public int? SourceLoss { get; set; }

        [JsonProperty("src", NullValueHandling = NullValueHandling.Ignore)]
        public int? Source { get; set; }

        [JsonProperty("transferCap", NullValueHandling = NullValueHandling.Ignore)]
        public int? TransferCap { get; set; }

        [JsonProperty("bNonTracking", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NonTracking { get; set; }

        [JsonProperty("baseIntensity", NullValueHandling = NullValueHandling.Ignore)]
        public int? BaseIntensity { get; set; }

        [JsonProperty("createOffset", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOffset CreateOffset { get; set; }

        [JsonProperty("critFreq", NullValueHandling = NullValueHandling.Ignore)]
        public uint? CritFreq { get; set; }

        [JsonProperty("critMultiplier", NullValueHandling = NullValueHandling.Ignore)]
        public uint? CritMultiplier { get; set; }

        [JsonProperty("defaultLaunchAngle", NullValueHandling = NullValueHandling.Ignore)]
        public uint? DefaultLaunchAngle { get; set; }

        [JsonProperty("dims", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOffset Dims { get; set; }

        [JsonProperty("elementalModifier", NullValueHandling = NullValueHandling.Ignore)]
        public uint? ElementalModifier { get; set; }

        [JsonProperty("etype", NullValueHandling = NullValueHandling.Ignore)]
        public uint? EType { get; set; }

        [JsonProperty("ignoreMagicResist", NullValueHandling = NullValueHandling.Ignore)]
        public int? IgnoreMagicResist { get; set; }

        [JsonProperty("imbuedEffect", NullValueHandling = NullValueHandling.Ignore)]
        public uint? ImbuedEffect { get; set; }

        [JsonProperty("numProjectiles", NullValueHandling = NullValueHandling.Ignore)]
        public int? NumProjectiles { get; set; }

        [JsonProperty("numProjectilesVariance", NullValueHandling = NullValueHandling.Ignore)]
        public double? NumProjectilesVariance { get; set; }

        [JsonProperty("padding", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOffset Padding { get; set; }

        [JsonProperty("peturbation", NullValueHandling = NullValueHandling.Ignore)]
        public CreateOffset Peturbation { get; set; }

        [JsonProperty("slayerCreatureType", NullValueHandling = NullValueHandling.Ignore)]
        public int? SlayerCreatureType { get; set; }

        [JsonProperty("slayerDamageBonus", NullValueHandling = NullValueHandling.Ignore)]
        public uint? SlayerDamageBonus { get; set; }

        [JsonProperty("spreadAngle", NullValueHandling = NullValueHandling.Ignore)]
        public uint? SpreadAngle { get; set; }

        [JsonProperty("variance", NullValueHandling = NullValueHandling.Ignore)]
        public int? Variance { get; set; }

        [JsonProperty("verticalAngle", NullValueHandling = NullValueHandling.Ignore)]
        public uint? VerticalAngle { get; set; }

        [JsonProperty("wcid", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Wcid { get; set; }

        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public int? Index { get; set; }

        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public int? Link { get; set; }

        [JsonProperty("portal_lifetime", NullValueHandling = NullValueHandling.Ignore)]
        public uint? PortalLifetime { get; set; }

        [JsonProperty("pos", NullValueHandling = NullValueHandling.Ignore)]
        public Position Position { get; set; }

        [JsonProperty("align", NullValueHandling = NullValueHandling.Ignore)]
        public int? Align { get; set; }

        [JsonProperty("max_power", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxPower { get; set; }

        [JsonProperty("min_power", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinPower { get; set; }

        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public int? Number { get; set; }

        [JsonProperty("number_variance", NullValueHandling = NullValueHandling.Ignore)]
        public double? NumberVariance { get; set; }

        [JsonProperty("power_variance", NullValueHandling = NullValueHandling.Ignore)]
        public uint? PowerVariance { get; set; }

        [JsonProperty("school", NullValueHandling = NullValueHandling.Ignore)]
        public uint? School { get; set; }

        [JsonProperty("damage_ratio", NullValueHandling = NullValueHandling.Ignore)]
        public double? DamageRatio { get; set; }

        [JsonProperty("drain_percentage", NullValueHandling = NullValueHandling.Ignore)]
        public double? DrainPercentage { get; set; }
    }
}
