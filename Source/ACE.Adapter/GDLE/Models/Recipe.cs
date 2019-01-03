using System.Collections.Generic;

using Newtonsoft.Json;

namespace ACE.Adapter.GDLE.Models
{
    public class Recipe
    {
        [JsonProperty("Mods")]
        public List<Mod> Mods { get; set; }

        [JsonProperty("Requirements")]
        public List<RequirementClass> Requirements { get; set; }

        [JsonProperty("SkillCheckFormulaType")]
        public int SkillCheckFormulaType { get; set; }

        [JsonProperty("DataID")]
        public int DataId { get; set; }

        [JsonProperty("Difficulty")]
        public int Difficulty { get; set; }

        [JsonProperty("FailAmount")]
        public int FailAmount { get; set; }

        [JsonProperty("FailMessage")]
        public string FailMessage { get; set; }

        [JsonProperty("FailWcid")]
        public int FailWcid { get; set; }

        [JsonProperty("FailureConsumeTargetAmount")]
        public int FailureConsumeTargetAmount { get; set; }

        [JsonProperty("FailureConsumeTargetChance")]
        public int FailureConsumeTargetChance { get; set; }

        [JsonProperty("FailureConsumeTargetMessage")]
        public string FailureConsumeTargetMessage { get; set; }

        [JsonProperty("FailureConsumeToolAmount")]
        public int FailureConsumeToolAmount { get; set; }

        [JsonProperty("FailureConsumeToolChance")]
        public int FailureConsumeToolChance { get; set; }

        [JsonProperty("FailureConsumeToolMessage")]
        public string FailureConsumeToolMessage { get; set; }

        [JsonProperty("RecipeID")]
        public int RecipeId { get; set; }

        [JsonProperty("Skill")]
        public int Skill { get; set; }

        [JsonProperty("SuccessAmount")]
        public int SuccessAmount { get; set; }

        [JsonProperty("SuccessConsumeTargetAmount")]
        public int SuccessConsumeTargetAmount { get; set; }

        [JsonProperty("SuccessConsumeTargetChance")]
        public int SuccessConsumeTargetChance { get; set; }

        [JsonProperty("SuccessConsumeTargetMessage")]
        public string SuccessConsumeTargetMessage { get; set; }

        [JsonProperty("SuccessConsumeToolAmount")]
        public int SuccessConsumeToolAmount { get; set; }

        [JsonProperty("SuccessConsumeToolChance")]
        public int SuccessConsumeToolChance { get; set; }

        [JsonProperty("SuccessConsumeToolMessage")]
        public string SuccessConsumeToolMessage { get; set; }

        [JsonProperty("SuccessMessage")]
        public string SuccessMessage { get; set; }

        [JsonProperty("SuccessWcid")]
        public int SuccessWcid { get; set; }

        [JsonProperty("Unknown")]
        public int Unknown { get; set; }
    }
}
