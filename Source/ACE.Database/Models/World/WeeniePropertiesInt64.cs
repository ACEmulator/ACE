using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World;

/// <summary>
/// Int64 Properties of Weenies
/// </summary>
public partial class WeeniePropertiesInt64
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
    /// Type of Property the value applies to (PropertyInt64.????)
    /// </summary>
    public ushort Type { get; set; }

    /// <summary>
    /// Value of this Property
    /// </summary>
    public long Value { get; set; }

    public virtual Weenie Object { get; set; }
}
