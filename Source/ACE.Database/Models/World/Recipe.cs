using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Recipes
/// </summary>
public partial class Recipe
{
    /// <summary>
    /// Unique Id of this Recipe
    /// </summary>
    public uint Id { get; set; }

    public uint Unknown1 { get; set; }

    public uint Skill { get; set; }

    public uint Difficulty { get; set; }

    public uint SalvageType { get; set; }

    /// <summary>
    /// Weenie Class Id of object to create upon successful application of this recipe
    /// </summary>
    public uint SuccessWCID { get; set; }

    /// <summary>
    /// Amount of objects to create upon successful application of this recipe
    /// </summary>
    public uint SuccessAmount { get; set; }

    public string SuccessMessage { get; set; }

    /// <summary>
    /// Weenie Class Id of object to create upon failing application of this recipe
    /// </summary>
    public uint FailWCID { get; set; }

    /// <summary>
    /// Amount of objects to create upon failing application of this recipe
    /// </summary>
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

    public virtual ICollection<CookBook> CookBook { get; set; } = new List<CookBook>();

    public virtual ICollection<RecipeMod> RecipeMod { get; set; } = new List<RecipeMod>();

    public virtual ICollection<RecipeRequirementsBool> RecipeRequirementsBool { get; set; } = new List<RecipeRequirementsBool>();

    public virtual ICollection<RecipeRequirementsDID> RecipeRequirementsDID { get; set; } = new List<RecipeRequirementsDID>();

    public virtual ICollection<RecipeRequirementsFloat> RecipeRequirementsFloat { get; set; } = new List<RecipeRequirementsFloat>();

    public virtual ICollection<RecipeRequirementsIID> RecipeRequirementsIID { get; set; } = new List<RecipeRequirementsIID>();

    public virtual ICollection<RecipeRequirementsInt> RecipeRequirementsInt { get; set; } = new List<RecipeRequirementsInt>();

    public virtual ICollection<RecipeRequirementsString> RecipeRequirementsString { get; set; } = new List<RecipeRequirementsString>();
}
