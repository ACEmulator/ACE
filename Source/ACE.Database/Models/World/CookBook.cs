using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Cook Book for Recipes
/// </summary>
public partial class CookBook
{
    /// <summary>
    /// Unique Id of this cook book instance
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Unique Id of Recipe
    /// </summary>
    public uint RecipeId { get; set; }

    /// <summary>
    /// Weenie Class Id of the source object for this recipe
    /// </summary>
    public uint SourceWCID { get; set; }

    /// <summary>
    /// Weenie Class Id of the target object for this recipe
    /// </summary>
    public uint TargetWCID { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Recipe Recipe { get; set; }
}
