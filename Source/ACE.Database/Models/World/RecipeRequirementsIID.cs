using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Recipe IID Requirments
/// </summary>
public partial class RecipeRequirementsIID
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

    public uint Value { get; set; }

    public int Enum { get; set; }

    public string Message { get; set; }

    public virtual Recipe Recipe { get; set; }
}
