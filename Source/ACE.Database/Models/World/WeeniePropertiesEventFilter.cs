using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// EventFilter Properties of Weenies
/// </summary>
public partial class WeeniePropertiesEventFilter
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
    /// Id of Event to filter
    /// </summary>
    public int Event { get; set; }

    public virtual Weenie Object { get; set; }
}
