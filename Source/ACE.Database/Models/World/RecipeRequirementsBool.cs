using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Recipe Bool Requirments
/// </summary>
public partial class RecipeRequirementsBool
{
    /// <summary>
    /// Unique Id of this Recipe Requirement instance
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Unique Id of Recipe
    /// </summary>
    public uint RecipeId { get; set; }

    public sbyte Index { get; set; }

    public int Stat { get; set; }

    public bool Value { get; set; }

    public int Enum { get; set; }

    public string Message { get; set; }

    public virtual Recipe Recipe { get; set; }
}
