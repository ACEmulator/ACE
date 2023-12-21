using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACE.Adapter.GDLE.Models
{
    public class Recipe
    {
        [JsonPropertyName("RecipeID")]
        public uint RecipeId { get; set; }

        [JsonPropertyName("Skill")]
        public uint Skill { get; set; }

        [JsonPropertyName("SkillCheckFormulaType")]
        public int SkillCheckFormulaType { get; set; }

        [JsonPropertyName("DataID")]
        public uint DataId { get; set; }

        [JsonPropertyName("Difficulty")]
        public uint Difficulty { get; set; }

        [JsonPropertyName("SuccessWcid")]
        public uint SuccessWcid { get; set; }

        [JsonPropertyName("SuccessAmount")]
        public uint SuccessAmount { get; set; }

        [JsonPropertyName("SuccessMessage")]
        public string SuccessMessage { get; set; }

        [JsonPropertyName("SuccessConsumeTargetAmount")]
        public uint SuccessConsumeTargetAmount { get; set; }

        [JsonPropertyName("SuccessConsumeTargetChance")]
        public double SuccessConsumeTargetChance { get; set; }

        [JsonPropertyName("SuccessConsumeTargetMessage")]
        public string SuccessConsumeTargetMessage { get; set; }

        [JsonPropertyName("SuccessConsumeToolAmount")]
        public uint SuccessConsumeToolAmount { get; set; }

        [JsonPropertyName("SuccessConsumeToolChance")]
        public double SuccessConsumeToolChance { get; set; }

        [JsonPropertyName("SuccessConsumeToolMessage")]
        public string SuccessConsumeToolMessage { get; set; }

        [JsonPropertyName("FailWcid")]
        public uint FailWcid { get; set; }

        [JsonPropertyName("FailAmount")]
        public uint FailAmount { get; set; }

        [JsonPropertyName("FailMessage")]
        public string FailMessage { get; set; }

        [JsonPropertyName("FailureConsumeTargetAmount")]
        public uint FailureConsumeTargetAmount { get; set; }

        [JsonPropertyName("FailureConsumeTargetChance")]
        public double FailureConsumeTargetChance { get; set; }

        [JsonPropertyName("FailureConsumeTargetMessage")]
        public string FailureConsumeTargetMessage { get; set; }

        [JsonPropertyName("FailureConsumeToolAmount")]
        public uint FailureConsumeToolAmount { get; set; }

        [JsonPropertyName("FailureConsumeToolChance")]
        public double FailureConsumeToolChance { get; set; }

        [JsonPropertyName("FailureConsumeToolMessage")]
        public string FailureConsumeToolMessage { get; set; }

        [JsonPropertyName("Unknown")]
        public int Unknown { get; set; }

        [JsonPropertyName("Mods")]
        public List<Mod> Mods { get; set; }

        [JsonPropertyName("Requirements")]
        public List<RecipeRequirements> Requirements { get; set; }
    }
}
