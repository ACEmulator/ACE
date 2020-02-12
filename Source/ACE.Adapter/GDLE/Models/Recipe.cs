using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Recipe
    {
        [JsonProperty("RecipeID")]
        public uint RecipeId { get; set; }

        [JsonProperty("Skill")]
        public uint Skill { get; set; }

        [JsonProperty("SkillCheckFormulaType")]
        public int SkillCheckFormulaType { get; set; }

        [JsonProperty("DataID")]
        public uint DataId { get; set; }

        [JsonProperty("Difficulty")]
        public uint Difficulty { get; set; }

        [JsonProperty("SuccessWcid")]
        public uint SuccessWcid { get; set; }

        [JsonProperty("SuccessAmount")]
        public uint SuccessAmount { get; set; }

        [JsonProperty("SuccessMessage")]
        public string SuccessMessage { get; set; }

        [JsonProperty("SuccessConsumeTargetAmount")]
        public uint SuccessConsumeTargetAmount { get; set; }

        [JsonProperty("SuccessConsumeTargetChance")]
        public int SuccessConsumeTargetChance { get; set; }

        [JsonProperty("SuccessConsumeTargetMessage")]
        public string SuccessConsumeTargetMessage { get; set; }

        [JsonProperty("SuccessConsumeToolAmount")]
        public uint SuccessConsumeToolAmount { get; set; }

        [JsonProperty("SuccessConsumeToolChance")]
        public int SuccessConsumeToolChance { get; set; }

        [JsonProperty("SuccessConsumeToolMessage")]
        public string SuccessConsumeToolMessage { get; set; }

        [JsonProperty("FailWcid")]
        public uint FailWcid { get; set; }

        [JsonProperty("FailAmount")]
        public uint FailAmount { get; set; }

        [JsonProperty("FailMessage")]
        public string FailMessage { get; set; }

        [JsonProperty("FailureConsumeTargetAmount")]
        public uint FailureConsumeTargetAmount { get; set; }

        [JsonProperty("FailureConsumeTargetChance")]
        public int FailureConsumeTargetChance { get; set; }

        [JsonProperty("FailureConsumeTargetMessage")]
        public string FailureConsumeTargetMessage { get; set; }

        [JsonProperty("FailureConsumeToolAmount")]
        public uint FailureConsumeToolAmount { get; set; }

        [JsonProperty("FailureConsumeToolChance")]
        public int FailureConsumeToolChance { get; set; }

        [JsonProperty("FailureConsumeToolMessage")]
        public string FailureConsumeToolMessage { get; set; }

        [JsonProperty("Unknown")]
        public int Unknown { get; set; }

        [JsonProperty("Mods")]
        public List<Mod> Mods { get; set; }

        [JsonProperty("Requirements")]
        public List<RecipeRequirements> Requirements { get; set; }
    }
}
