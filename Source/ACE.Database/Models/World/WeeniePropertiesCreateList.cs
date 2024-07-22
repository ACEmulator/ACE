using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// CreateList Properties of Weenies
/// </summary>
public partial class WeeniePropertiesCreateList
{
    /// <summary>
    /// Unique Id of this Property
    /// </summary>
    public uint Id { get; set; }

    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Type of Destination the value applies to (DestinationType.????)
    /// </summary>
    public sbyte DestinationType { get; set; }

    /// <summary>
    /// Weenie Class Id of object to Create
    /// </summary>
    public uint WeenieClassId { get; set; }

    /// <summary>
    /// Stack Size of object to create (-1 = infinite)
    /// </summary>
    public int StackSize { get; set; }

    /// <summary>
    /// Palette Color of Object
    /// </summary>
    public sbyte Palette { get; set; }

    /// <summary>
    /// Shade of Object&apos;s Palette
    /// </summary>
    public float Shade { get; set; }

    /// <summary>
    /// Unused?
    /// </summary>
    public bool TryToBond { get; set; }

    public virtual Weenie Object { get; set; }
}
