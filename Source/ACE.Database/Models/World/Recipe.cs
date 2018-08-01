using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Recipe
    {
        public Recipe()
        {
            CookBook = new HashSet<CookBook>();
            RecipeComponent = new HashSet<RecipeComponent>();
            RecipeMod = new HashSet<RecipeMod>();
            RecipeRequirementsBool = new HashSet<RecipeRequirementsBool>();
            RecipeRequirementsDID = new HashSet<RecipeRequirementsDID>();
            RecipeRequirementsFloat = new HashSet<RecipeRequirementsFloat>();
            RecipeRequirementsIID = new HashSet<RecipeRequirementsIID>();
            RecipeRequirementsInt = new HashSet<RecipeRequirementsInt>();
            RecipeRequirementsString = new HashSet<RecipeRequirementsString>();
        }

        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public uint Unknown1 { get; set; }
        public uint Skill { get; set; }
        public uint Difficulty { get; set; }
        public uint SalvageType { get; set; }
        public uint SuccessWCID { get; set; }
        public uint SuccessAmount { get; set; }
        public string SuccessMessage { get; set; }
        public uint FailWCID { get; set; }
        public uint FailAmount { get; set; }
        public string FailMessage { get; set; }
        public uint DataId { get; set; }

        public ICollection<CookBook> CookBook { get; set; }
        public ICollection<RecipeComponent> RecipeComponent { get; set; }
        public ICollection<RecipeMod> RecipeMod { get; set; }
        public ICollection<RecipeRequirementsBool> RecipeRequirementsBool { get; set; }
        public ICollection<RecipeRequirementsDID> RecipeRequirementsDID { get; set; }
        public ICollection<RecipeRequirementsFloat> RecipeRequirementsFloat { get; set; }
        public ICollection<RecipeRequirementsIID> RecipeRequirementsIID { get; set; }
        public ICollection<RecipeRequirementsInt> RecipeRequirementsInt { get; set; }
        public ICollection<RecipeRequirementsString> RecipeRequirementsString { get; set; }
    }
}
