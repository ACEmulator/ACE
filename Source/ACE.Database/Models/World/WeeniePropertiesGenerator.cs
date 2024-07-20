using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Generator Properties of Weenies
/// </summary>
public partial class WeeniePropertiesGenerator
{
    /// <summary>
    /// Unique Id of this Property
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    public float Probability { get; set; }

    /// <summary>
    /// Weenie Class Id of object to generate
    /// </summary>
    public uint WeenieClassId { get; set; }

    /// <summary>
    /// Amount of delay before generation
    /// </summary>
    public float? Delay { get; set; }

    /// <summary>
    /// Number of object to generate initially
    /// </summary>
    public int InitCreate { get; set; }

    /// <summary>
    /// Maximum amount of objects to generate
    /// </summary>
    public int MaxCreate { get; set; }

    /// <summary>
    /// When to generate the weenie object
    /// </summary>
    public uint WhenCreate { get; set; }

    /// <summary>
    /// Where to generate the weenie object
    /// </summary>
    public uint WhereCreate { get; set; }

    /// <summary>
    /// StackSize of object generated
    /// </summary>
    public int? StackSize { get; set; }

    /// <summary>
    /// Palette Color of Object Generated
    /// </summary>
    public uint? PaletteId { get; set; }

    /// <summary>
    /// Shade of Object generated&apos;s Palette
    /// </summary>
    public float? Shade { get; set; }

    public uint? ObjCellId { get; set; }

    public float? OriginX { get; set; }

    public float? OriginY { get; set; }

    public float? OriginZ { get; set; }

    public float? AnglesW { get; set; }

    public float? AnglesX { get; set; }

    public float? AnglesY { get; set; }

    public float? AnglesZ { get; set; }

    public virtual Weenie Object { get; set; }
}
