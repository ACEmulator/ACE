using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Recipe Mods
/// </summary>
public partial class RecipeMod
{
    /// <summary>
    /// Unique Id of this Recipe Mod instance
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Unique Id of Recipe
    /// </summary>
    public uint RecipeId { get; set; }

    public bool ExecutesOnSuccess { get; set; }

    public int Health { get; set; }

    public int Stamina { get; set; }

    public int Mana { get; set; }

    public bool Unknown7 { get; set; }

    public int DataId { get; set; }

    public int Unknown9 { get; set; }

    public int InstanceId { get; set; }

    public virtual Recipe Recipe { get; set; }

    public virtual ICollection<RecipeModsBool> RecipeModsBool { get; set; } = new List<RecipeModsBool>();

    public virtual ICollection<RecipeModsDID> RecipeModsDID { get; set; } = new List<RecipeModsDID>();

    public virtual ICollection<RecipeModsFloat> RecipeModsFloat { get; set; } = new List<RecipeModsFloat>();

    public virtual ICollection<RecipeModsIID> RecipeModsIID { get; set; } = new List<RecipeModsIID>();

    public virtual ICollection<RecipeModsInt> RecipeModsInt { get; set; } = new List<RecipeModsInt>();

    public virtual ICollection<RecipeModsString> RecipeModsString { get; set; } = new List<RecipeModsString>();
}
