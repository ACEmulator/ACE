using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Mod
    {
        [JsonProperty("ModificationScriptId")]
        public int ModificationScriptId { get; set; }

        [JsonProperty("ModifyHealth")]
        public int ModifyHealth { get; set; }

        [JsonProperty("ModifyMana")]
        public int ModifyMana { get; set; }

        [JsonProperty("ModifyStamina")]
        public int ModifyStamina { get; set; }

        [JsonProperty("RequiresHealth")]
        public int RequiresHealth { get; set; }

        [JsonProperty("RequiresMana")]
        public int RequiresMana { get; set; }

        [JsonProperty("RequiresStamina")]
        public int RequiresStamina { get; set; }

        [JsonProperty("Unknown10")]
        public int Unknown10 { get; set; }

        [JsonProperty("Unknown7")]
        public bool Unknown7 { get; set; }

        [JsonProperty("Unknown9")]
        public int Unknown9 { get; set; }


        [JsonProperty("DIDRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<DoubleRequirement> DidRequirements { get; set; }

        [JsonProperty("FloatRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<DoubleRequirement> FloatRequirements { get; set; }

        [JsonProperty("StringRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<StringRequirement> StringRequirements { get; set; }

        [JsonProperty("BoolRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<DoubleRequirement> BoolRequirements { get; set; }

        [JsonProperty("IntRequirements", NullValueHandling = NullValueHandling.Ignore)]
        public List<DoubleRequirement> IntRequirements { get; set; }
    }
}
