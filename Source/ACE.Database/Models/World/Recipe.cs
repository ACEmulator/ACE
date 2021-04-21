using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Recipe
    {
        public Recipe()
        {
            CookBook = new HashSet<CookBook>();
            RecipeMod = new HashSet<RecipeMod>();
            RecipeRequirementsBool = new HashSet<RecipeRequirementsBool>();
            RecipeRequirementsDID = new HashSet<RecipeRequirementsDID>();
            RecipeRequirementsFloat = new HashSet<RecipeRequirementsFloat>();
            RecipeRequirementsIID = new HashSet<RecipeRequirementsIID>();
            RecipeRequirementsInt = new HashSet<RecipeRequirementsInt>();
            RecipeRequirementsString = new HashSet<RecipeRequirementsString>();
        }

        public uint Id { get; set; }
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
        public double SuccessDestroySourceChance { get; set; }
        public uint SuccessDestroySourceAmount { get; set; }
        public string SuccessDestroySourceMessage { get; set; }
        public double SuccessDestroyTargetChance { get; set; }
        public uint SuccessDestroyTargetAmount { get; set; }
        public string SuccessDestroyTargetMessage { get; set; }
        public double FailDestroySourceChance { get; set; }
        public uint FailDestroySourceAmount { get; set; }
        public string FailDestroySourceMessage { get; set; }
        public double FailDestroyTargetChance { get; set; }
        public uint FailDestroyTargetAmount { get; set; }
        public string FailDestroyTargetMessage { get; set; }
        public uint DataId { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<CookBook> CookBook { get; set; }
        public virtual ICollection<RecipeMod> RecipeMod { get; set; }
        public virtual ICollection<RecipeRequirementsBool> RecipeRequirementsBool { get; set; }
        public virtual ICollection<RecipeRequirementsDID> RecipeRequirementsDID { get; set; }
        public virtual ICollection<RecipeRequirementsFloat> RecipeRequirementsFloat { get; set; }
        public virtual ICollection<RecipeRequirementsIID> RecipeRequirementsIID { get; set; }
        public virtual ICollection<RecipeRequirementsInt> RecipeRequirementsInt { get; set; }
        public virtual ICollection<RecipeRequirementsString> RecipeRequirementsString { get; set; }
    }
}
