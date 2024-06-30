using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard;

/// <summary>
/// EventFilter Properties of Weenies
/// </summary>
public partial class BiotaPropertiesEventFilter
{
    /// <summary>
    /// Id of the object this property belongs to
    /// </summary>
    public uint ObjectId { get; set; }

    /// <summary>
    /// Id of Event to filter
    /// </summary>
    public int Event { get; set; }

    public virtual Biota Object { get; set; }
}
